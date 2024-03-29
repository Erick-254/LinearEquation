﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LinearEquation
{
    class Matrix
	{
		private List<LinearEquation> listOfEquations = new List<LinearEquation>();

		private int width = 0;
		public int Width
		{
			get
			{
				return this.width;
			}
		}

		public int Length
		{
			get
			{
				return listOfEquations.Count;
			}
		}

		public LinearEquation getEquation(int index)
		{
			return listOfEquations[index];
		}

		public LinearEquation setEquation(int index, LinearEquation e)
		{
			return listOfEquations[index] = e;
		}

		public void ForwardElimination()
		{
			int N0 = this.Length;
			int N1 = this.Width;

			for (int k = 0; k < N0; k++)
			{
				for (int i = k + 1; i < N1 - 1; i++)
				{
					double factor = this.getEquation(i).Array[k] / this.getEquation(k).Array[k];

					for (int j = k; j < N1; j++)
						this.getEquation(i).Array[j] -= factor * this.getEquation(k).Array[j];
				}
			}
		}

		private double[] solveMatrix()
		{
			int length = this.getEquation(0).Array.Length;

			for (int i = 0; i < this.Length - 1; i++)
			{
				if (this.getEquation(i).Array[i] == 0 && !pivotProcedure(this, i, i))
					throw new Exception("Unable to calculate");
				for (int j = i; j < this.Length; j++)
				{
					double[] d = new double[length];

					for (int x = 0; x < length; x++)
					{
						d[x] = this.getEquation(j).Array[x];
						if (this.getEquation(j).Array[i] != 0)
							d[x] = d[x] / this.getEquation(j).Array[i];
					}
					this.setEquation(j, new LinearEquation(d));
				}
				for (int y = i + 1; y < this.Length; y++)
				{
					double[] f = new double[length];
					for (int g = 0; g < length; g++)
					{
						f[g] = this.getEquation(y).Array[g];
						if (this.getEquation(y).Array[i] != 0)
							f[g] = f[g] - this.getEquation(i).Array[g];
					}
					this.setEquation(y, new LinearEquation(f));
				}
			}

			return backwardSubstitution(this);
		}

		private bool pivotProcedure(Matrix matrix, int row, int column)
		{
			bool swapped = false;
			for (int z = matrix.Length - 1; z > row; z--)
			{
				if (matrix.getEquation(z).Array[row] != 0)
				{
					double[] temp = new double[matrix.getEquation(0).Array.Length];
					temp = matrix.getEquation(z).Array;
					matrix.setEquation(z, matrix.getEquation(column));
					matrix.setEquation(column, new LinearEquation(temp));
					swapped = true;
				}
			}

			return swapped;
		}
		public double[] backwardSubstitution(Matrix matrix)
		{
			double val = 0;
			int length = matrix.getEquation(0).Array.Length;
			double[] result = new double[matrix.Length];
			for (int i = matrix.Length - 1; i >= 0; i--)
			{
				val = matrix.getEquation(i).Array[length - 1];
				for (int x = length - 2; x > i - 1; x--)
				{
					if (result.Length <= x || matrix.getEquation(i).Array.Length <= x)
						break;

					val -= matrix.getEquation(i).Array[x] * result[x];
				}
				result[i] = val / matrix.getEquation(i).Array[i];

				if (!Validate(result[i]))
					throw new Exception("Unable to calculate");
			}
			return result;
		}
		private bool Validate(double result)
		{
			return !(double.IsNaN(result) || double.IsInfinity(result));
		}

		public void printResults()
		{
			double[] result = this.solveMatrix();

			for (int i = 0; i < result.Length; i++)
				Console.WriteLine($"x{i + 1} = {result[i]}");
		}

		public void addEquation(LinearEquation e)
		{
			if (this.listOfEquations.Count == 0)
				this.width = e.Array.Length;

			if (e.Array.Length != this.width)
				throw new Exception("Invalid equation added, wrong dimensions!");

			this.listOfEquations.Add(e);
		}

		public void removeEquation(LinearEquation e)
		{
			this.listOfEquations.Remove(e);
		}

		public override string ToString()
		{
			String res = "";
			foreach (LinearEquation e in listOfEquations)
				res += String.Join(" ", e.Array) + "\n";

			return res;
		}
	}
}
