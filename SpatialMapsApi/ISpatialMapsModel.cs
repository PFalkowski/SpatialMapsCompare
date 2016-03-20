using GeoLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpatialMaps
{
    using Polygon = PolygonAdapter;
    public interface ISpatialMapsModel
    {
        Polygon GetPolyByKey(string polygonKey);
        Polygon TryGetPolyByKeySafe(string polygonKey);
        string FileType { get; set; }
        string FilterString { get; }
        IOService InputOutputService { get; }
        KeyValuePair<string, Polygon> GetPolygonFromFile(string fileName);
        KeyValuePair<string, Polygon> GetPolygonUsingIOService();
        void WritePolygonToFile(string polyName);
        bool IsPolygonValid(string polygonKey);
        void AddPolygonToDictionary(string name, Polygon polygon);
        string GetUniqueNameForPolygon(string basedOnName);
        bool IsPolygonNew(Polygon polygon, string name);
        void Update(string name, Polygon list);
        double? GetArea(string polygonKey);
        double? GetPerimeter(string rightPolyName);
        double? GetOverlappingArea(string leftPolygonName, string rightPolygonName);
        double? GetNonOverlappingArea(string leftPolygonName, string rightPolygonName);
    }
}