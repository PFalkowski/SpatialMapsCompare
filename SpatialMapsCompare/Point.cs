using System;

namespace WindowsFormsApplication4
{
	public class Point
	{
		private double _x;

		private double _y;

		public double x
		{
			get
			{
				return this._x;
			}
			set
			{
				this._x = value;
			}
		}

		public double y
		{
			get
			{
				return this._y;
			}
			set
			{
				this._y = value;
			}
		}

		public Point(double a, double b)
		{
			this._x = a;
			this._y = b;
		}

		public Point(object a, object b)
		{
			try
			{
				if (a != null)
				{
					this._x = Convert.ToDouble(a);
				}
				else
				{
					this._x = 0.0;
				}
				if (b != null)
				{
					this._y = Convert.ToDouble(b);
				}
				else
				{
					this._y = 0.0;
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

		public double distance(Point another)
		{
			return Math.Sqrt(Math.Pow(this.x - another.x, 2.0) + Math.Pow(this.y - another.y, 2.0));
		}
	}
}
