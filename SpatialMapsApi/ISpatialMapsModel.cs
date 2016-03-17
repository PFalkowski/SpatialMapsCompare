using GeoLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpatialMaps
{
    public interface ISpatialMapsModel
    {
        List<C2DPoint> GetPolyByKey(string polygonKey);
        List<C2DPoint> TryGetPolyByKeySafe(string polygonKey);
        string FileType { get; set; }
        string FilterString { get; }
        IOService InputOutputService { get; }
        KeyValuePair<string, List<C2DPoint>> GetPolygonFromFile(string fileName);
        KeyValuePair<string, List<C2DPoint>> GetPolygonUsingIOService();
        void WritePolygonToFile(string polyName);
        bool IsPolygonValid(string polygonKey);
        void AddPolygonToDictionary(List<C2DPoint> polygon, string name);
        string GetUniqueNameForPolygon(string basedOnName);
        bool IsPolygonNew(List<C2DPoint> polygon, string name);
        double? GetArea(string polygonKey);
        double? GetPerimeter(string rightPolyName);
    }
}