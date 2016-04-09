using System.Collections.Generic;

namespace GeoLib
{
    /// <summary>
    /// Class representing a set of holed polygons.
    /// </summary>
    public class C2DHoledPolyBaseSet : List<C2DHoledPolyBase>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public C2DHoledPolyBaseSet() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~C2DHoledPolyBaseSet() { }
        /// <summary>
        /// Extracts all from the set provided.
        /// </summary>
        /// <param name="Polys">The set to extract from converting to holed polygns.</param>
        public void ExtractAllOf(List<C2DPolyBase> Polys)
        {
            for (var i = 0; i < Polys.Count; i++)
            {
                var NewPoly = new C2DHoledPolyBase();
                NewPoly.Rim = Polys[i];
                Add(NewPoly);
            }

            Polys.Clear();
        }
        /// <summary>
        /// Extracts all from the set provided.
        /// </summary>
        /// <param name="Polys">The set to extract from.</param>
        public void ExtractAllOf(List<C2DHoledPolyBase> Polys)
        {
            this.AddRange(Polys);
            Polys.Clear();
        }
        /// <summary>
        /// Basic multiple unification.
        /// </summary>
        public void UnifyBasic()
        {
	        var TempSet = new C2DHoledPolyBaseSet();
	        var UnionSet = new C2DHoledPolyBaseSet();

	        while (Count > 0)
	        {
		        var pLast = this[Count - 1];
                this.RemoveAt(Count - 1);

		        var bIntersect = false;
		        var i = 0;

                while (i < Count && !bIntersect)
		        {
                    var grid = new CGrid();
                    this[i].GetUnion(pLast, UnionSet, grid);

			        if (UnionSet.Count == 1)
			        {
				        this[i] = UnionSet[0];
				        bIntersect = true;
			        }
			        else
			        {
                        //Debug.Assert(UnionSet.Count == 0);
				        UnionSet.Clear();
				        i++;
			        }
		        }

		        if (!bIntersect)
		        {
			        TempSet.Add(pLast);
		        }
	        }

	        this.AddRange( TempSet);
        }
        /// <summary>
        /// Unification by growing shapes of fairly equal size (fastest for large groups).
        /// </summary>
        /// <param name="grid">The CGrid with the degenerate settings.</param>
        public void UnifyProgressive(CGrid grid)
        {
            // Record the degenerate handling so we can reset.
            CGrid.eDegenerateHandling DegenerateHandling = grid.DegenerateHandling;
	        switch( grid.DegenerateHandling )
	        {
	        case CGrid.eDegenerateHandling.RandomPerturbation:
		        for (var i = 0 ; i < Count ; i++)
		        {
			        this[i].RandomPerturb();
		        }
                    grid.DegenerateHandling = CGrid.eDegenerateHandling.None;
		        break;
	        case CGrid.eDegenerateHandling.DynamicGrid:

		        break;
	        case CGrid.eDegenerateHandling.PreDefinedGrid:
		        for (var i = 0 ; i < Count ; i++)
		        {
			        this[i].SnapToGrid(grid);
		        }
		        grid.DegenerateHandling = CGrid.eDegenerateHandling.PreDefinedGridPreSnapped;
		        break;
	        case CGrid.eDegenerateHandling.PreDefinedGridPreSnapped:

		        break;
	        }


	        var NoUnionSet = new C2DHoledPolyBaseSet();
	        var PossUnionSet = new C2DHoledPolyBaseSet();
	        var SizeHoldSet = new C2DHoledPolyBaseSet();
	        var UnionSet = new C2DHoledPolyBaseSet();
	        var TempSet = new C2DHoledPolyBaseSet();

	        var nThreshold = GetMinLineCount();

	        if (nThreshold == 0)
		        nThreshold = 10;	// avoid infinate loop.
        	
	        // Assumed all are size held to start
	        SizeHoldSet.AddRange(this);
            this.Clear();

	        do
	        {
		        // double the threshold
		        nThreshold *= 3;

		        // Put all the possible intersects back in this.
		        this.AddRange(PossUnionSet);
                PossUnionSet.Clear();

		        // Put all the size held that are small enough back (or in to start with)
		        while (SizeHoldSet.Count > 0)
		        {
			        var pLast = SizeHoldSet[SizeHoldSet.Count - 1];
                    SizeHoldSet.RemoveAt(SizeHoldSet.Count - 1);

			        if (pLast.GetLineCount() > nThreshold)
			        {
				        TempSet.Add(pLast);
			        }
			        else
			        {
				        this.Add(pLast);
			        }
		        }
		        SizeHoldSet.AddRange( TempSet);
                TempSet.Clear();


		        // Cycle through all popping the last and finding a union
		        while (Count > 0)
		        {
			        var pLast = this[Count-1];
        		    this.RemoveAt(Count-1);

			        var bIntersect = false;

			        var i = 0;
			        while ( i < Count && !bIntersect )
			        {
				        this[i].GetUnion( pLast, UnionSet, grid);

				        if (UnionSet.Count == 1)
				        {
					        var pUnion = UnionSet[UnionSet.Count - 1];
                            UnionSet.RemoveAt(UnionSet.Count - 1);

					        if (pUnion.GetLineCount() > nThreshold)
					        {
						        RemoveAt(i);
						        SizeHoldSet.Add(pUnion);
					        }
					        else
					        {
						        this[i] = pUnion;
						        i++;
					        }

					        bIntersect = true;
				        }
				        else
				        {
                            if (UnionSet.Count != 0)
                            {
                                grid.LogDegenerateError();
                            }
					        UnionSet.Clear();
					        i++;
				        }
			        }

			        if (!bIntersect)
			        {
				        var bPosInterSect = false;
				        for (var j = 0 ; j <  SizeHoldSet.Count; j ++)
				        {
					        if (pLast.Rim.BoundingRect.Overlaps( 
								        SizeHoldSet[j].Rim.BoundingRect))
					        {
						        bPosInterSect = true;	
						        break;
					        }
				        }

				        if (bPosInterSect)
				        {
					        PossUnionSet.Add( pLast);
				        }
				        else
				        {
					        NoUnionSet.Add(pLast);
				        }
			        }
		        }
	        }
	        while (SizeHoldSet.Count != 0);


	        this.AddRange( NoUnionSet);
            NoUnionSet.Clear();

            grid.DegenerateHandling = DegenerateHandling;
        }
        /// <summary>
        /// Adds a new polygon and looks for a possible unification.
        /// Assumes current set is distinct.
        /// </summary>
        /// <param name="pPoly">The polygon to add and possible unify.</param>
        public void AddAndUnify(C2DHoledPolyBase pPoly)
        {
            if (!AddIfUnify(pPoly))
                Add(pPoly);
        }
        /// <summary>
        /// Adds a new polygon set and looks for a possible unifications.
        /// Assumes both sets are distinct.
        /// </summary>
        /// <param name="pOther">The polygon set to add and possible unify.</param>
        public void AddAndUnify(C2DHoledPolyBaseSet pOther)
        {
            var TempSet = new C2DHoledPolyBaseSet();

            while (pOther.Count > 0)
            {
                var pLast = pOther[pOther.Count - 1];
                pOther.RemoveAt(pOther.Count - 1);

                if (!AddIfUnify(pLast))
                    TempSet.Add(pLast);
            }

            this.AddRange(TempSet) ;
        }
        /// <summary>
        /// Adds a new polygon ONLY if there is a unifications.
        /// Assumes current set is distinct.
        /// </summary>
        /// <param name="pPoly">The polygon to add if there is a union.</param>
        public bool AddIfUnify(C2DHoledPolyBase pPoly)
        {
            var TempSet = new C2DHoledPolyBaseSet();
            var UnionSet = new C2DHoledPolyBaseSet();
            var grid = new CGrid();
            while (Count > 0 && pPoly != null)
            {
                var pLast = this[Count-1];
                this.RemoveAt(Count - 1);

                pLast.GetUnion(pPoly, UnionSet, grid);

                if (UnionSet.Count == 1)
                {
                    pPoly = null;
                    pLast = null;

                    AddAndUnify(UnionSet[0]);
                    UnionSet.Clear();
                }
                else
                {
                    //Debug.Assert(UnionSet.Count == 0);
                    UnionSet.Clear();
                    TempSet.Add(pLast);
                }
            }

            this.AddRange( TempSet);
            TempSet.Clear();
            
            return (pPoly == null);

        }

