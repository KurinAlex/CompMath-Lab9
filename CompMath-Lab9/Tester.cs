using CompMath_Lab9.ODE;

namespace CompMath_Lab9;

public class Tester
{
	public static bool DiffInRange(double a, double b, double eps) => Math.Abs(a - b) <= eps;

	public static TestResult Test(
		FunctionData functionData,
		ODEData odeData,
		BoundsData boundsData,
		double h,
		int precision)
	{
		double eps = Math.Pow(10, -precision);
		double x0 = boundsData.X0;
		double x1 = boundsData.X1;

		if (!DiffInRange(boundsData.L0(functionData, x0), boundsData.C0, eps)
			|| !DiffInRange(boundsData.L1(functionData, x1), boundsData.C1, eps))
		{
			return TestResult.BoundaryFail;
		}

		var xArr = Enumerable.Range(1, (int)((x1 - x0) / h) - 1)
			.Select(k => x0 + k * h);
		foreach (var x in xArr)
		{
			if (!DiffInRange(odeData.L(functionData, x), odeData.F(x), eps))
			{
				return TestResult.EquationFail;
			}
		}

		return TestResult.Pass;
	}
}
