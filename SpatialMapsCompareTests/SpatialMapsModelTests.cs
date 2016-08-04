using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpatialMaps;

namespace SpatialMapsCompareTests
{
    [TestClass]
    public class SpatialMapsModelTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var moq = new Mock<IOService>(MockBehavior.Strict);
            var model = new SpatialMapsModel(moq.Object);
            Assert.IsTrue(model.FileType.Contains("xml"));
            Assert.IsTrue(model.FilterString.Contains(model.FileType));
            Assert.IsTrue(model.RoundDigits < 5);
        }
        [TestMethod]
        public void IsPolygonValidTest()
        {
            var model = new SpatialMapsModel(null);
            Assert.IsFalse(model.IsPolygonValid(null));
        }
    }
}
