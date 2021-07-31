using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Moq;
using NUnit.Framework;

namespace Svg.UnitTests
{
    public class SvgVisualElementTests
    {
        [Test]
        public void TestSmoothingModeRestoreForGraphics()
        {
            var visualElementMock = new Mock<SvgVisualElement>();
            visualElementMock.Setup(_ => _.Attributes).CallBase();

            var visualElement = visualElementMock.Object;
            visualElement.ShapeRendering = SvgShapeRendering.Auto;

            var g = Graphics.FromHwnd(IntPtr.Zero);
            var renderer = SvgRenderer.FromGraphics(g);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            visualElement.RenderElement(renderer);

            Assert.That(g.SmoothingMode, Is.EqualTo(SmoothingMode.AntiAlias));
        }

        [Test]
        public void TestSmoothingModeRestoreForCustomRenderer()
        {
            var visualElementMock = new Mock<SvgVisualElement>();
            visualElementMock.Setup(_ => _.Attributes).CallBase();

            var visualElement = visualElementMock.Object;
            visualElement.ShapeRendering = SvgShapeRendering.Auto;

            var renderer = Mock.Of<ISvgRenderer>(_ => _.Transform == new Matrix());

            renderer.SmoothingMode = SmoothingMode.HighQuality;

            visualElement.RenderElement(renderer);

            Assert.That(renderer.SmoothingMode, Is.EqualTo(SmoothingMode.HighQuality));
        }
    }
}
