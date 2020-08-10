using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgDocumentTests
    {
        [Test]
        public void GetPointsPerInchDpiTests()
        {
            var pointsPerInch = SvgDocument.PointsPerInch;
            SvgDocument.PointsPerInch = 1;
            var testPointsPerInch = SvgDocument.PointsPerInch;
            Assert.AreEqual(1, testPointsPerInch, "Test sets the Dpi to 1");

            // resets the System Dpi
            SvgDocument.PointsPerInch = pointsPerInch;
            var comparePointsPerInch = SvgDocument.PointsPerInch;
            Assert.AreEqual(pointsPerInch, comparePointsPerInch, "After setting to null the default Implementation should be taken");
        }
    }
}
