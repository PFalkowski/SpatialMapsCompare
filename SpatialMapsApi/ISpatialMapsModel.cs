using GeoLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpatialMaps
{
    public interface ISpatialMapsModel
    {
        IOService InputOutputService { get; }
        IList<C2DPoint> ReadPolygonFromFile(string fileName);
        void WritePolygonToFile(IList<C2DPoint> poly, string fileName);
    }
}