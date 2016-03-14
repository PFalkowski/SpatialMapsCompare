using GeoLib;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace DrawingCanvas
{
    public class DrawingCanvasViewModel : BindableBase
    {
        public C2DPoint StartingPoint { get; set; }
        public bool IsFirstPoint { get; set; } = true;
        public List<C2DPoint> Points { get; set; } = new List<C2DPoint>();
    }
}
