using NUnit.Framework;
using System.Drawing;
using System.Globalization;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgColourServerTests
    {
        [Test]
        public void ToString_SemiTransparentColor_EndsWithAlphaChannel()
        {
            var server = new SvgColourServer(Color.FromArgb(0x80, 0xFF, 0x00, 0x00));

            var colorText = server.ToString();

            Assert.AreEqual("#ff000080", colorText,
                "Semi-transparent red should be serialized as #RRGGBBAA and keep alpha at the end.");
        }

        [Test]
        public void ConvertFrom_HexWithTrailingAlpha_PreservesAlpha()
        {
            var converter = new SvgColourConverter();

            var color = (Color)converter.ConvertFrom(null, CultureInfo.InvariantCulture, "#ff000080");

            Assert.AreEqual(0xFF, color.R);
            Assert.AreEqual(0x00, color.G);
            Assert.AreEqual(0x00, color.B);
            Assert.AreEqual(0x80, color.A);
        }

        [Test]
        public void ConvertFrom_HexWithoutAlpha()
        {
            var converter = new SvgColourConverter();

            var color = (Color)converter.ConvertFrom(null, CultureInfo.InvariantCulture, "#aabbcc");

            Assert.AreEqual(0xAA, color.R);
            Assert.AreEqual(0xBB, color.G);
            Assert.AreEqual(0xCC, color.B);
            Assert.AreEqual(0xFF, color.A);
        }
    }
}
