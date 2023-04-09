namespace CompMath_Lab9.ODE;

public static class LinearSystem
{
	public static (double, double) Solve2(double a00, double a01, double b0, double a10, double a11, double b1)
	{
		double d = a00 * a11 - a01 * a10;
		if (d == 0.0)
		{
			throw new ArgumentException("Matrix is singular");
		}
		return ((a11 * b0 - a01 * b1) / d, (a00 * b1 - a10 * b0) / d);
	}

	public static double[] Solve(double[][] matrix)
	{
		int m = matrix.Length;
		int n = m + 1;
		if (matrix.Any(row => row.Length != n))
		{
			throw new ArgumentException("Matrix has wrong dimensions", nameof(matrix));
		}

		for (int k = 0; k < m; k++)
		{
			int kMax = matrix
				.Skip(k)
				.Select((row, i) => (First: row[k], I: i))
				.MaxBy(t => Math.Abs(t.First))
				.I + k;

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
		return matrix.Select(row => row[m]).ToArray();
	}
}
