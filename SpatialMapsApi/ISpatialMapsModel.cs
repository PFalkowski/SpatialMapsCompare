using GeoLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpatialMaps
{
    public interface ISpatialMapsModel
    {
        IOService InputOutputService { get; }
        ObservableCollection<C2DPoint> LeftPoly { get; set; }
        ObservableCollection<C2DPoint> RightPoly { get; set; }

        void OpenLeftFile();
        void OpenRightFile();
        void SaveLeftFile();
        void SaveRightFile();
        IList<C2DPoint> ReadPolygonFromFile(string fileName);
    }
}