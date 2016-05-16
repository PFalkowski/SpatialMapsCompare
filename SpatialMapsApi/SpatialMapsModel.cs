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
        private Dictionary<string, List<C2DPoint>> Polygons { get; set; } = new Dictionary<string, List<C2DPoint>>();

        public IOService InputOutputService { get; }

        public SpatialMapsModel(IOService ioService)
        {
            InputOutputService = ioService;
        }

        public List<C2DPoint> GetPolyByKey(string polygonKey)
        {
            if (string.IsNullOrEmpty(polygonKey)) return null;
            List<C2DPoint> tempPoly;
            if (Polygons.TryGetValue(polygonKey, out tempPoly))
            {
                return tempPoly;
            }
            else
            {
                throw new KeyNotFoundException($"No polygon with name \"{polygonKey}\" loaded");
            }
        }

        public List<C2DPoint> TryGetPolyByKeySafe(string polygonKey)
        {
            List<C2DPoint> tempPoly;
            Polygons.TryGetValue(polygonKey, out tempPoly);
            return tempPoly;
        }

        private bool IsPolygonValid(IList<C2DPoint> polygon)
        {
            if (polygon?.Count > 2) return true;
            return false;
        }

        public bool IsPolygonValid(string polygonKey)
        {
            if (string.IsNullOrEmpty(polygonKey)) return false;
            var tempPoly = TryGetPolyByKeySafe(polygonKey);
            return IsPolygonValid(tempPoly);
        }

        public bool IsNameUnique(string name)
        {
            return Polygons.ContainsKey(name);
        }

        public string GetUniqueNameForPolygon(string basedOnName)
        {
            if (Polygons.ContainsKey(basedOnName))
            {
                return GetUniqueNameForPolygon(basedOnName + "'");
            }
            else return basedOnName;
        }

        public bool IsPolygonNew(List<C2DPoint> polygon, string name)
        {
            if (Polygons.ContainsKey(name))
            {
                var polyRetreived = Polygons[name];
                if (polygon.SequenceEqual(polyRetreived))
                    return false;
            }
            return true;
        }


        public void Update(string name, List<C2DPoint> polygon)
        {
            if (Polygons.ContainsKey(name))
            {
                Polygons[name] = polygon;
            }
            else
            {
                Polygons.Add(name, polygon);
            }
        }

        public void AddPolygonToDictionary(string name, List<C2DPoint> polygon)
        {
            if (Polygons.ContainsKey(name))
            {
                var polyRetreived = Polygons[name];
                if (!polygon.SequenceEqual(polyRetreived))
                {
                    throw new ArgumentException($"Polygon with name \"{name}\" already exists, but with different data. Change the file name to load it.");
                }
            }
            else Polygons.Add(name, polygon);
        }

        public void AddPolygonToDictionary(KeyValuePair<string, List<C2DPoint>> polygon)
        {
            AddPolygonToDictionary(polygon.Key, polygon.Value);
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
            AddPolygonToDictionary(fileNameWithoutExtension, tempPoly);
            return new KeyValuePair<string, List<C2DPoint>>(fileNameWithoutExtension, tempPoly);
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

        private void WritePolygonToFile(IList<C2DPoint> poly, string fileName)
        {
            poly.SerializeToXDoc().Save(Path.ChangeExtension(fileName, FileType));
        }

        public void WritePolygonToFile(string polyName)
        {
            var poly = GetPolyByKey(polyName);
            var fileName = InputOutputService.GetFileNameForWrite(null, polyName, FilterString);
            if (fileName != null)
            {
                WritePolygonToFile(poly, fileName);
            }
        }

        public double? GetArea(string polygonKey)
        {
            var points = GetPolyByKey(polygonKey);
            if (IsPolygonValid(points))
            {
                var poly = new C2DPolygon(points, true);
                var area = poly.GetArea();
                return Math.Round(area, RoundDigits);
            }
            return null;
        }

        public double? GetPerimeter(string polygonKey)
        {
            var points = GetPolyByKey(polygonKey);
            if (IsPolygonValid(points))
            {
                var poly = new C2DPolygon(points, true);
                var area = poly.GetPerimeter();
                return Math.Round(area, RoundDigits);
            }
            return null;
        }

        public enum IntersectionType
        {
            Overlapping,
            NonOverlapping
        }
        public List<C2DHoledPolygon> GetIntersectingPolygons(string firstPolygonName, string secondPolygonName, IntersectionType whichPolygons)
        {
            var tempLeftPoints = GetPolyByKey(firstPolygonName);
            var tempRightPoints = GetPolyByKey(secondPolygonName);
            var leftPoly = new C2DPolygon(tempLeftPoints, true);
            var rightPoly = new C2DPolygon(tempRightPoints, true);
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

        public double? GetOverlappingArea(string firstPolygonName, string secondPolygonName)
        {
            var polygons = GetIntersectingPolygons(firstPolygonName, secondPolygonName, IntersectionType.Overlapping);
            var area = polygons.Sum(p => p.GetArea());
            return Math.Round(area, RoundDigits);
        }

        public double? GetNonOverlappingArea(string firstPolygonName, string secondPolygonName)
        {
            var polygons = GetIntersectingPolygons(firstPolygonName, secondPolygonName, IntersectionType.NonOverlapping);
            var area = polygons.Sum(p => p.GetArea());
            return Math.Round(area, RoundDigits);
        }

        public IList<C2DPoint> SnapToOrigin(IList<C2DPoint> input)
        {
            IList<C2DPoint> result = new List<C2DPoint>(input);
            SnapToOriginInPlace(result);
            return result;
        }
        public void SnapToOriginInPlace(IList<C2DPoint> input)
        {
            var minX = double.MaxValue;
            var minY = double.MaxValue;
            foreach (var t in input)
            {
                if (t.X < minX)
                    minX = t.x;
                if (t.y < minY)
                    minY = t.y;
            }
            //minX += 0.0000000000001;
            //minX += 0.0000000000001;
            for (var i = 0; i < input.Count; ++i)
            {
                input[i] = new C2DPoint(input[i].X - minX, input[i].Y - minY);
            }
        }
    }
}
