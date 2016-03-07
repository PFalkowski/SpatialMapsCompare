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
        public SpatialMapsModel(IOService ioService)
        {
            InputOutputService = ioService;
        }
        public IOService InputOutputService { get; }
        public ObservableCollection<C2DPoint> LeftPoly { get; set; } = new ObservableCollection<C2DPoint>();
        public ObservableCollection<C2DPoint> RightPoly { get; set; } = new ObservableCollection<C2DPoint>();

        private Dictionary<string, List<C2DPoint>> Polygons { get; set; } = new Dictionary<string, List<C2DPoint>>();

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

        public void WritePolygonToFile(IList<C2DPoint> poly, string fileName)
        {
            poly.SerializeToXDoc().Save(Path.ChangeExtension(fileName, "xml"));
        }

        public void OpenLeftFile()
        {
            var fileName = InputOutputService.GetFileNameForRead(Environment.CurrentDirectory);
            if (fileName != null)
            {
                var tempPoly = ReadPolygonFromFile(fileName);
                LeftPoly.Clear();
                foreach (var point in tempPoly)
                {
                    LeftPoly.Add(point);
                }
            }
        }
        public void OpenRightFile()
        {
            var fileName = InputOutputService.GetFileNameForRead(Environment.CurrentDirectory);
            if (fileName != null)
            {
                var tempPoly = ReadPolygonFromFile(fileName);
                RightPoly.Clear();
                foreach (var point in tempPoly)
                {
                    RightPoly.Add(point);
                }
            }

            //if (fileName != null) //assigning doesnt work with binding because collection reports it's changes, but property is not itself reporting PropertyChanged event
            //    RightPoly = new ObservableCollection<C2DPoint>(ReadPolygonFromFile(fileName));
        }

        public void SaveLeftFile()
        {
            var fileName = InputOutputService.GetFileNameForWrite(Environment.CurrentDirectory);
            if (fileName != null)
            {
                WritePolygonToFile(LeftPoly, fileName);
            }
        }

        public void SaveRightFile()
        {
            var fileName = InputOutputService.GetFileNameForWrite(Environment.CurrentDirectory);
            if (fileName != null)
            {
                WritePolygonToFile(RightPoly, fileName);
            }
        }
    }
}
