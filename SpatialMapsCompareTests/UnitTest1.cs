using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using GeoLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpatialMapsCompare;
using SpatialMaps;
using System.Linq;

namespace SpatialMapsCompareTests
{
    [TestClass]
    public class UnitTest1
    {
        private string testFileName = "testSerializationData.xml";
        private static readonly Polygon _referencePolygon = new Polygon("referencePolygon", new List<C2DPoint>
        {
            new C2DPoint(0.50001, 0.750001),
            new C2DPoint(0.250001, 0.75001),
            new C2DPoint(0.250001, 0.5001),
            new C2DPoint(1E-05, 0.5001),
            new C2DPoint(1E-05, 0.25001),
            new C2DPoint(0.250001, 0.25001),
            new C2DPoint(0.250001, 0.0001),
            new C2DPoint(1.0001, 0.0001),
            new C2DPoint(1.0001, 0.5001),
            new C2DPoint(0.5001, 0.5001),
            new C2DPoint(0.5001, 0.7501)
        });

        private static readonly Polygon _oryginalPolygon = new Polygon("oryginalPolygon", new List<C2DPoint>
        {
            new C2DPoint(0.50001, 0.750001),
            new C2DPoint(0.250001, 0.750001),
            new C2DPoint(0.250001, 0.50001),
            new C2DPoint(1E-05, 0.50001),
            new C2DPoint(1E-05, 0.250001),
            new C2DPoint(0.250001, 0.250001),
            new C2DPoint(0.250001, 1E-05),
            new C2DPoint(1.00001, 1E-05),
            new C2DPoint(1.00001, 0.50001),
            new C2DPoint(0.50001, 0.50001),
            new C2DPoint(0.5001, 0.75001)
        });

        private static readonly Polygon _userPolygon = new Polygon("oryginalPolygon", new List<C2DPoint>
        {
            new C2DPoint(0.9060227713, 0.7615049749),
            new C2DPoint(0.9077990317, 0.6170206608),
            new C2DPoint(0.9503555075, 0.6028332772),
            new C2DPoint(0.9982237396, 0.5124085694),
            new C2DPoint(1.0, 0.4042553071),
            new C2DPoint(0.9716252329, 0.2109890933),
            new C2DPoint(0.7482250164, 0.1968073847),
            new C2DPoint(0.4485806658, 0.2003542306),
            new C2DPoint(0.4202127086, 0.1294286627),
            new C2DPoint(0.4343955522, 0.0549619239),
            new C2DPoint(0.4680809407, 0.0177285545),
            new C2DPoint(0.390070761, 0.0),
            new C2DPoint(0.2659561247, 0.0230459859),
            new C2DPoint(0.2180850551, 0.0248222463),
            new C2DPoint(0.1524825935, 0.0709198929),
            new C2DPoint(0.1187943675, 0.1577977549),
            new C2DPoint(0.06914874, 0.2145359392),
            new C2DPoint(0.0354599465, 0.2340407541),
            new C2DPoint(0.0, 0.3120543388),
            new C2DPoint(0.0035462784, 0.5531887847),
            new C2DPoint(0.0301419476, 0.645384078),
            new C2DPoint(0.406027595, 0.6950285706),
            new C2DPoint(0.4627657793, 0.6666651533),
            new C2DPoint(0.5177311082, 0.5549593702),
            new C2DPoint(0.5567350631, 0.526595953),
            new C2DPoint(0.6046089702, 0.5319133843),
            new C2DPoint(0.7074448011, 0.6152444004),
            new C2DPoint(0.6985805239, 0.744678738)
        });
        private static readonly Polygon _polygon101Et1 = new Polygon("polygon101Et1", new List<C2DPoint>
        {
            new C2DPoint(0.6105029523, 0.4965778411),
            new C2DPoint(0.6105029523, 0.3566753218),
            new C2DPoint(0.2975934225, 0.3566753218),
            new C2DPoint(0.2975934225, 0.4967174783),
            new C2DPoint(0.0, 0.4967174783),
            new C2DPoint(0.0, 0.0),
            new C2DPoint(1.0, 0.0),
            new C2DPoint(1.0, 0.4979393039),
            new C2DPoint(0.6105029523, 0.4965778411)
        });
        private static readonly Polygon _polygon101Et2 = new Polygon("polygon101Et2", new List<C2DPoint>
        {
            new C2DPoint(1.0, 0.0),
            new C2DPoint(1.0, 0.4076410106),
            new C2DPoint(0.3009, 0.4076410106),
            new C2DPoint(0.300954817, 0.5541427993),
            new C2DPoint(0.047771, 0.5541427),
            new C2DPoint(0.0477711657, 0.4060508864),
            new C2DPoint(0.0, 0.40605),
            new C2DPoint(0.0, 0.0),
            new C2DPoint(1.0, 0.0)
        });
        private static readonly Polygon _polygon101Et3 = new Polygon("polygon101Et3", new List<C2DPoint>
        {
                new C2DPoint(1.0, 0.0669970844),
                new C2DPoint(1.0, 0.3921625343),
                new C2DPoint(0.3284303375, 0.3921625343),
                new C2DPoint(0.3284303375, 0.4411756199),
                new C2DPoint(0.1209150941, 0.4411756199),
                new C2DPoint(0.1209150941, 0.2908533975),
                new C2DPoint(0.0408490588, 0.2908533975),
                new C2DPoint(0.0408490588, 0.1029433884),
                new C2DPoint(0.0, 0.1029433884),
                new C2DPoint(0.0, 0.0),
                new C2DPoint(0.1617641529, 0.0),
                new C2DPoint(0.1617641529, 0.0669970844),
                new C2DPoint(1.0, 0.0669970844)
        });
        
        [TestMethod]
        public void TestSerialization()
        {
            var serialized = _referencePolygon.SerializeToXDoc();
            Console.WriteLine(serialized);
            serialized.Save(testFileName);
        }
        
        [TestMethod]
        public void TestDeserialization()
        {
            if (!File.Exists(testFileName))
            {
                _referencePolygon.SerializeToXDoc().Save(testFileName);
            }

            var result = Helper.DeserializeFromXml<Polygon>(testFileName);
            Assert.AreEqual(_referencePolygon.Points.Count, result.Points.Count);
        }

        [TestMethod]
        public void SerializeAllPolygons()
        {
            List<Polygon> polygons = new List<Polygon> { _referencePolygon, _oryginalPolygon, _userPolygon, _polygon101Et1, _polygon101Et2, _polygon101Et3 };
            foreach (var poly in polygons)
            {
                poly.SerializeToXDoc().Save(Path.ChangeExtension(poly.Name, "xml"));
            }
        }
    }
}
