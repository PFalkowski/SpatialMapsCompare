using GeoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialMaps
{
    public class PolygonAdapter
    {
        public string Name { get; set; }
        public List<C2DPoint> C2DPoints;
        public C2DPolygon C2DPoly;
        public bool IsValueEqual(PolygonAdapter other)
        {
            throw new NotImplementedException();
        }
    }
}
