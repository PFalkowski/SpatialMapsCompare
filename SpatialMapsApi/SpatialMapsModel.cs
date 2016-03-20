using GeoLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialMaps
{
    using Polygon = PolygonAdapter;
    public class SpatialMapsModel : ISpatialMapsModel
    {
        public string FileType { get; set; } = "xml";
        public string FilterString => $"{FileType.ToUpper()} Files|*.{FileType.ToLower()};";
        public int RoundDigits { get; set; } = 1;
        private Dictionary<string, Polygon> Polygons { get; set; } = new Dictionary<string, Polygon>();

        public IOService InputOutputService { get; }

        public SpatialMapsModel(IOService ioService)
        {
            InputOutputService = ioService;
        }

        public Polygon GetPolyByKey(string polygonKey)
        {
            if (string.IsNullOrEmpty(polygonKey)) return null;
            Polygon tempPoly;
            if (Polygons.TryGetValue(polygonKey, out tempPoly))
            {
                return tempPoly;
            }
            else
            {
                throw new KeyNotFoundException($"No polygon with name \"{polygonKey}\" loaded");
            }
        }

        public Polygon TryGetPolyByKeySafe(string polygonKey)
        {
            Polygon tempPoly = null;
            Polygons.TryGetValue(polygonKey, out tempPoly);
            return tempPoly;
        }

        private bool IsPolygonValid(Polygon polygon)
        {
            if (polygon?.C2DPoly?.Lines.Count > 2) return true;
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

        public bool IsPolygonNew(Polygon polygon, string name)
        {
            if (Polygons.ContainsKey(name))
            {
                var polyRetreived = Polygons[name];
                if (polygon.IsValueEqual(polyRetreived)) return false;
            }
            return true;
        }


        public void Update(string name, Polygon polygon)
        {
            if(Polygons.ContainsKey(name))
            {
                Polygons[name] = polygon;
            }
            else
            {
                Polygons.Add(name, polygon);
            }
        }

        public void AddPolygonToDictionary(string name, Polygon polygon)
        {
            if (Polygons.ContainsKey(name))
            {
                var polyRetreived = Polygons[name];
                if (Enumerable.SequenceEqual(polygon.C2DPoly.Lines, polyRetreived.C2DPoly.Lines))
                {
                    throw new ArgumentException($"Polygon with name \"{name}\" already exists, but with different data. Change the file name to load it.");
                }
            }
            else Polygons.Add(name, polygon);
        }

        public void AddPolygonToDictionary(KeyValuePair<string, Polygon> polygon)
        {
            AddPolygonToDictionary(polygon.Key, polygon.Value);
        }

        public KeyValuePair<string, Polygon> GetPolygonFromFile(string fileName)
        {
            Polygon tempPoly = null;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
            {
                throw new ArgumentException($"The path choosen (\"{fileName}\") does not contain valid file name");
            }
            else
            {
                try
                {
                    tempPoly = Helper.DeserializeFromXml<Polygon>(fileName);
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
            return new KeyValuePair<string, Polygon>(fileNameWithoutExtension, tempPoly);
        }

        public KeyValuePair<string, Polygon> GetPolygonUsingIOService()
        {
            var fileName = InputOutputService.GetFileNameForRead(null, null, FilterString);
            var temp = new KeyValuePair<string, Polygon>();
            if (fileName != null)
            {
                temp = GetPolygonFromFile(fileName);
            }
            return temp;
        }

        private void WritePolygonToFile(Polygon poly, string fileName)
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
            var poly = GetPolyByKey(polygonKey);
            if (IsPolygonValid(poly))
            {
                var area = poly.C2DPoly.GetArea();
                return Math.Round(area, RoundDigits);
            }
            return null;
        }

        public double? GetPerimeter(string polygonKey)
        {
            var poly = GetPolyByKey(polygonKey);
            if (IsPolygonValid(poly))
            {
                var area = poly.C2DPoly.GetPerimeter();
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
            var leftPoly = GetPolyByKey(firstPolygonName);
            var rightPoly = GetPolyByKey(secondPolygonName);
            var someGrid = new CGrid();
            var smallPolygons = new List<C2DHoledPolygon>();
            switch (whichPolygons)
            {
                case IntersectionType.Overlapping:
                    leftPoly.C2DPoly.GetOverlaps(rightPoly, smallPolygons, someGrid);
                    break;
                case IntersectionType.NonOverlapping:
                    leftPoly.C2DPoly.GetNonOverlaps(rightPoly, smallPolygons, someGrid);
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
    }
}
