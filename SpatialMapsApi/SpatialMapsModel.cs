using GeoLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpatialMaps
{
    public class SpatialMapsModel : ISpatialMapsModel
    {
        public string FileType { get; set; } = "xml";
        public string FilterString => $"{FileType.ToUpper()} Files|*.{FileType.ToLower()};";
        public int RoundDigits { get; set; } = 1;

        public IOService InputOutputService { get; }

        public SpatialMapsModel(IOService ioService)
        {
            InputOutputService = ioService;
        }

        public bool IsPolygonValid(IList<C2DPoint> polygon)
        {
            return polygon?.Count > 2;
        }

        public enum IntersectionType
        {
            Overlapping,
            NonOverlapping
        }

        public List<C2DHoledPolygon> GetIntersectingPolygons(IList<C2DPoint> pointsA, IList<C2DPoint> pointsB, IntersectionType whichPolygons)
        {
            var leftPoly = new C2DPolygon(pointsA.ToList(), true);
            var rightPoly = new C2DPolygon(pointsB.ToList(), true);
            rightPoly.RandomPerturb();
            var someGrid = new CGrid();
            var smallPolygons = new List<C2DHoledPolygon>();
            switch (whichPolygons)
            {
                case IntersectionType.Overlapping:
                    leftPoly.GetOverlaps(rightPoly, smallPolygons, someGrid);
                    break;
                case IntersectionType.NonOverlapping:
                    leftPoly.GetNonOverlaps(rightPoly, smallPolygons, someGrid);
                    break;
                default:
                    throw new ArgumentException(nameof(whichPolygons));
            }
            return smallPolygons;
        }

        public double? GetOverlappingArea(IList<C2DPoint> pointsA, IList<C2DPoint> pointsB)
        {
            var polygons = GetIntersectingPolygons(pointsA, pointsB, IntersectionType.Overlapping);
            var area = polygons.Sum(p => p.GetArea());
            return Math.Round(area, RoundDigits);
        }

        public double? GetNonOverlappingArea(IList<C2DPoint> pointsA, IList<C2DPoint> pointsB)
        {
            var polygons = GetIntersectingPolygons(pointsA, pointsB, IntersectionType.NonOverlapping);
            var area = polygons.Sum(p => p.GetArea());
            return Math.Round(area, RoundDigits);
        }
        
        private Tuple<double, double, double, double> MinMax(IList<C2DPoint> input)
        {
            var minX = double.MaxValue;
            var minY = double.MaxValue;
            var maxX = double.MinValue;
            var maxY = double.MinValue;
            foreach (var t in input)
            {
                if (t.X < minX)
                    minX = t.x;
                if (t.y < minY)
                    minY = t.y;
                if (t.X > maxX)
                    maxX = t.x;
                if (t.y > maxY)
                    maxY = t.y;
            }
            return new Tuple<double, double, double, double>(minX, minY, maxX, maxY);
        }
        public void SnapToOriginInPlace(IList<C2DPoint> input)
        {
            var minXy = MinMax(input);
            for (var i = 0; i < input.Count; ++i)
            {
                input[i] = new C2DPoint(input[i].X - minXy.Item1, input[i].Y - minXy.Item2);
            }
            var poly = new C2DPolygon(input.ToList(), true);
            poly.RandomPerturb();
            var pointsCopy = new C2DPointSet();
            poly.GetPointsCopy(pointsCopy);
            for (var i = 0; i < pointsCopy.Count; ++i)
            {
                input[i] = pointsCopy[i];
            }
        }
        public KeyValuePair<string, List<C2DPoint>> GetPolygonFromFile(string fileName)
        {
            List<C2DPoint> tempPoly;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
            {
                throw new ArgumentException($"The path choosen (\"{fileName}\") does not contain valid file name");
            }
            else
            {
                try
                {
                    tempPoly = Helper.DeserializeFromXml<List<C2DPoint>>(fileName);
                }
                catch (IOException ex)
                {
                    throw new IOException($"Error opening file \"{fileNameWithoutExtension}\": {ex.Message}", ex);
                }
                catch (InvalidOperationException iop)
                {
                    throw new IOException($"The file \"{fileName}\" is not a valid xml file with polygon points data.", iop);
                }
            }
            return new KeyValuePair<string, List<C2DPoint>>(fileNameWithoutExtension, tempPoly);
        }

        public void CleanPoints(List<C2DPoint> poly)
        {
            var polygon = new C2DPolygon(poly, true);
            var pointsCopy = new List<C2DPoint>();
            polygon.GetPointsCopy(pointsCopy);
            for (int i = 0; i < pointsCopy.Count; ++i)
            {
                poly[i] = pointsCopy[i];
            }
        }
        public KeyValuePair<string, List<C2DPoint>> GetPolygonUsingIOService()
        {
            var fileName = InputOutputService.GetFileNameForRead(null, null, FilterString);
            var temp = new KeyValuePair<string, List<C2DPoint>>();
            if (fileName != null)
            {
                temp = GetPolygonFromFile(fileName);
            }
            return temp;
        }

        public void WritePolygonToFile(IList<C2DPoint> poly, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
                poly.SerializeToXDoc().Save(Path.ChangeExtension(fileName, FileType));
        }

        public double? GetArea(IList<C2DPoint> points)
        {
            if (IsPolygonValid(points))
            {
                var poly = new C2DPolygon(points.ToList(), true);
                var area = poly.GetArea();
                return Math.Round(area, RoundDigits);
            }
            return null;
        }

        public double? GetPerimeter(IList<C2DPoint> points)
        {
            if (IsPolygonValid(points))
            {
                var poly = new C2DPolygon(points.ToList(), true);
                var area = poly.GetPerimeter();
                return Math.Round(area, RoundDigits);
            }
            return null;
        }
        public void ScaleInPlace(IList<C2DPoint> points, double size)
        {
            var minXYMaxXY = MinMax(points);
            for (int i = 0; i < points.Count; ++i)
            {
                points[i].X = points[i].X / minXYMaxXY.Item3 * size;
                points[i].Y = points[i].Y / minXYMaxXY.Item3 * size;
            }
        }
    }
}
