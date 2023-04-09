namespace CompMath_Lab9.ODE;

public record BoundsData(
	double X0,
	double X1,
	double A0,
	double B0,
	double C0,
	double A1,
	double B1,
	double C1)
{

	public double L0(FunctionData fd, double x) => A0 * fd.F(x) + B0 * fd.DF(x);
	public double L1(FunctionData fd, double x) => A1 * fd.F(x) + B1 * fd.DF(x);
}
