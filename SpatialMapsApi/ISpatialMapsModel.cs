using GeoLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpatialMaps
{
    public interface ISpatialMapsModel
    {
        IOService InputOutputService { get; }
        IList<C2DPoint> ReadPolygonFromFile(string fileName);//todo switch to key string handling instead of IList<C2DPoint>
        void WritePolygonToFile(IList<C2DPoint> poly, string polygonKey);//todo switch to key string handling instead of IList<C2DPoint>
        bool IsPolygonValid(string polygonKey);
        double? GetArea(string polygonKey);
        void AddPolygonToDictionary(List<C2DPoint> polygon, string name);
        string GetUniqueNameForPolygon(string basedOnName);
        bool IsPolygonNew(List<C2DPoint> polygon, string name);
    }
}