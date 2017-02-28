using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test class to test bounds for elements without vs. with transformations
    /// (see issue 281). Only some basic elements are tested - this should be sufficient
    /// to verify the functionality for all path-based elements.
    /// </summary>
    [TestClass]
    public class BoundsTests : SvgTestHelper
    {
        private const string BoundsTestSvg = "Issue281_Bounds.BoundsTest.svg";
        private static SvgDocument testDocument;

        [TestMethod]
        public void TestLineBounds()
        {
            // x1="10" x2="30" y1="20" y2="40", default line thickness is 1
            AssertEqualBounds("line", 9.5f, 19.5f, 21, 21);
            // additional translation(5, 5)
            AssertEqualBounds("line-xlate", 14.5f, 24.5f, 21, 21);
            // additional rotation(180) and translation(-50, 0)
            AssertEqualBounds("line-xform", 19.5f, -40.5f, 21, 21);
        }

        [TestMethod]
        public void TestRectangleBounds()
        {
            // x="10" y="30" width="10" height="20"
            AssertEqualBounds("rect", 9.5f, 29.5f, 10.5f, 20.5f);
            // additional translation(10, 10)
            AssertEqualBounds("rect-xlate", 19.5f, 39.5f, 10.5f, 20.5f);
            // additional rotation(90)
            AssertEqualBounds("rect-rot", -50, 9.5f, 20.5f, 10.5f);
        }

        [TestMethod]
        public void TestGroupBounds()
        {
            // all lines from TestLineBounds()
            AssertEqualBounds("lines", 9.5f, -40.5f, 31, 86);
            // all rectangles from TestRectangleBounds()
            AssertEqualBounds("rects", -50f, 9.5f, 80, 50.5f);
        }

        private void AssertEqualBounds(string elementId, float x, float y, float width, float height)
        {
            const float Epsilon = 0.01f;
            var element = GetElement(elementId);
            var elementBounds = element.Bounds;
            Assert.AreEqual(x, elementBounds.X, Epsilon);
            Assert.AreEqual(y, elementBounds.Y, Epsilon);
            Assert.AreEqual(width, elementBounds.Width, Epsilon);
            Assert.AreEqual(height, elementBounds.Height, Epsilon);
        }

        private SvgVisualElement GetElement(string elementId)
        {
            if (testDocument == null)
            {
                testDocument = OpenSvg(GetXMLDocFromResource(GetFullResourceString(BoundsTestSvg)));
            }
            return testDocument.GetElementById<SvgVisualElement>(elementId);
        }
    }
}
