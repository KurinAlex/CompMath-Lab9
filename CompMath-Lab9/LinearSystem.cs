namespace CompMath_Lab9;

public static class LinearSystem
{
	public static (double, double) Solve2(double a00, double a01, double b0, double a10, double a11, double b1)
	{
		double q = a00 * a11 - a01 * a10;
		return ((a11 * b0 - a01 * b1) / q, (a00 * b1 - a10 * b0) / q);
	}

	public static double[] Solve(double[][] a, double[] b)
	{
		int m = a.Length;
		if (a.Any(row => row.Length != m))
		{
			throw new ArgumentException("Matrix is not square");
		}
		if (m != b.Length)
		{
			throw new ArgumentException("Matrixes have different number of rows");
		}

		double[][] matrix = a
			.Zip(b)
			.Select(t => t.First.Append(t.Second).ToArray())
			.ToArray();
		int n = m + 1;

		for (int k = 0; k < m; k++)
		{
			int kMax = matrix
				.Skip(k)
				.Select((row, i) => (row[k], i))
				.MaxBy(t => Math.Abs(t.Item1))
				.Item2 + k;

			if (matrix[kMax][k] == 0.0)
			{
				throw new ArgumentException("Matrix is singular");
			}

			if (k != kMax)
			{
				(matrix[k], matrix[kMax]) = (matrix[kMax], matrix[k]);
			}

			double f = matrix[k][k];
			for (int j = k; j < n; j++)
			{
				matrix[k][j] /= f;
			}

			for (int i = 0; i < m; i++)
			{
				if (i == k)
				{
					continue;
				}

				f = matrix[i][k] / matrix[k][k];
				for (int j = k; j < n; j++)
				{
					matrix[i][j] -= matrix[k][j] * f;
				}
			}
		}
		return matrix.Select(row => row.Last()).ToArray();
	}
}
