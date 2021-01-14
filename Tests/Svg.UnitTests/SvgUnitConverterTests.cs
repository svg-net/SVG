using System;
using System.Globalization;
using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgUnitConverterTests
    {
        private readonly SvgUnitConverter _converter = new SvgUnitConverter();

        [Test]
        public void ParseReturnsValidUnit()
        {
            var unitNone = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "none");
            Assert.AreEqual(new SvgUnit(SvgUnitType.None, 0f), unitNone);
            
            var unitPoint = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1pt");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Point, 1f), unitPoint);

            var unitPixel1 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1.25px");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Pixel, 1.25f), unitPixel1);

            var unitPixel2 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "15px");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Pixel, 15f), unitPixel2);

            var unitPica = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1pc");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Pica, 1f), unitPica);

            var unitMillimeter1 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1mm");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Millimeter, 1f), unitMillimeter1);

            var unitPixel3 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "3.543307px");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Pixel, 3.543307f), unitPixel3);

            var unitCentimeter= _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1cm");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Centimeter, 1f), unitCentimeter);

            var unitPixel4 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "35.43307px");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Pixel, 35.43307f), unitPixel4);

            var unitInch1 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1in");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Inch, 1f), unitInch1);

            var unitPixel5 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "90px");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Pixel, 90f), unitPixel5);

            var unitEm1 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "15em");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Em, 15f), unitEm1);

            var unitMillimeter2 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "0.2822222mm");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Millimeter, 0.2822222f), unitMillimeter2);

            var unitUser1 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "3990");
            Assert.AreEqual(new SvgUnit(SvgUnitType.User, 3990f), unitUser1);

            var unitUser2 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1990");
            Assert.AreEqual(new SvgUnit(SvgUnitType.User, 1990f), unitUser2);

            var unitUser3 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "-50");
            Assert.AreEqual(new SvgUnit(SvgUnitType.User, -50f), unitUser3);

            var unitInch2 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, ".4in");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Inch, .4f), unitInch2);

            var unitEm2 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, ".25em");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Em, .25f), unitEm2);

            var unitPercentage1 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "10%");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Percentage, 10f), unitPercentage1);

            var unitPercentage2 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1%");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Percentage, 1f), unitPercentage2);

            var unitPercentage3 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "0%");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Percentage, 0f), unitPercentage3);

            var unitPercentage4 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "100%");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Percentage, 100f), unitPercentage4);

            var unitEm3 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1.2em");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Em, 1.2f), unitEm3);

            var unitEm4 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "medium");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Em, 1f), unitEm4);

            var unitEm5 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "x-small");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Em, 0.7f), unitEm5);

            var unitEm6 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "xx-large");
            Assert.AreEqual(new SvgUnit(SvgUnitType.Em, 1.7f), unitEm6);

            var unitUser4 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "657.45");
            Assert.AreEqual(new SvgUnit(SvgUnitType.User, 657.45f), unitUser4);

            var unitUser5 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "12.5");
            Assert.AreEqual(new SvgUnit(SvgUnitType.User, 12.5f), unitUser5);

            var unitUser6 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "0");
            Assert.AreEqual(new SvgUnit(SvgUnitType.User, 0f), unitUser6);

            var unitUser7 = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "12");
            Assert.AreEqual(new SvgUnit(SvgUnitType.User, 12f), unitUser7);
        }
    }
}
