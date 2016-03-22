using GeoLib;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;

namespace DrawingCanvas
{
    public class DrawingCanvasViewModel : BindableBase
    {
        public DrawingCanvasViewModel()
        {
            FileName = FileNamePlaceholder;
        }
        public C2DPoint StartingPoint { get; set; }
        public bool IsFirstPoint { get; set; } = true;
        public string FileNamePlaceholder { get; set; } = "Polygon";
        private string _fileName;
        public string FileName
        {
            get
            { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }
        public List<C2DPoint> Points { get; set; } = new List<C2DPoint>();
    }
}
