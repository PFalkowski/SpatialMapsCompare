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

namespace DravingCanvas
{
    public class DravingCanvasViewModel : BindableBase
    {
        public bool drawingIsOn { get; set; }
        public Point startPoint { get; set; }
        public ObservableCollection<C2DPoint> Points { get; set; } = new ObservableCollection<C2DPoint>();

    }
}
