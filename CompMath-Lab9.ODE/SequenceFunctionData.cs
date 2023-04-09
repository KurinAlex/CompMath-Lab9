namespace CompMath_Lab9.ODE;

public delegate double SequenceFunction(double x, int i);

public record SequenceFunctionData(
	SequenceFunction F,
	SequenceFunction DF,
	SequenceFunction DDF);
