using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

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
        public C2DPoint() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="other">Point to which this will be assigned.</param>
        public C2DPoint(C2DPoint other) 
        { 
            X = other.X; 
            Y = other.Y; 
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dx">The x value of the point.</param>
        /// <param name="dy">The y value of the point.</param>
	    public C2DPoint(double dx, double dy) 
        { 
            X = dx; 
            Y = dy; 
        }

        /// <summary>
        /// Sets the x and y values of the point.
        /// </summary>
        /// <param name="dx">The x value of the point.</param>
        /// <param name="dy">The y value of the point.</param>
	    public void Set(double dx, double dy) 
        { 
            X = dx; 
            Y = dy;
        }

        /// <summary>
        /// Assignment to another point.
        /// </summary>
        /// <param name="pt">The point to assign to.</param>
        public void Set(C2DPoint pt)
        {
            X = pt.X;
            Y = pt.Y;
        }

        /// <summary>
        /// Construction from a vector which can be thought of as a point (and vice versa).
        /// </summary>
        /// <param name="vector">Vector to assign to.</param>
        public C2DPoint( C2DVector vector) 
        { 
            X = vector.i; 
            Y = vector.j; 
        }

        /// <summary>
        /// Returns the mid point between this and the other as a new object.
        /// </summary>
        /// <param name="other">Another point.</param>
	    public C2DPoint GetMidPoint(C2DPoint other)
        {
            return new C2DPoint((X + other.X) / 2, (Y + other.Y) / 2);
        }

        /// <summary>
        /// Projects the point on the vector given returning a distance along the vector.
        /// </summary>
        /// <param name="vector">The vector to project this on.</param>
	    public double Project(C2DVector vector) 
        {
	        C2DVector vecthis = new C2DVector(X, Y);

	        return (vecthis.Dot(vector)) / vector.GetLength();
        }

        /// <summary>
        /// Projects the point on the line given returning a distance along the line from the start.
        /// </summary>
        /// <param name="line">The line to project this on.</param>
        public double Project(C2DLine line)
        {
	        C2DVector vecthis = new C2DVector(line.point, this );

	        return (vecthis.Dot(line.vector)) / line.vector.GetLength();
        }

        /// <summary>
        /// Projects the point on the vector given returning a distance along the vector.
        /// </summary>
        /// <param name="vector">The vector to project this on.</param>
        /// <param name="interval">The interval to recieve the result. 
        /// Both the min and max of the interval will be set to the result.</param>
        public override void Project(C2DVector vector,  CInterval interval)
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
	    public override void Project(C2DLine line,  CInterval interval)
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
	    public bool ProjectsOnLine(C2DLine line,  C2DPoint ptOnLine , 
		    ref bool bAbove)
        {
            C2DVector vecthis = new C2DVector(X - line.point.X, Y - line.point.Y);
	        double dProj = vecthis.Dot(line.vector);

	        if (dProj < 0)
	        {
		        bAbove = false;
		        return false;
	        }

	        double dLength = line.vector.GetLength();

	        dProj /= dLength;

	        if (dProj > dLength)
	        {
		        bAbove = true;
		        return false;
	        }

	        double dFactor = dProj / dLength;
    		
	        C2DVector vProj = new C2DVector(line.vector);
	        vProj.Multiply(dFactor);
	        ptOnLine.Set(line.point.X + vProj.i, line.point.Y + vProj.j);
	        return true;

        }

        /// <summary>
        /// Returns the distance between this and the other point.
        /// </summary>
        /// <param name="other">The point to return the distance to.</param>
        public override double Distance(C2DPoint other)
        {
	        double dXd = X - other.X;
	        double dYd = Y - other.Y;

            return Math.Sqrt( dXd * dXd + dYd * dYd );
        }

        /// <summary>
        /// Returns a vector from this to the other point as a new object.
        /// </summary>
        /// <param name="pointTo">The point that vector is to go to.</param>
	    public C2DVector MakeVector(C2DPoint pointTo)
        {
            return new C2DVector(pointTo.X - X, pointTo.Y - Y);
        }


        /// <summary>
        /// Returns the point that the vector supplied would take this point to as a new object.
        /// </summary>
        /// <param name="v1">The vector from this to the new point.</param>
	    public C2DPoint GetPointTo(C2DVector v1)
        {
            return new C2DPoint(X + v1.i, Y + v1.j);
        }

        /// <summary>
        /// Adds 2 points together.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        public static C2DPoint operator +(C2DPoint p1, C2DPoint p2) 
        {
            C2DPoint result = new C2DPoint(p1.X + p2.X, p1.Y + p2.Y);
            return result;
        }

        /// <summary>
        /// Takes 1 point from the other.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        public static C2DPoint operator -(C2DPoint p1, C2DPoint p2) 
        {
            C2DPoint result = new C2DPoint(p1.X - p2.X, p1.Y - p2.Y);
            return result;
        }

        /// <summary>
        /// Multiplies the point by the factor.
        /// </summary>
        /// <param name="dFactor">The multiplication factor.</param>
	    public void Multiply(double dFactor)
        {
            X *= dFactor;
            Y *= dFactor;
        }

        /// <summary>
        /// Divides the point by the factor.
        /// </summary>
        /// <param name="dFactor">The divisor.</param>
	    public void Divide(double dFactor)
        {
            X /= dFactor;
            Y /= dFactor;
        }

        /// <summary>
        /// Equality test which really tests for point proximity.
        /// <seealso cref="Constants.conEqualityTolerance"/> 
        /// </summary>
        /// <param name="other">The other point.</param>
	    public bool PointEqualTo( C2DPoint other)
        {
	        bool bxClose;
	        bool byClose;

	        if ( X == 0 )
                bxClose = other.X == 0;
	        else
                bxClose = Math.Abs((other.X - X) / X) < Constants.conEqualityTolerance;

	        if (!bxClose)
		        return false;		// Get out early if we can.

	        if ( Y == 0 )
                byClose = other.Y == 0;
	        else
                byClose = Math.Abs((other.Y - Y) / Y) < Constants.conEqualityTolerance;

	        return ( byClose );		// We know x is close.
        }

        /// <summary>
        /// Moves this point by the vector given.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public override void Move(C2DVector vector)
        {
            X += vector.i;
            Y += vector.j;
        }

        /// <summary>
        /// Rotates this to the right about the origin provided.
        /// </summary>
        /// <param name="dAng">The angle through which to rotate.</param>
        /// <param name="origin">The origin about which to rotate.</param>
        public override void RotateToRight(double dAng, C2DPoint origin)
        {
	        C2DVector vector = new C2DVector(origin, this);
	        vector.TurnRight(dAng);
	        X = origin.X + vector.i;
	        Y = origin.Y + vector.j;
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
	        C2DVector vector = new C2DVector(origin, this);
	        vector.Multiply(dFactor);
	        X = origin.X + vector.i;
	        Y = origin.Y + vector.j;

        }

        /// <summary>
        /// Reflects this through the point given.
        /// </summary>
        /// <param name="other">The point to reflect this through.</param>
        public override void Reflect(C2DPoint other)
        {
	        // Set up a vector from this to the other
	        C2DVector vec = new C2DVector(this, other);
	        // Now use the vector to find the reflection by continuing along it from the point given.
	        this.Set(  other.GetPointTo(vec) );
        }

        /// <summary>
        /// Reflects this through the line given.
        /// </summary>
        /// <param name="line">The line to reflect this through.</param>
	    public override  void Reflect(C2DLine line)
        {
	        // First find the point along the line that this projects onto.
	        // Make a vector from the point on the line given to this point.
	        C2DVector vecthis = new C2DVector(line.point, this );
	        // Find the length of the line given.
	        double dLength = line.vector.GetLength();
	        // Now make the projection of this point on the line.
	        double dProj = vecthis.Dot(line.vector);
	        dProj /= dLength;
	        // Find the factor along the line that the projection is.
	        double dFactor = dProj / dLength;
	        // Now set up a copy of the vector of the line given.
	        C2DVector vProj = new C2DVector(line.vector);
	        // Multiply that by that factor calculated.
	        vProj.Multiply(dFactor);
	        // Use the vector to find the point on the line.
	        C2DPoint ptOnLine = new C2DPoint(line.point.GetPointTo(vProj) );
	        // Noe simply reflect this in the point.
	        this.Reflect(ptOnLine);
        }

        /// <summary>
        /// Reflects through the y axis.
        /// </summary>
	    public void  ReflectY()
        {
	        X = -X;
        }
        /// <summary>
        /// Reflects through the x axis.
        /// </summary>
	    public void  ReflectX()
        {
            Y = -Y;
        }

        /// <summary>
        /// Returns the bounding rectangle which will be set to this point.
        /// </summary>
        /// <param name="rect">The rectangle to recieve the result.</param>
        public override void GetBoundingRect( C2DRect rect) 
        {
            rect.Set(this);
        }


        /// <summary>
        /// Snaps this to the grid. The x and y values can only be multiples or the grid size.
        /// </summary>
        /// <param name="grid">The grid to snap this to.</param>
        public override void SnapToGrid(CGrid grid)
        {
            double dxMultiple = Math.Abs(X / grid.GridSize) + 0.5;

            dxMultiple = Math.Floor(dxMultiple);

            if (X < 0)
                X = -dxMultiple * grid.GridSize;
            else
                X = dxMultiple * grid.GridSize;

            double dyMultiple = Math.Abs(Y / grid.GridSize) + 0.5;

	        dyMultiple = Math.Floor(dyMultiple);

            if (Y < 0) 
                Y = -dyMultiple * grid.GridSize;
            else
                Y = dyMultiple * grid.GridSize;
        }

        /// <summary>
        /// True if the point projects onto the line given and returns the 
        /// point on the line.
        /// </summary>
        /// <param name="line"></param>
        public void ProjectOnRay(C2DLine line)
        {
	        C2DVector vecthis = new C2DVector(line.point, this );
	        double dProj = vecthis.Dot(line.vector);

	        double dFactor = dProj / (line.vector.i * line.vector.i + line.vector.j * line.vector.j );
        	
	        C2DVector vProj = new C2DVector(line.vector);
	        vProj.i *= dFactor;
            vProj.j *= dFactor;
            this.Set(line.point.X + vProj.i, line.point.Y + vProj.j);
        }


        /// <summary>
        /// The x component of the point.
        /// </summary>
        public double X;
        /// <summary>
        /// The y component of the point.
        /// </summary>
        public double Y;

    }
}