        /// <summary>
        /// Adds a set of holes to the current set of polygons as holes within them.
        /// </summary>
        /// <param name="pOther">The polygon set to add as holes.</param>
        public void AddKnownHoles(List<C2DPolyBase> pOther)
        {
	        if (Count != 0)
	        {
		        while (pOther.Count > 0)
		        {
			        var pLast = pOther[pOther.Count - 1];
                    pOther.RemoveAt(pOther.Count - 1);
			        if (pLast.Lines.Count > 0)
			        {
                        var i = Count - 1;
				        var bFound = false;
				        while ( i > 0 && !bFound)
				        {
					        if ( this[i].Contains( pLast.Lines[0].GetPointFrom()))
					        {
                                this[i].AddHole(pLast);
						        bFound = true;
					        }
					        i--;
				        }

				        if (!bFound)
				        {
                            this[0].AddHole(pLast);
				        }
			        }
		        }
	        }
        }

        /// <summary>
        /// Total line count for all polygons contained.
        /// </summary>
        public int GetLineCount()
        {
	        var nResult = 0 ;

	        for (var n = 0; n < Count ; n ++)
	        {
		        nResult += this[n].GetLineCount();
	        }

	        return nResult;
        }

        /// <summary>
        /// Minimum line count of all polygons contained.
        /// </summary>
        public int GetMinLineCount()
        {
	        var nMin = ~(int)0;

	        for(var i = 0 ; i < Count; i++)
	        {
		        var nCount = this[i].GetLineCount();
		        if( nCount < nMin)
		        {
			        nMin = nCount;
		        }
	        }
	        return nMin;
        }

        /// <summary>
        /// Transformation.
        /// </summary>
        public void Transform(CTransformation pProject)
        {
	        for (var i = 0 ; i <  this.Count; i++)
	        {
		        this[i].Transform(pProject);
	        }
        }
        /// <summary>
        /// Transformation.
        /// </summary>
        public void InverseTransform(CTransformation pProject)
        {
	        for ( var i = 0 ; i < this.Count; i++)
	        {
		        this[i].InverseTransform(pProject);
	        }
        }
    }
}
