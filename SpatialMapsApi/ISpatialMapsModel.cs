using GeoLib;
using System.Collections.Generic;

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
        void AddPolygonToDictionary(string name, List<C2DPoint> polygon);
        string GetUniqueNameForPolygon(string basedOnName);
        bool IsPolygonNew(List<C2DPoint> polygon, string name);
        void Update(string name, List<C2DPoint> list);
        double? GetArea(string polygonKey);
        double? GetPerimeter(string rightPolyName);
        double? GetOverlappingArea(string leftPolygonName, string rightPolygonName);
        double? GetNonOverlappingArea(string leftPolygonName, string rightPolygonName);
        IList<C2DPoint>  SnapToOrigin(IList<C2DPoint> input);
        void SnapToOriginInPlace(IList<C2DPoint> input);
    }
}