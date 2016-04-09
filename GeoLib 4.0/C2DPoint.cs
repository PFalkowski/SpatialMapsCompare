using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace GeoLib
{
    /// <summary>
    /// Class to represent a 2 dimensional point in cartesian co-ordinates.
    /// </summary>
    [Serializable]
    public class C2DPoint : C2DBase, ISerializable
    {
        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected C2DPoint(SerializationInfo info, StreamingContext context)
        {
            X = (double)info.GetValue("x", typeof(double));
            Y = (double)info.GetValue("y", typeof(double));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", X);
            info.AddValue("y", Y);
        }
        /// <summary>
        /// GetObjectData for serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("x", X);
            info.AddValue("y", Y);
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public C2DPoint() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="other">Point to which this will be assigned.</param>
        public C2DPoint(C2DPoint other)
        {
            x = other.x;
            y = other.y;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dx">The x value of the point.</param>
        /// <param name="dy">The y value of the point.</param>
	    public C2DPoint(double dx, double dy)
        {
            x = dx;
            y = dy;
        }

        /// <summary>
        /// Sets the x and y values of the point.
        /// </summary>
        /// <param name="dx">The x value of the point.</param>
        /// <param name="dy">The y value of the point.</param>
	    public void Set(double dx, double dy)
        {
            x = dx;
            y = dy;
        }

        /// <summary>
        /// Assignment to another point.
        /// </summary>
        /// <param name="pt">The point to assign to.</param>
        public void Set(C2DPoint pt)
        {
            x = pt.x;
            y = pt.y;
        }

        /// <summary>
        /// Construction from a vector which can be thought of as a point (and vice versa).
        /// </summary>
        /// <param name="vector">Vector to assign to.</param>
        public C2DPoint(C2DVector vector)
        {
            x = vector.i;
            y = vector.j;
        }

        /// <summary>
        /// Returns the mid point between this and the other as a new object.
        /// </summary>
        /// <param name="other">Another point.</param>
	    public C2DPoint GetMidPoint(C2DPoint other)
        {
            return new C2DPoint((x + other.x) / 2, (y + other.y) / 2);
        }

        /// <summary>
        /// Projects the point on the vector given returning a distance along the vector.
        /// </summary>
        /// <param name="vector">The vector to project this on.</param>
	    public double Project(C2DVector vector)
        {
            var vecthis = new C2DVector(x, y);

            return (vecthis.Dot(vector)) / vector.GetLength();
        }

        /// <summary>
        /// Projects the point on the line given returning a distance along the line from the start.
        /// </summary>
        /// <param name="line">The line to project this on.</param>
        public double Project(C2DLine line)
        {
            var vecthis = new C2DVector(line.point, this);

            return (vecthis.Dot(line.vector)) / line.vector.GetLength();
        }

        /// <summary>
        /// Projects the point on the vector given returning a distance along the vector.
        /// </summary>
        /// <param name="vector">The vector to project this on.</param>
        /// <param name="interval">The interval to recieve the result. 
        /// Both the min and max of the interval will be set to the result.</param>
        public override void Project(C2DVector vector, CInterval interval)
        {
            interval.dMax = Project(vector);
            interval.dMin = interval.dMax;
        }

        /// <summary>
        /// Projects the point on the vector given returning a distance along the vector.
        /// </summary>
        /// <param name="line">The vector to project this on.</param>
        /// <param name="interval">The interval to recieve the result. 
        /// Both the min and max of the interval will be set to the result.</param>
	    public override void Project(C2DLine line, CInterval interval)
        {
            interval.dMax = Project(line);
            interval.dMin = interval.dMax;
        }

        /// <summary>
        /// True if the point projects onto the line given and returns the point on the line.
        /// Also returns whether the line projects above or below the line if relevant.
        /// </summary>
        /// <param name="line">The line to project this on.</param>
        /// <param name="ptOnLine">The point to recieve the result.</param>
        /// <param name="bAbove">The flag to indicate whether it projects above or below.</param>
	    public bool ProjectsOnLine(C2DLine line, C2DPoint ptOnLine,
            ref bool bAbove)
        {
            var vecthis = new C2DVector(x - line.point.x, y - line.point.y);
            var dProj = vecthis.Dot(line.vector);

            if (dProj < 0)
            {
                bAbove = false;
                return false;
            }

            var dLength = line.vector.GetLength();

            dProj /= dLength;

            if (dProj > dLength)
            {
                bAbove = true;
                return false;
            }

            var dFactor = dProj / dLength;

            var vProj = new C2DVector(line.vector);
            vProj.Multiply(dFactor);
            ptOnLine.Set(line.point.x + vProj.i, line.point.y + vProj.j);
            return true;

        }

        /// <summary>
        /// Returns the distance between this and the other point.
        /// </summary>
        /// <param name="other">The point to return the distance to.</param>
        public override double Distance(C2DPoint other)
        {
            var dXd = x - other.x;
            var dYd = y - other.y;

            return Math.Sqrt(dXd * dXd + dYd * dYd);
        }

        /// <summary>
        /// Returns a vector from this to the other point as a new object.
        /// </summary>
        /// <param name="pointTo">The point that vector is to go to.</param>
	    public C2DVector MakeVector(C2DPoint pointTo)
        {
            return new C2DVector(pointTo.x - x, pointTo.y - y);
        }


        /// <summary>
        /// Returns the point that the vector supplied would take this point to as a new object.
        /// </summary>
        /// <param name="v1">The vector from this to the new point.</param>
	    public C2DPoint GetPointTo(C2DVector v1)
        {
            return new C2DPoint(x + v1.i, y + v1.j);
        }

        /// <summary>
        /// Adds 2 points together.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        public static C2DPoint operator +(C2DPoint p1, C2DPoint p2)
        {
            var result = new C2DPoint(p1.x + p2.x, p1.y + p2.y);
            return result;
        }

        /// <summary>
        /// Takes 1 point from the other.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        public static C2DPoint operator -(C2DPoint p1, C2DPoint p2)
        {
            var result = new C2DPoint(p1.x - p2.x, p1.y - p2.y);
            return result;
        }

        /// <summary>
        /// Multiplies the point by the factor.
        /// </summary>
        /// <param name="dFactor">The multiplication factor.</param>
	    public void Multiply(double dFactor)
        {
            x *= dFactor;
            y *= dFactor;
        }

        /// <summary>
        /// Divides the point by the factor.
        /// </summary>
        /// <param name="dFactor">The divisor.</param>
	    public void Divide(double dFactor)
        {
            x /= dFactor;
            y /= dFactor;
        }

        /// <summary>
        /// Equality test which really tests for point proximity.
        /// <seealso cref="Constants.conEqualityTolerance"/> 
        /// </summary>
        /// <param name="other">The other point.</param>
	    public bool PointEqualTo(C2DPoint other)
        {
            bool bxClose;
            bool byClose;

            if (x == 0)
                bxClose = other.x == 0;
            else
                bxClose = Math.Abs((other.x - x) / x) < Constants.conEqualityTolerance;

            if (!bxClose)
                return false;       // Get out early if we can.

            if (y == 0)
                byClose = other.y == 0;
            else
                byClose = Math.Abs((other.y - y) / y) < Constants.conEqualityTolerance;

            return (byClose);		// We know x is close.
        }

        /// <summary>
        /// Moves this point by the vector given.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public override void Move(C2DVector vector)
        {
            x += vector.i;
            y += vector.j;
        }

        /// <summary>
        /// Rotates this to the right about the origin provided.
        /// </summary>
        /// <param name="dAng">The angle through which to rotate.</param>
        /// <param name="origin">The origin about which to rotate.</param>
        public override void RotateToRight(double dAng, C2DPoint origin)
        {
            var vector = new C2DVector(origin, this);
            vector.TurnRight(dAng);
            x = origin.x + vector.i;
            y = origin.y + vector.j;
        }

        /// <summary>
        /// Grows this about the origin provided. 
        /// In the case of a point this will just move it away (or closer) 
        /// to the origin as there is no shape to grow.
        /// </summary>
        /// <param name="dFactor">The factor to grow this by.</param>
        /// <param name="origin">The origin about which to rotate.</param>
        public override void Grow(double dFactor, C2DPoint origin)
        {
            var vector = new C2DVector(origin, this);
            vector.Multiply(dFactor);
            x = origin.x + vector.i;
            y = origin.y + vector.j;

        }

        /// <summary>
        /// Reflects this through the point given.
        /// </summary>
        /// <param name="other">The point to reflect this through.</param>
        public override void Reflect(C2DPoint other)
        {
            // Set up a vector from this to the other
            var vec = new C2DVector(this, other);
            // Now use the vector to find the reflection by continuing along it from the point given.
            this.Set(other.GetPointTo(vec));
        }

        /// <summary>
        /// Reflects this through the line given.
        /// </summary>
        /// <param name="line">The line to reflect this through.</param>
	    public override void Reflect(C2DLine line)
        {
            // First find the point along the line that this projects onto.
            // Make a vector from the point on the line given to this point.
            var vecthis = new C2DVector(line.point, this);
            // Find the length of the line given.
            var dLength = line.vector.GetLength();
            // Now make the projection of this point on the line.
            var dProj = vecthis.Dot(line.vector);
            dProj /= dLength;
            // Find the factor along the line that the projection is.
            var dFactor = dProj / dLength;
            // Now set up a copy of the vector of the line given.
            var vProj = new C2DVector(line.vector);
            // Multiply that by that factor calculated.
            vProj.Multiply(dFactor);
            // Use the vector to find the point on the line.
            var ptOnLine = new C2DPoint(line.point.GetPointTo(vProj));
            // Noe simply reflect this in the point.
            this.Reflect(ptOnLine);
        }

        /// <summary>
        /// Reflects through the y axis.
        /// </summary>
	    public void ReflectY()
        {
            x = -x;
        }
        /// <summary>
        /// Reflects through the x axis.
        /// </summary>
	    public void ReflectX()
        {
            y = -y;
        }

        /// <summary>
        /// Returns the bounding rectangle which will be set to this point.
        /// </summary>
        /// <param name="rect">The rectangle to recieve the result.</param>
        public override void GetBoundingRect(C2DRect rect)
        {
            rect.Set(this);
        }


        /// <summary>
        /// Snaps this to the grid. The x and y values can only be multiples or the grid size.
        /// </summary>
        /// <param name="grid">The grid to snap this to.</param>
        public override void SnapToGrid(CGrid grid)
        {
            var dxMultiple = Math.Abs(x / grid.GridSize) + 0.5;

            dxMultiple = Math.Floor(dxMultiple);

            if (x < 0)
                x = -dxMultiple * grid.GridSize;
            else
                x = dxMultiple * grid.GridSize;

            var dyMultiple = Math.Abs(y / grid.GridSize) + 0.5;

            dyMultiple = Math.Floor(dyMultiple);

            if (y < 0)
                y = -dyMultiple * grid.GridSize;
            else
                y = dyMultiple * grid.GridSize;
        }

        /// <summary>
        /// True if the point projects onto the line given and returns the 
        /// point on the line.
        /// </summary>
        /// <param name="line"></param>
        public void ProjectOnRay(C2DLine line)
        {
            var vecthis = new C2DVector(line.point, this);
            var dProj = vecthis.Dot(line.vector);

            var dFactor = dProj / (line.vector.i * line.vector.i + line.vector.j * line.vector.j);

            var vProj = new C2DVector(line.vector);
            vProj.i *= dFactor;
            vProj.j *= dFactor;
            this.Set(line.point.x + vProj.i, line.point.y + vProj.j);
        }


        /// <summary>
        /// The x component of the point. It's for the internal (though cannot be declared internal) use of GeoLib and GeoPolygon libraries. Use property 'X' instead.
        /// </summary>
        public double x;
        /// <summary>
        /// The x component of the point.
        /// </summary>
        [XmlIgnoreAttribute]
        public double X { get { return x; } set { x = value; } }
        /// <summary>
        /// The y component of the point. It's for the internal (though cannot be declared internal) use of GeoLib and GeoPolygon libraries. Use property 'Y' instead.
        /// </summary>
        public double y;
        /// <summary>
        /// The y component of the point.
        /// </summary>
        [XmlIgnoreAttribute]
        public double Y { get { return y; } set { y = value; } }

    }
}
