using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgDocumentTests
    {
        [Test]
        public void GetSystemDpiTests()
        {
            var pointsPerInch = SvgDocument.PointsPerInch;
            SvgDocument.GetSystemDpi = TestSystemDpi;
            var testPointsPerInch = SvgDocument.PointsPerInch;
            Assert.AreEqual(1, testPointsPerInch, "Test sets the Dpi to 1");

            // resets the Get System Dpi function
            SvgDocument.GetSystemDpi = null;
            var comparePointsPerInch = SvgDocument.PointsPerInch;
            Assert.AreEqual(pointsPerInch, comparePointsPerInch, "After setting to null the default Implementation should be taken");
        }

        private int TestSystemDpi()
        {
            return 1;
        }
    }
}
