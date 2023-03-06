namespace CompMath_Lab9;

public class Program
{
	static (double, double)[] SolveColocation(ODEData odeData, BoundsData boundsData, double h)
	{
		double x0 = boundsData.X0;
		double x1 = boundsData.X1;

		var (d, g) = LinearSystem.Solve2(
			x0 * boundsData.A0 + boundsData.B0, boundsData.A0, boundsData.C0,
			x1 * boundsData.A1 + boundsData.B1, boundsData.A1, boundsData.C1);
		var f0 = new SequenceFunctionData
		{
			F = (x, _) => d * x + g,
			DF = (_, _) => d,
			DDF = (_, _) => 0.0
		};

		double w = x1 - x0;
		int n = (int)(w / h) + 1;

		var ss = new string[n];
		var f = new SequenceFunctionData[n];
		for (int j = 0; j < n; j++)
		{
			double ComputeS(int i) => -(boundsData.A1 * w * w + boundsData.B1 * (i + 2) * w)
				/ (boundsData.A1 * w + boundsData.B1 * (i + 1));
			f[j] = new()
			{
				F = (x, i) => Math.Pow(x - x0, i + 1) * (ComputeS(i) + x - x0),
				DF = (x, i) => Math.Pow(x - x0, i) * ((i + 1) * ComputeS(i) + (i + 2) * (x - x0)),
				DDF = (x, i) => Math.Pow(x - x0, i - 1) * (i + 1) * (i * ComputeS(i) + (i + 2) * (x - x0))
			};
		}

		var x = Enumerable.Range(0, n).Select(k => x0 + k * h);
		var a = x.Select(x => f.Select((f, i) => odeData.L(f, x, i + 1)).ToArray()).ToArray();
		var b = x.Select(x => odeData.F(x) - odeData.L(f0, x, 0)).ToArray();
		var c = LinearSystem.Solve(a, b);

		double Res(double x) => f0.F(x, 0) +
			c.Zip(f)
				.Select((t, i) => (C: t.First, FD: t.Second, I: i + 1))
				.Sum(t => t.C * t.FD.F(x, t.I));

		/*double Dres(double x) => f0.DF(x, 0) +
			c.Zip(f)
				.Select((t, i) => (C: t.First, FD: t.Second, I: i + 1))
				.Sum(t => t.C * t.FD.DF(x, t.I));

		double DDres(double x) => f0.DDF(x, 0) +
			c.Zip(f)
				.Select((t, i) => (C: t.First, FD: t.Second, I: i + 1))
				.Sum(t => t.C * t.FD.DDF(x, t.I));

		Console.WriteLine(2 * Res(x1) - Dres(x1));
		Console.WriteLine();*/

		return x.Select(x => (x, Res(x))).ToArray();
	}

	static void Main()
	{
		Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

		var odeData = new ODEData
		{
			P = x => 1.5 / x,
			Q = x => -1.0 / x,
			F = x => x + 1
		};

		var boundsData = new BoundsData
		{
			X0 = 1.2,
			X1 = 1.5,

			A0 = 1.0,
			B0 = 1.0,
			C0 = 1.0,

			A1 = 2.0,
			B1 = -1.0,
			C1 = 1.5
		};

		double x0 = 1.2;
		double x1 = 1.5;
		double h = 0.05;

		Function sol = x => (2.22679 * Math.Exp(2 * Math.Sqrt(x)) + 4.76128 * Math.Exp(-2 * Math.Sqrt(x))) / Math.Sqrt(x) - (x + 3) * (x + 3);
		var x = Enumerable.Range(0, (int)((x1 - x0) / h) + 1).Select(k => x0 + k * h);
		var res = x.Select(x => (x, sol(x)));

		Console.WriteLine(string.Join(Environment.NewLine, SolveColocation(odeData, boundsData, h)));
		Console.WriteLine();
		Console.WriteLine(string.Join(Environment.NewLine, res));
		Console.ReadLine();
	}
}