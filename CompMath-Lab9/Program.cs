using CompMath_Lab9.ODE;

namespace CompMath_Lab9;

public class Program
{
	const double H = 0.05;
	const int Precision = 4;

	static readonly ODEData ODEData = new(
		P: x => 1.5 / x,
		Q: x => -1.0 / x,
		F: x => x + 1
	);

	static readonly BoundsData BoundsData = new(
		X0: 1.2,
		X1: 1.5,

		A0: 1.0,
		B0: 1.0,
		C0: 1.0,

		A1: 2.0,
		B1: -1.0,
		C1: 1.5
	);

	static readonly FunctionData ExactSolution = new(
		F: x => (2.22679 * Math.Exp(2 * Math.Sqrt(x))
			+ 4.76128 * Math.Exp(-2 * Math.Sqrt(x)))
			/ Math.Sqrt(x) - (x + 3) * (x + 3),

		DF: x => (2.22679 * Math.Exp(2 * Math.Sqrt(x)) * (Math.Sqrt(x) - 0.5)
			- 4.76128 * Math.Exp(-2 * Math.Sqrt(x)) * (Math.Sqrt(x) + 0.5))
			/ Math.Pow(x, 1.5) - 2 * (x + 3),

		DDF: x => (2.22679 * Math.Exp(2 * Math.Sqrt(x)) * (x - 1.5 * Math.Sqrt(x) + 0.75)
			+ 4.76128 * Math.Exp(-2 * Math.Sqrt(x)) * (x + 1.5 * Math.Sqrt(x) + 0.75))
			/ Math.Pow(x, 2.5) - 2
	);

	static readonly Dictionary<TestResult, string> ResultMessage = new()
	{
		[TestResult.Pass] = "Test passed",
		[TestResult.EquationFail] = "Equation condition test failed",
		[TestResult.BoundaryFail] = "Boundary conditions test failed"
	};

	static void TestSolution(FunctionData functionData, string solutionName)
	{
		var testResult = Tester.Test(functionData, ODEData, BoundsData, H, Precision);
		Console.WriteLine($"{solutionName} solution: {ResultMessage[testResult]}");
	}

	static void Main()
	{
		Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

		var x = Enumerable.Range(0, (int)((BoundsData.X1 - BoundsData.X0) / H) + 1)
			.Select(k => BoundsData.X0 + k * H);

		var (solution, coefficients) = Collocation.Solve(ODEData, BoundsData, H);

		var y = new Dictionary<string, IEnumerable<double>>()
		{
			["Exact"] = x.Select(x => ExactSolution.F(x)),
			["Collocation"] = x.Select(x => solution.F(x))
		};

		Drawer.DrawTable(x, "x", y, "y(x)", Precision);
		Console.WriteLine();

		Console.WriteLine("Tests:");
		TestSolution(solution, "Collocation");
		TestSolution(ExactSolution, "Exact");
		Console.WriteLine();

		Console.WriteLine("Orthogonal functions coefficients:");
		Console.WriteLine(string.Join(Environment.NewLine, coefficients.Select((c, i) => $"c{i + 1} = {c}")));

		Console.ReadLine();
	}
}