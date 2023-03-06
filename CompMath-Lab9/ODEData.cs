namespace CompMath_Lab9;

public readonly struct ODEData
{
	public Function P { get; init; }
	public Function Q { get; init; }
	public Function F { get; init; }

	public double L(SequenceFunctionData fd, double x, int i) => fd.DDF(x, i) + P(x) * fd.DF(x, i) + Q(x) * fd.F(x, i);
}
