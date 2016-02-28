using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLib
{

    /// <summary>
    /// Class to represent a 2D rectangle.
    /// </summary>
    public class C2DRect : C2DBase
    {

        /// <summary>
        /// Constructor.
        /// </summary>
	    public C2DRect() {}

        /// <summary>
        /// Destructor.
        /// </summary>
	    ~C2DRect() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Other">The other rect.</param>   
        public C2DRect(C2DRect Other)
        {
            TopLeft.Set(Other.TopLeft);
            BottomRight.Set(Other.BottomRight);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ptTopLeft">The top left point.</param>  
        /// <param name="ptBottomRight">The bottom right point.</param>  
	    public C2DRect(C2DPoint ptTopLeft, C2DPoint ptBottomRight)
        {
            TopLeft.Set(ptTopLeft);
            BottomRight.Set(ptBottomRight);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dLeft">Left.</param>  
        /// <param name="dTop">Top.</param>  
        /// <param name="dRight">Right.</param>  
        /// <param name="dBottom">Bottom.</param>  
	    public C2DRect(double dLeft, double dTop, double dRight, double dBottom)
        {
            TopLeft.X = dLeft;
            TopLeft.Y = dTop;

            BottomRight.X = dRight;
            BottomRight.Y = dBottom;
        }

        /// <summary>
        /// Constructor sets both the top left and bottom right to equal the rect.
        /// </summary>
        /// <param name="pt">Point.</param>  
	    public C2DRect(C2DPoint pt )
        {
            TopLeft.Set(pt);
            BottomRight.Set(pt);
        }

        /// <summary>
        /// Sets both the top left and bottom right to equal the rect.
        /// </summary>
        /// <param name="pt">Point.</param>  
	    public void Set( C2DPoint pt)
        {
            TopLeft.Set(pt);
            BottomRight.Set(pt);
        }

        /// <summary>
        /// Assignment.
        /// </summary>
        /// <param name="ptTopLeft">The top left point.</param>  
        /// <param name="ptBottomRight">The bottom right point.</param>  
        public void Set(C2DPoint ptTopLeft, C2DPoint ptBottomRight)
        {
            TopLeft.Set(ptTopLeft);
            BottomRight.Set(ptBottomRight);
        }

        /// <summary>
        /// Assignment.
        /// </summary>
        /// <param name="dLeft">Left.</param>  
        /// <param name="dTop">Top.</param>  
        /// <param name="dRight">Right.</param>  
        /// <param name="dBottom">Bottom.</param>  
        public void Set(double dLeft, double dTop, double dRight, double dBottom)
        {
            TopLeft.X = dLeft;
            TopLeft.Y = dTop;

            BottomRight.X = dRight;
            BottomRight.Y = dBottom;
        }

        /// <summary>
        /// Assignment.
        /// </summary>
        /// <param name="dTop">Top.</param>  
	    public void SetTop(double dTop) 
        {
            TopLeft.Y = dTop; 
        }


        /// <summary>
        /// Assignment.
        /// </summary>
        /// <param name="dLeft">Left.</param>  
	    public void SetLeft(double dLeft) 
        {
            TopLeft.X = dLeft; 
        }

        /// <summary>
        /// Assignment.
        /// </summary>
        /// <param name="dBottom">Bottom.</param>  
	    public void SetBottom(double dBottom) 
        {
            BottomRight.Y = dBottom; 
        }


        /// <summary>
        /// Assignment.
        /// </summary>
        /// <param name="dRight">Right.</param>  
	    public void SetRight(double dRight) 
        {
            BottomRight.X = dRight; 
        }

        /// <summary>
        /// Clears the rectangle.
        /// </summary>
        public void Clear()
        {
            TopLeft.X = 0;
            TopLeft.Y = 0;
            BottomRight.X = 0;
            BottomRight.Y = 0;
        }

        /// <summary>
        /// Expands to include the point.
        /// </summary>
        /// <param name="NewPt">Point.</param> 
	    public void ExpandToInclude(C2DPoint NewPt)
        {
            if (NewPt.X > BottomRight.X) 
                BottomRight.X = NewPt.X;
            else if (NewPt.X < TopLeft.X) 
                TopLeft.X = NewPt.X;
            if (NewPt.Y > TopLeft.Y) 
                TopLeft.Y = NewPt.Y;
            else if (NewPt.Y < BottomRight.Y) 
                BottomRight.Y = NewPt.Y;
        }

        /// <summary>
        /// Expands to include the rectangle.
        /// </summary>
        /// <param name="Other">Rectangle.</param> 
	    public void ExpandToInclude(C2DRect Other)
        {
            ExpandToInclude(Other.TopLeft);
            ExpandToInclude(Other.BottomRight);
        }

        /// <summary>
        /// True if there is an overlap, returns the overlap.
        /// </summary>
        /// <param name="Other">Rectangle.</param> 
        /// <param name="Overlap">Output. The overlap.</param> 
        public bool Overlaps(C2DRect Other, C2DRect Overlap)
        {
            C2DPoint ptOvTL = new C2DPoint();
            C2DPoint ptOvBR = new C2DPoint();

            ptOvTL.Y = Math.Min(TopLeft.Y, Other.TopLeft.Y);
            ptOvBR.Y = Math.Max(BottomRight.Y, Other.BottomRight.Y);

            ptOvTL.X = Math.Max(TopLeft.X, Other.TopLeft.X);
            ptOvBR.X = Math.Min(BottomRight.X, Other.BottomRight.X);

            Overlap.Set(ptOvTL, ptOvBR);

            return Overlap.IsValid();
        }

        /// <summary>
        /// True if the point is within the rectangle.
        /// </summary>
        /// <param name="Pt">Point.</param> 
	    public bool Contains(C2DPoint Pt)
        {
            return (Pt.X >= TopLeft.X && Pt.X <= BottomRight.X &&
                     Pt.Y <= TopLeft.Y && Pt.Y >= BottomRight.Y);
        }


        /// <summary>
        /// True if the entire other rectangle is within.
        /// </summary>
        /// <param name="Other">Other rectangle.</param> 
	    public bool Contains(C2DRect Other)
        {
            return (Other.GetLeft() > TopLeft.X &&
                      Other.GetRight() < BottomRight.X &&
                      Other.GetBottom() > BottomRight.Y &&
                      Other.GetTop() < TopLeft.Y);
        }

        /// <summary>
        /// True if there is an overlap.
        /// </summary>
        /// <param name="Other">Other rectangle.</param> 
	    public bool Overlaps(C2DRect Other)
        {
            bool bOvX = !(Other.GetLeft() >= BottomRight.X ||
                          Other.GetRight() <= TopLeft.X);

            bool bOvY = !(Other.GetBottom() >= TopLeft.Y ||
                          Other.GetTop() <= BottomRight.Y);

            return bOvX && bOvY;
        }

        /// <summary>
        /// If the area is positive e.g. the top is greater than the bottom.
        /// </summary>
	    public bool IsValid()
        {
            return ((TopLeft.X < BottomRight.X) && (TopLeft.Y > BottomRight.Y));
        }

        /// <summary>
        /// Returns the area.
        /// </summary>
        public double GetArea()
        {
            return ((TopLeft.Y - BottomRight.Y) * (BottomRight.X - TopLeft.X));
        }

        /// <summary>
        /// Returns the width.
        /// </summary>
	    public double Width()
        {
            return (BottomRight.X - TopLeft.X);
        }

        /// <summary>
        /// Returns the height.
        /// </summary>
        public double Height()
        {
            return (TopLeft.Y - BottomRight.Y);
        }

        /// <summary>
        /// Returns the top.
        /// </summary>
	    public double GetTop( ) 
        {
            return  TopLeft.Y;
        }

        /// <summary>
        /// Returns the left.
        /// </summary>
	    public double GetLeft( )   
        {
            return  TopLeft.X ;
        }


        /// <summary>
        /// Returns the bottom.
        /// </summary>
	    public double GetBottom( )  
        {
            return BottomRight.Y;
        }

        /// <summary>
        /// Returns the right.
        /// </summary>
	    public double GetRight( )  
        {
            return BottomRight.X ;
        }

        /// <summary>
	    /// Assignment.
        /// </summary>
        /// <param name="Other">Other rectangle.</param> 
	    public void Set(C2DRect Other)
        {
            TopLeft.X = Other.TopLeft.X;
            TopLeft.Y = Other.TopLeft.Y;
            BottomRight.X = Other.BottomRight.X;
            BottomRight.Y = Other.BottomRight.Y;
        }

        /// <summary>
        /// Grows it from its centre.
        /// </summary>
        /// <param name="dFactor">Factor to grow by.</param> 
        public void Grow(double dFactor)
        {
            C2DPoint ptCentre = new C2DPoint(GetCentre());

            BottomRight.X = (BottomRight.X - ptCentre.X) * dFactor + ptCentre.X;
            BottomRight.Y = (BottomRight.Y - ptCentre.Y) * dFactor + ptCentre.Y;

            TopLeft.X = (TopLeft.X - ptCentre.X) * dFactor + ptCentre.X;
            TopLeft.Y = (TopLeft.Y - ptCentre.Y) * dFactor + ptCentre.Y;

        }

        /// <summary>
        /// Grow the height it from its centre.
        /// </summary>
        /// <param name="dFactor">Factor to grow by.</param> 
        public void GrowHeight(double dFactor)
        {
            C2DPoint ptCentre = new C2DPoint(GetCentre());
            BottomRight.Y = (BottomRight.Y - ptCentre.Y) * dFactor + ptCentre.Y;
            TopLeft.Y = (TopLeft.Y - ptCentre.Y) * dFactor + ptCentre.Y;

        }

        /// <summary>
        /// Grows the width from its centre.
        /// </summary>
        /// <param name="dFactor">Factor to grow by.</param> 
        public void GrowWidth(double dFactor)
        {
            C2DPoint ptCentre = new C2DPoint(GetCentre());
            BottomRight.X = (BottomRight.X - ptCentre.X) * dFactor + ptCentre.X;
            TopLeft.X = (TopLeft.X - ptCentre.X) * dFactor + ptCentre.X;

        }

        /// <summary>
        /// Expands from the centre by the fixed amount given.
        /// </summary>
        /// <param name="dRange">Amount to expand by.</param> 
        public void Expand(double dRange)
        {
            BottomRight.X += dRange;
            BottomRight.Y -= dRange;

            TopLeft.X -= dRange;
            TopLeft.Y += dRange;
        }

        /// <summary>
        /// Grows it from the given point.
        /// </summary>
        /// <param name="dFactor">Factor to grow by.</param> 
        /// <param name="Origin">The origin.</param> 
        public override void Grow(double dFactor, C2DPoint Origin)
        {
            BottomRight.Grow(dFactor, Origin);
            TopLeft.Grow(dFactor, Origin);
        }

        /// <summary>
        /// Moves this point by the vector given.
        /// </summary>
        /// <param name="Vector">The vector.</param>
        public override void Move(C2DVector Vector)
        {
            TopLeft.Move(Vector);
            BottomRight.Move(Vector);
        }

        /// <summary>
        /// Reflect throught the point given. 
        /// Switches Top Left and Bottom Right to maintain validity.
        /// </summary>
        /// <param name="Point">Reflection point.</param> 
        public override void Reflect(C2DPoint Point)
        {
            TopLeft.Reflect(Point);
            BottomRight.Reflect(Point);

            double x = TopLeft.X;
            double y = TopLeft.Y;

            TopLeft.Set( BottomRight);
            BottomRight.X = x;
            BottomRight.Y = y;
        }

        /// <summary>
        /// Reflect throught the line by reflecting the centre of the 
        /// rect and keeping the validity.
        /// </summary>
        /// <param name="Line">Reflection Line.</param> 
        public override void Reflect(C2DLine Line)
        {
	        C2DPoint ptCen = new C2DPoint(this.GetCentre());
	        C2DPoint ptNewCen = new C2DPoint(ptCen);
	        ptNewCen.Reflect(Line);
	        C2DVector vec = new C2DVector(ptCen, ptNewCen);
	        Move(vec);
        }

        /// <summary>
        /// Rotates this to the right about the origin provided.
        /// Note that as the horizontal/vertical line property will be
        /// preserved. If you rotate an object and its bounding box, the box may not still
        /// bound the object.
        /// </summary>
        /// <param name="dAng">The angle through which to rotate.</param>
        /// <param name="Origin">The origin about which to rotate.</param>
        public override void RotateToRight(double dAng, C2DPoint Origin)
        {
            double dHalfWidth = Width() / 2;
            double dHalfHeight = Height() / 2;

            C2DPoint ptCen = new C2DPoint(GetCentre());
            ptCen.RotateToRight(dAng, Origin);

            TopLeft.X = ptCen.X - dHalfWidth;
            TopLeft.Y = ptCen.Y + dHalfHeight;
            BottomRight.X = ptCen.X + dHalfWidth;
            BottomRight.Y = ptCen.Y - dHalfHeight;
        }

        /// <summary>
        /// Returns the distance from this to the point. 0 if the point inside.
        /// </summary>
        /// <param name="TestPoint">Test Point.</param> 
        public override double Distance(C2DPoint TestPoint)
        {
            if (TestPoint.X > BottomRight.X) // To the east half
            {
                if (TestPoint.Y > TopLeft.Y)			// To the north east
                    return TestPoint.Distance(new C2DPoint(BottomRight.X, TopLeft.Y));
                else if (TestPoint.Y < BottomRight.Y)		// To the south east
                    return TestPoint.Distance(BottomRight);
                else
                    return (TestPoint.X - BottomRight.X);	// To the east
            }
            else if (TestPoint.X < TopLeft.X)	// To the west half
            {
                if (TestPoint.Y > TopLeft.Y)			// To the north west
                    return TestPoint.Distance(TopLeft);
                else if (TestPoint.Y < BottomRight.Y)		// To the south west
                    return TestPoint.Distance(new C2DPoint(TopLeft.X, BottomRight.Y));
                else
                    return (TopLeft.X - TestPoint.X);	// To the west
            }
            else
            {
                if (TestPoint.Y > TopLeft.Y)		//To the north
                    return (TestPoint.Y - TopLeft.Y);
                else if (TestPoint.Y < BottomRight.Y)	// To the south
                    return (BottomRight.Y - TestPoint.Y);
            }

          //  assert(Contains(TestPoint));
            return 0;	// Inside
        }

        /// <summary>
        /// Returns the distance from this to the other rect. 0 if there is an overlap.
        /// </summary>
        /// <param name="Other">Other rectangle.</param> 
       public double Distance(C2DRect Other)
       {
	        if (this.Overlaps(Other))
		        return 0;

	        if (Other.GetLeft() > this.BottomRight.X)
	        {
		        // Other is to the right
		        if (Other.GetBottom() > this.TopLeft.Y)
		        {
			        // Other is to the top right
			        C2DPoint ptTopRight = new C2DPoint(BottomRight.X,  TopLeft.Y);
			        return ptTopRight.Distance(new C2DPoint(Other.GetLeft(), Other.GetBottom()));
		        }
		        else if (Other.GetTop() < this.BottomRight.Y)
		        {
			        // Other to the bottom right
			        return BottomRight.Distance( Other.TopLeft );
		        }
		        else
		        {
			        // to the right
			        return Other.GetLeft() - this.BottomRight.X;
		        }
	        }
	        else if ( Other.GetRight() < this.TopLeft.X)
	        {
		        // Other to the left
		        if (Other.GetBottom() > this.TopLeft.Y)
		        {
			        // Other is to the top left
			        return  TopLeft.Distance(Other.BottomRight);
		        }
		        else if (Other.GetTop() < this.BottomRight.Y)
		        {
			        // Other to the bottom left
			        C2DPoint ptBottomLeft = new C2DPoint(TopLeft.X, BottomRight.Y);
			        return ptBottomLeft.Distance ( new C2DPoint( Other.GetRight(), Other.GetTop()));
		        }
		        else
		        {
			        //Just to the left
			        return (this.TopLeft.X - Other.GetRight());
		        }
	        }
	        else
	        {
		        // There is horizontal overlap;
		        if (Other.GetBottom() >  TopLeft.Y)
			        return Other.GetBottom() -  TopLeft.Y;
		        else
			        return BottomRight.Y - Other.GetTop();
	        }		

        }

        /// <summary>
        /// Returns the bounding rectangle. (Required for virtual base class).
        /// </summary>
        /// <param name="Rect">Ouput. Bounding rectangle.</param> 
        public override void GetBoundingRect(C2DRect Rect) 
        { 
            Rect.Set(this);
        }

        /// <summary>
        /// Scales the rectangle accordingly.
        /// </summary>
	    public void Scale(C2DPoint ptScale) 
        {
            TopLeft.X =  TopLeft.X * ptScale.X;
            TopLeft.Y = TopLeft.Y * ptScale.Y; 

		    BottomRight.X = BottomRight.X * ptScale.X;
            BottomRight.Y = BottomRight.Y * ptScale.Y;
        }

        /// <summary>
        /// Returns the centre.
        /// </summary>
        public C2DPoint GetCentre()
        {
            return BottomRight.GetMidPoint(TopLeft);
        }

        /// <summary>
        /// Returns the point which is closest to the origin (0,0).
        /// </summary>
        public C2DPoint GetPointClosestToOrigin()
        {
            C2DPoint ptResult = new C2DPoint();
            if (Math.Abs(TopLeft.X) < Math.Abs(BottomRight.X))
            {
                // Left is closest to the origin.
                ptResult.X = TopLeft.X;
            }
            else
            {
                // Right is closest to the origin
                ptResult.X = BottomRight.X;
            }

            if (Math.Abs(TopLeft.Y) < Math.Abs(BottomRight.Y))
            {
                // Top is closest to the origin.
                ptResult.Y = TopLeft.Y;
            }
            else
            {
                // Bottom is closest to the origin
                ptResult.Y = BottomRight.Y;
            }

            return ptResult;
        }

        /// <summary>
        /// Returns the point which is furthest from the origin (0,0).
        /// </summary>
        public C2DPoint GetPointFurthestFromOrigin()
        {
            C2DPoint ptResult = new C2DPoint();
            if (Math.Abs(TopLeft.X) > Math.Abs(BottomRight.X))
            {
                // Left is furthest to the origin.
                ptResult.X = TopLeft.X;
            }
            else
            {
                // Right is furthest to the origin
                ptResult.X = BottomRight.X;
            }

            if (Math.Abs(TopLeft.Y) > Math.Abs(BottomRight.Y))
            {
                // Top is furthest to the origin.
                ptResult.Y = TopLeft.Y;
            }
            else
            {
                // Bottom is furthest to the origin
                ptResult.Y = BottomRight.Y;
            }

            return ptResult;
        }

        /// <summary>
        /// Projection onto the line
        /// </summary>
        /// <param name="Line">Line to project on.</param> 
        /// <param name="Interval">Ouput. Projection.</param> 
        public override void Project(C2DLine Line,  CInterval Interval)
        {
	        this.TopLeft.Project( Line,  Interval);
	        Interval.ExpandToInclude( BottomRight.Project( Line));
	        C2DPoint TR = new C2DPoint( BottomRight.X,   TopLeft.Y);
            C2DPoint BL = new C2DPoint( TopLeft.X, BottomRight.Y);
	        Interval.ExpandToInclude( TR.Project( Line));
	        Interval.ExpandToInclude( BL.Project( Line));

        }

        /// <summary>
        /// Projection onto the Vector.
        /// </summary>
        /// <param name="Vector">Vector to project on.</param> 
        /// <param name="Interval">Ouput. Projection.</param> 
        public override void Project(C2DVector Vector,  CInterval Interval)
        {
	        this.TopLeft.Project( Vector,  Interval);
	        Interval.ExpandToInclude( BottomRight.Project( Vector));
	        C2DPoint TR = new C2DPoint( BottomRight.X,   TopLeft.Y);
            C2DPoint BL = new C2DPoint(TopLeft.X, BottomRight.Y);
	        Interval.ExpandToInclude( TR.Project( Vector));
	        Interval.ExpandToInclude( BL.Project( Vector));

        }

        /// <summary>
        /// Snaps this to the conceptual grid.
        /// </summary>
        /// <param name="grid">Grid to snap to.</param> 
        public override void SnapToGrid(CGrid grid)
        {
            TopLeft.SnapToGrid(grid);
            BottomRight.SnapToGrid(grid);

        }



        /// <summary>
        /// True if this is above or below the other
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool OverlapsVertically( C2DRect Other)
        {
	        return !(Other.GetLeft() >= BottomRight.X ||
				          Other.GetRight() <=  TopLeft.X);
        }


        /// <summary>
        /// True if this is above the other.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool OverlapsAbove( C2DRect Other)
        {
	        if (Other.GetLeft() >= BottomRight.X ||
				          Other.GetRight() <=  TopLeft.X)
	        {
		        return false;
	        }
	        else 
	        {
		        return TopLeft.Y > Other.GetBottom();
	        }
        }


        /// <summary>
        /// True if this is below the other.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool OverlapsBelow( C2DRect Other)
        {
	        if (Other.GetLeft() >= BottomRight.X ||
				          Other.GetRight() <=  TopLeft.X)
	        {
		        return false;
	        }
	        else 
	        {
		        return BottomRight.Y < Other.GetTop();
	        }
        }


        /// <summary>
        /// Top left.
        /// </summary>
        private C2DPoint topLeft = new C2DPoint();
        /// <summary>
        /// Top left.
        /// </summary>
        public C2DPoint TopLeft
        {
            get
            {
                return topLeft;
            }

        }
        /// <summary>
        /// Bottom right.
        /// </summary>
        private C2DPoint bottomRight = new C2DPoint();
        /// <summary>
        /// Bottom right.
        /// </summary>
        public C2DPoint BottomRight
        {
            get
            {
                return bottomRight;
            }
        }
    }
}
