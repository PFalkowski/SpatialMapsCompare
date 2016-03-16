using GeoLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpatialMaps
{
    public interface ISpatialMapsModel
    {
        string FileType { get; set; }
        string FilterString { get; }
        IOService InputOutputService { get; }
        IList<C2DPoint> ReadPolygonFromFile(string fileName);//todo switch to key string handling instead of IList<C2DPoint>
        void WritePolygonToFile(string polyName);
        bool IsPolygonValid(string polygonKey);
        double? GetArea(string polygonKey);
        void AddPolygonToDictionary(List<C2DPoint> polygon, string name);
        string GetUniqueNameForPolygon(string basedOnName);
        bool IsPolygonNew(List<C2DPoint> polygon, string name);
    }
}