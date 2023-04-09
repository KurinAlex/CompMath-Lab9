namespace CompMath_Lab9.ODE;

public static class Collocation
{
	public static (FunctionData, double[]) Solve(ODEData odeData, BoundsData boundsData, double h)
	{
		double x0 = boundsData.X0;
		double x1 = boundsData.X1;
		if (x0 >= x1)
		{
			throw new ArgumentOutOfRangeException(nameof(x0), "x0 must be less than x1");
		}

		double w = x1 - x0;
		if (w < h)
		{
			throw new ArgumentOutOfRangeException(nameof(h), "Step must be less than or equal to width of x interval");
		}

		var (d, g) = LinearSystem.Solve2(
			x0 * boundsData.A0 + boundsData.B0, boundsData.A0, boundsData.C0,
			x1 * boundsData.A1 + boundsData.B1, boundsData.A1, boundsData.C1);

		var f0 = new FunctionData(
			F: x => d * x + g,
			DF: _ => d,
			DDF: _ => 0.0
		);

		int n = (int)(w / h) - 1;

		var ss = new string[n];
		var f = new SequenceFunctionData[n];
		for (int j = 0; j < n; j++)
		{
			double ComputeS(int i) => -(boundsData.A1 * w * w + boundsData.B1 * (i + 2) * w)
				/ (boundsData.A1 * w + boundsData.B1 * (i + 1));
			f[j] = new(
				F: (x, i) => Math.Pow(x - x0, i + 1) * (ComputeS(i) + x - x0),
				DF: (x, i) => Math.Pow(x - x0, i) * ((i + 1) * ComputeS(i) + (i + 2) * (x - x0)),
				DDF: (x, i) => Math.Pow(x - x0, i - 1) * (i + 1) * (i * ComputeS(i) + (i + 2) * (x - x0))
			);
		}

		var x = Enumerable.Range(1, n).Select(k => x0 + k * h);
		var matrix = x.Select(
			x => f.Select((f, i) => odeData.L(f, x, i + 1)).Append(odeData.F(x) - odeData.L(f0, x)).ToArray())
			.ToArray();
		var c = LinearSystem.Solve(matrix);

		var res = new FunctionData(
			F: x => f0.F(x) +
				c.Zip(f)
					.Select((t, i) => (C: t.First, FD: t.Second, I: i + 1))
					.Sum(t => t.C * t.FD.F(x, t.I)),

			DF: x => f0.DF(x) +
				c.Zip(f)
					.Select((t, i) => (C: t.First, FD: t.Second, I: i + 1))
					.Sum(t => t.C * t.FD.DF(x, t.I)),

			DDF: x => f0.DDF(x) +
				c.Zip(f)
					.Select((t, i) => (C: t.First, FD: t.Second, I: i + 1))
					.Sum(t => t.C * t.FD.DDF(x, t.I))
		);

		return (res, c);
	}
}
