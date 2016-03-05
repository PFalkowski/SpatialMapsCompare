using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoLib;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace SpatialMaps
{
    [Serializable]
    public class Polygon : BindableBase
    {
        public string Name { get; set; }
        public ObservableCollection<C2DPoint> Points { get; set; }

        public Polygon() {}
        public Polygon(string name, IEnumerable<C2DPoint> points)
        {
            Name = name;
            Points = new ObservableCollection<C2DPoint>(points);
        }

        public static implicit operator List<C2DPoint>(Polygon polygon)
        {
            return polygon.Points.ToList();
        }
        public static implicit operator C2DPolygon(Polygon polygon)
        {
            return new C2DPolygon(polygon.Points.ToList(), true);
        }
    }
}
