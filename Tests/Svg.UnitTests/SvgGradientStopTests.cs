using NUnit.Framework;
using System.Drawing;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgGradientStopTests
    {
        [Test]
        public void Constructor_WithOffsetAndColour_AppliesStopColor()
        {
            var expectedOffset = new SvgUnit(SvgUnitType.Percentage, 50f);
            var expectedColor = Color.Green;

            var gradientStop = new SvgGradientStop(expectedOffset, expectedColor);

            Assert.That(gradientStop.Offset, Is.EqualTo(expectedOffset));
            Assert.That(gradientStop.StopColor, Is.EqualTo(new SvgColourServer(expectedColor)));
            Assert.That(gradientStop.GetColor(null), Is.EqualTo(expectedColor));
        }
    }
}
