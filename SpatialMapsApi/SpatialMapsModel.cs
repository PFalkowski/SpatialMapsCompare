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
        private Dictionary<string, List<C2DPoint>> Polygons { get; set; } = new Dictionary<string, List<C2DPoint>>();
        public IOService InputOutputService { get; }

        public SpatialMapsModel(IOService ioService)
        {
            InputOutputService = ioService;
        }


        public IList<C2DPoint> ReadPolygonFromFile(string fileName)
        {
            List<C2DPoint> tempPoly = null;
            bool flagSameName = false;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
            {
                throw new ArgumentException($"The path choosen (\"{fileName}\") does not contain valid file name");
            }
            else
            {
                if (Polygons.ContainsKey(fileNameWithoutExtension))
                {
                    flagSameName = true;
                }
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
            if (flagSameName)
            {
                var areIdentical = true;
                var polyRetreived = Polygons[fileNameWithoutExtension];
                if (tempPoly.Count == polyRetreived.Count)
                {
                    using (var poly1Iter = tempPoly.GetEnumerator())
                    using (var poly2Iter = polyRetreived.GetEnumerator())
                    {
                        while (poly1Iter.MoveNext() && poly2Iter.MoveNext())
                        {
                            if (!poly1Iter.Current.PointEqualTo(poly2Iter.Current))
                            {
                                areIdentical = false;
                            }
                        }
                    }
                    if (areIdentical) return polyRetreived;
                    else throw new ArgumentException($"Polygon with name \"{fileNameWithoutExtension}\" already exists, but with different data. Change the file name to load it.");
                }
            }
            Polygons.Add(fileNameWithoutExtension, tempPoly);
            return tempPoly;
        }
        // TODO change to accept polygon name and select from dictionary
        public void WritePolygonToFile(IList<C2DPoint> poly, string fileName)
        {
            poly.SerializeToXDoc().Save(Path.ChangeExtension(fileName, "xml"));
        }

        // TODO change to accept polygon name and select from dictionary
        public double GetOverlappingArea(IList<C2DPoint> poly1, IList<C2DPoint> poly2)
        {
            var holedPolys = new List<C2DHoledPolygon>();
            throw new NotImplementedException();
        }
        // TODO change to accept polygon name and select from dictionary
        public double GetNonOverlappingArea(IList<C2DPoint> poly1, IList<C2DPoint> poly2)
        {
            var holedPolys = new List<C2DHoledPolygon>();
            throw new NotImplementedException();
        }
        // TODO change to accept polygon name and select from dictionary
        public double GetPolyArea(IList<C2DPoint> poly)
        {
            throw new NotImplementedException();
        }
        // TODO change to accept polygon name and select from dictionary
        public double GetPerimeter(IList<C2DPoint> poly)
        {
            throw new NotImplementedException();
        }
    }
}
