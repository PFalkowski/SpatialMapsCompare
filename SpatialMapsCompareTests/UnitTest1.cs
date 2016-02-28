using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using GeoLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpatialMapsCompare;

namespace SpatialMapsCompareTests
{
    [TestClass]
    public class UnitTest1
    {
        private string testFileName = "testSerializationData.xml";
        private readonly List<C2DPoint> _referencePolygonC2DPoints = new List<C2DPoint>
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
        };

        [TestMethod]
        public void TestSerialization()
        {
            var serialized = _referencePolygonC2DPoints.SerializeToXDoc();
            Console.WriteLine(serialized);
            serialized.Save(testFileName);
        }
        
        [TestMethod]
        public void TestDeserialization()
        {
            if (!File.Exists(testFileName))
            {
                _referencePolygonC2DPoints.SerializeToXDoc().Save(testFileName);
            }

            var result = Helper.DeserializeFromXml<List<C2DPoint>>(testFileName);
            Assert.AreEqual(_referencePolygonC2DPoints.Count, result.Count);
        }
    }
}
