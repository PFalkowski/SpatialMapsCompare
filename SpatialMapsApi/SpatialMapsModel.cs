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
    public class SpatialMapsModel : ISpatialMapsModel
    {
        public string FileType { get; set; } = "xml";
        public string FilterString => $"{FileType.ToUpper()} Files|*.{FileType.ToLower()};";
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
            List<C2DPoint> tempPoly = null;
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
                if (Enumerable.SequenceEqual(polygon, polyRetreived))
                    return false;
            }
            return true;
        }

        public void AddPolygonToDictionary(List<C2DPoint> polygon, string name)
        {
            if (Polygons.ContainsKey(name))
            {
                var polyRetreived = Polygons[name];
                if (Enumerable.SequenceEqual(polygon, polyRetreived))
                {
                    throw new ArgumentException($"Polygon with name \"{name}\" already exists, but with different data. Change the file name to load it.");
                }
            }
            else Polygons.Add(name, polygon);
        }

        public void AddPolygonToDictionary(KeyValuePair<string, List<C2DPoint>> polygon)
        {
            AddPolygonToDictionary(polygon.Value, polygon.Key);
        }

        public KeyValuePair<string, List<C2DPoint>> GetPolygonFromFile(string fileName)
        {
            List<C2DPoint> tempPoly = null;
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
            AddPolygonToDictionary(tempPoly, fileNameWithoutExtension);
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
            poly.SerializeToXDoc().Save(Path.ChangeExtension(fileName, "xml"));
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
                return Math.Round(area, 3);
            }
            return null;
        }
    }
}
