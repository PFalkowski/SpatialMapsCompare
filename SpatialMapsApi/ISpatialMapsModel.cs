using GeoLib;
using System.Collections.Generic;

namespace SpatialMaps
{
    public interface ISpatialMapsModel
    {
        string FileType { get; set; }
        string FilterString { get; }
        int RoundDigits { get; set; }
        IOService InputOutputService { get; }
        bool IsPolygonValid(IList<C2DPoint> polygon);
        List<C2DHoledPolygon> GetIntersectingPolygons(IList<C2DPoint> pointsA, IList<C2DPoint> pointsB, SpatialMapsModel.IntersectionType whichPolygons);
        double? GetOverlappingArea(IList<C2DPoint> pointsA, IList<C2DPoint> pointsB);
        double? GetNonOverlappingArea(IList<C2DPoint> pointsA, IList<C2DPoint> pointsB);
        void SnapToOriginInPlace(IList<C2DPoint> input);
        KeyValuePair<string, List<C2DPoint>> GetPolygonFromFile(string fileName);
        KeyValuePair<string, List<C2DPoint>> GetPolygonUsingIOService();
        void WritePolygonToFile(IList<C2DPoint> poly, string fileName);
        double? GetArea(IList<C2DPoint> points);
        double? GetPerimeter(IList<C2DPoint> points);
        void ScaleInPlace(IList<C2DPoint> points, double size);
    }
}