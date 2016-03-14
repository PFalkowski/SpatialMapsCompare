using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawingCanvas;
using System.Text.RegularExpressions;
using System.IO;

namespace SpatialMapsCompareTests
{
    [TestClass]
    public class NameValidatorTest
    {
        [TestMethod]
        public void NameValidatorCreateTest()
        {
            var tested = new NameValidator();
            var expectedRegex = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]");
            var testString = new string(Path.GetInvalidFileNameChars());
            var expectedMatches = expectedRegex.Matches(testString);
            var receivedMatches = tested.InvalidCharsRegex.Matches(testString);
            var expected = expectedMatches[0];
            var received = receivedMatches[0];
            Assert.AreEqual(expected.Value, received.Value);
        }

        [TestMethod]
        public void NameValidatorValidateTestInvariantCulture()
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var tested = new NameValidator();
            var result = tested.Validate(null, culture);
            Assert.IsFalse(result.IsValid);
            result = tested.Validate("", culture);
            Assert.IsFalse(result.IsValid);
            result = tested.Validate(" ", culture);
            Assert.IsFalse(result.IsValid);
            result = tested.Validate("      ", culture);
            Assert.IsFalse(result.IsValid);
            result = tested.Validate("\t\t\t", culture);
            Assert.IsFalse(result.IsValid);
            result = tested.Validate("sgfyiwegfiweg hfwuiegfi  ywegfygiw   gefoyweoryu123675887989091823", culture);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void NameValidatorValidateTestInvariantCulture2()
        {
            var invalid = new string(Path.GetInvalidFileNameChars());
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var tested = new NameValidator();
            var result = tested.Validate("qwertyuiop|", culture);
            Assert.IsFalse(result.IsValid);
            result = tested.Validate("abc?2 ", culture);
            Assert.IsFalse(result.IsValid);
            result = tested.Validate("%21313!weq  ", culture);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void NameValidatorValueNameTest()
        {
            var culture = System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("EN");
            var tested = new NameValidator();
            var result = tested.Validate("qwertyuiop|", culture);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorContent.ToString().Contains(tested.valueName));
            tested.valueName = "custom value name";
            result = tested.Validate("qwertyuiop|", culture);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorContent.ToString().Contains("custom value name"));
        }
    }
}
