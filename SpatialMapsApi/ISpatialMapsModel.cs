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
        string ReadPolygonFromFile(string fileName);
        void WritePolygonToFile(string polyName);
        bool IsPolygonValid(string polygonKey);
        double? GetArea(string polygonKey);
        void AddPolygonToDictionary(List<C2DPoint> polygon, string name);
        string GetUniqueNameForPolygon(string basedOnName);
        bool IsPolygonNew(List<C2DPoint> polygon, string name);
    }
}