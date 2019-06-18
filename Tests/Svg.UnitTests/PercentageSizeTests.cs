using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class PercentageSizeTests
    {
        [Test]
        public void TestRectangle()
        {
            var svgRectangle = new SvgRectangle()
            {
                X = new SvgUnit(SvgUnitType.Percentage, 50),
                Y = new SvgUnit(SvgUnitType.Percentage, 50),
                Width = new SvgUnit(SvgUnitType.Percentage, 15),
                Height = new SvgUnit(SvgUnitType.Percentage, 20)
            };
            CheckPercentageSize(svgRectangle);
        }

        [Test]
        public void TestCircle()
        {
            var svgCircle = new SvgCircle()
            {
                CenterX = new SvgUnit(SvgUnitType.Percentage, 50),
                CenterY = new SvgUnit(SvgUnitType.Percentage, 50),
                Radius = new SvgUnit(SvgUnitType.Percentage, 15),
            };
            CheckPercentageSize(svgCircle);
        }

        [Test]
        public void TestEllipse()
        {
            var svgEllipse = new SvgEllipse()
            {
                CenterX = new SvgUnit(SvgUnitType.Percentage, 50),
                CenterY = new SvgUnit(SvgUnitType.Percentage, 50),
                RadiusX = new SvgUnit(SvgUnitType.Percentage, 15),
                RadiusY = new SvgUnit(SvgUnitType.Percentage, 5),
            };
            CheckPercentageSize(svgEllipse);
        }

        private void CheckPercentageSize(SvgVisualElement element)
        {
            var svgDoc = new SvgDocument()
            {
                Width = 200,
                Height = 200
            };
            svgDoc.Children.Add(element);
            var boudsPref = element.Bounds;
            svgDoc.Width = 400;
            svgDoc.Height = 400;
            var boundsAfter = element.Bounds;
            Assert.AreNotEqual(boudsPref.Size, boundsAfter.Size, "To device value convert error");
        }
    }
}
