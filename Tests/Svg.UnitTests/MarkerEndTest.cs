using NUnit.Framework;
using Svg.DataTypes;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test Class of rendering SVGs with marker-end elements.
    /// Based on Issue 212.
    /// </summary>
    /// <remarks>
    /// Test use the following embedded resources:
    ///   - Issue212_MakerEnd\OperatingPlan.svg
    /// </remarks>
    [TestFixture]
    public class MarkerEndTest : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("Issue212_MakerEnd.OperatingPlan.svg"); } }
        protected override int ExpectedSize { get { return 4300; } } // original image has 4314 bytes

        [Test]
        public void TestOperatingPlanRendering()
        {
            LoadSvg(GetXMLDocFromResource());
        }

        [Test]
        public void TestArrowCodeCreation()
        {
            // Sample code from Issue 212. Thanks to podostro.
            const int width = 50;
            const int height = 50;

            var document = new SvgDocument()
            {
                ID = "svgMap",
                ViewBox = new SvgViewBox(0, 0, width, height)
            };

            var defsElement = new SvgDefinitionList() { ID = "defsMap" };
            document.Children.Add(defsElement);

            var groupElement = new SvgGroup() { ID = "gMap" };
            document.Children.Add(groupElement);

            var arrowPath = new SvgPath()
            {
                ID = "pathMarkerArrow",
                Fill = new SvgColourServer(Color.Black),
                PathData = SvgPathBuilder.Parse(@"M0,0 L4,2 L0,4 L1,2 z")
            };

            var arrowMarker = new SvgMarker()
            {
                ID = "markerArrow",
                MarkerUnits = SvgMarkerUnits.StrokeWidth,
                MarkerWidth = 5,
                MarkerHeight = 5,
                RefX = 3,
                RefY = 2,
                Orient = new SvgOrient() { IsAuto = true },
                Children = { arrowPath }
            };

            defsElement.Children.Add(arrowMarker);

            var line = new SvgLine()
            {
                ID = "lineLinkedPoint",
                StartX = 0,
                StartY = 15,
                EndX = 35,
                EndY = 35,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 3,
                MarkerEnd = new Uri(string.Format("url(#{0})", arrowMarker.ID), UriKind.Relative)
            };

            groupElement.Children.Add(line);

            var svgXml = document.GetXML();
            var img = document.Draw();

            var file = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            File.WriteAllText(file + ".svg", svgXml);
            img.Save(file + ".png");
            Debug.WriteLine(string.Format("Svg saved to '{0}'", file));

            // Remove
            var svg = new FileInfo(file + ".svg");
            if (svg.Exists) svg.Delete();
            var png = new FileInfo(file + ".png");
            if (png.Exists) png.Delete();
        }
    }
}
