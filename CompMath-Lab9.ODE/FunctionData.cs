namespace CompMath_Lab9.ODE;

public delegate double Function(double x);

public record FunctionData(
	Function F,
	Function DF,
	Function DDF);
