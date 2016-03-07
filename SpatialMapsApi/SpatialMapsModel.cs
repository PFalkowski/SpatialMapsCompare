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
        public string LeftPolyName { get; set; }
        public string RightPolyName { get; set; }


        private Dictionary<string, Polygon> Polygons { get; set; } = new Dictionary<string, Polygon>();

        public Polygon ReadPolygonFromFile(string fileName)
        {
            Polygon tempPoly = null;
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
            if (flagSameName)
            {
                var areIdentical = true;
                var polyRetreived = Polygons[fileNameWithoutExtension];
                if (tempPoly.Points.Count == polyRetreived.Points.Count)
                {
                    using (var poly1Iter = tempPoly.Points.GetEnumerator())
                    using (var poly2Iter = polyRetreived.Points.GetEnumerator())
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

        public void WritePolygonToFile(Polygon poly, string fileName)
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
                foreach (var point in tempPoly.Points)
                {
                    LeftPoly.Add(point);
                }
                LeftPolyName = tempPoly.Name;
            }
        }
        public void OpenRightFile()
        {
            var fileName = InputOutputService.GetFileNameForRead(Environment.CurrentDirectory);
            if (fileName != null)
            {
                var tempPoly = ReadPolygonFromFile(fileName);
                RightPoly.Clear();
                foreach (var point in tempPoly.Points)
                {
                    RightPoly.Add(point);
                }
                RightPolyName = tempPoly.Name;
            }

            //if (fileName != null) //assigning doesnt work with binding because collection reports it's changes, but property is not itself reporting PropertyChanged event
            //    RightPoly = new ObservableCollection<C2DPoint>(ReadPolygonFromFile(fileName));
        }

        public void SaveLeftFile()
        {
            var fileName = InputOutputService.GetFileNameForWrite(Environment.CurrentDirectory);
            if (fileName != null)
            {
                WritePolygonToFile(Polygons[LeftPolyName], fileName);
            }
        }

        public void SaveRightFile()
        {
            var fileName = InputOutputService.GetFileNameForWrite(Environment.CurrentDirectory);
            if (fileName != null)
            {
                WritePolygonToFile(Polygons[RightPolyName], fileName);
            }
        }
    }
}
