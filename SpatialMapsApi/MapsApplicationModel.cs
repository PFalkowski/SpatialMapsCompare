using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialMaps
{
    public class MapsApplicationModel
    {

        public Dictionary<string, Polygon> Polygons { get; set; } =  new Dictionary<string, Polygon>();

        public Polygon ReadPolygonFromFile(string fileName)
        {
            Polygon tempPoly = null;
            bool flagSameName = false;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
            {
                throw new ArgumentException($"The path choosen (\"{fileName}\") does not contain valid file name");
            }
            else if (Polygons.ContainsKey(fileNameWithoutExtension))
            {
                flagSameName = true;
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
            if (flagSameName)
            {
                var areIdentical = true;
                var polyRetreived = Polygons[fileNameWithoutExtension];
                if (tempPoly.Points.Count == polyRetreived.Points.Count)
                {
                    using (var poly1Iter = tempPoly.Points.GetEnumerator())
                    using (var poly2Iter = polyRetreived.Points.GetEnumerator())
                    {
                        while(poly1Iter.MoveNext() && poly2Iter.MoveNext())
                        {
                            areIdentical = false;
                        }
                    }
                    if (areIdentical) return polyRetreived;
                    else throw new ArgumentException($"Polygon with name \"{fileNameWithoutExtension}\" already exists, but with different data. Change the file name to load it.");
                }
            }
            Polygons.Add(fileNameWithoutExtension, tempPoly);
            return tempPoly;
        }
    }
}
