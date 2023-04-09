namespace CompMath_Lab9.ODE;

public record ODEData(
	Function P,
	Function Q,
	Function F)
{
	public double L(FunctionData fd, double x) => fd.DDF(x) + P(x) * fd.DF(x) + Q(x) * fd.F(x);
	public double L(SequenceFunctionData fd, double x, int i) => fd.DDF(x, i) + P(x) * fd.DF(x, i) + Q(x) * fd.F(x, i);
}
