using System;

namespace WindowsFormsApplication4
{
	public class Point
	{
		private double _x;

		private double _y;

		public double X
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
			}
		}

		public double Y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
			}
		}

		public Point(double a, double b)
		{
			_x = a;
			_y = b;
		}

		public Point(object a, object b)
		{
			try
			{
				if (a != null)
				{
					_x = Convert.ToDouble(a);
				}
				else
				{
					_x = 0.0;
				}
				if (b != null)
				{
					_y = Convert.ToDouble(b);
				}
				else
				{
					_y = 0.0;
				}
			}
			catch (FormatException)
			{
				Console.WriteLine("The {0} value {1} is not recognized as a valid Double value.", a.GetType().Name, a);
			}
			catch (InvalidCastException)
			{
				Console.WriteLine("Conversion of the {0} value {1} to a Double is not supported.", a.GetType().Name, a);
			}
		}

		public double Distance(Point another)
		{
			return Math.Sqrt(Math.Pow(X - another.X, 2.0) + Math.Pow(Y - another.Y, 2.0));
		}
	}
}
