using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoLib;

namespace SpatialMapsCompare
{
    [Serializable]
    public class Polygon
    {
        public string Name { get; set; }
        public List<C2DPoint> Points { get; set; }

        public Polygon() {}
        public Polygon(string name, IEnumerable<C2DPoint> points)
        {
            Name = name;
            Points = points.ToList();
        }

        public static implicit operator List<C2DPoint>(Polygon polygon)
        {
            return polygon.Points;
        }
        public static implicit operator C2DPolygon(Polygon polygon)
        {
            return new C2DPolygon(polygon.Points, true);
        }
    }
}
