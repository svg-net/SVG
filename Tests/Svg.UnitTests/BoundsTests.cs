using NUnit.Framework;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test class to test bounds for elements without vs. with transformations
    /// (see issue 281). Only some basic elements are tested - this should be sufficient
    /// to verify the functionality for all path-based elements.
    /// </summary>
    [TestFixture]
    public class BoundsTests : SvgTestHelper
    {
        private const string BoundsTestSvg = "Issue281_Bounds.BoundsTest.svg";
        private static SvgDocument testDocument;

        [Test]
        public void TestLineBounds()
        {
            // x1="10" x2="30" y1="20" y2="40", default line thickness is 1
            AssertEqualBounds("line", 9.5f, 19.5f, 21, 21);
            // additional translation(5, 5)
            AssertEqualBounds("line-xlate", 14.5f, 24.5f, 21, 21);
            // additional rotation(180) and translation(-50, 0)
            AssertEqualBounds("line-xform", 19.5f, -40.5f, 21, 21);
        }

        [Test]
        public void TestRectangleBounds()
        {
            // x="10" y="30" width="10" height="20"
            AssertEqualBounds("rect", 9.5f, 29.5f, 11f, 21f);
            // additional translation(10, 10)
            AssertEqualBounds("rect-xlate", 19.5f, 39.5f, 11f, 21f);
            // additional rotation(90)
            AssertEqualBounds("rect-rot", -50.5f, 9.5f, 21f, 11f);
        }

        [Test]
        public void TestTextBounds()
        {
            // x="10" y="30" font-family="Tahoma" font-size="15pt"  content="VVVV-svg"
            AssertEqualBounds("text", 9.92f, 15.46f, 83.69f, 18.67f);
            // additional translation(10, 10)
            AssertEqualBounds("text-xlate", 19.92f, 24.8f, 133.95f, 19.33f);
            // additional rotation(30)
            AssertEqualBounds("text-rot", -2.08f, 18.34f, 102.46f, 71.01f);
        }

        [Test]
        public void TestGroupBounds()
        {
            // all lines from TestLineBounds()
            AssertEqualBounds("lines", 9.5f, -40.5f, 31f, 86f);
            // all rectangles from TestRectangleBounds()
            AssertEqualBounds("rects", -50.5f, 9.5f, 81f, 51f);
        }

        [Test]
        public void TestTranslatedGroupBounds()
        {
            AssertEqualBounds("lines-translated", -12.5f, -7, 31, 86);
        }

        [Test]
        public void TestScaledGroupBounds()
        {
            AssertEqualBounds("lines-scaled", 19, -81, 62, 172);
        }

        [Test]
        public void TestRotatedGroupBounds()
        {
            AssertEqualBounds("lines-rotated", -45.5f, 9.5f, 86, 31);
        }

        [Test]
        public void TestBoundsIndempotent()
        {
            // ensure that accessing the Bounds property returns the same value when called repeatedly.
            AssertEqualBounds("lines-rotated", -45.5f, 9.5f, 86, 31);
            AssertEqualBounds("lines-rotated", -45.5f, 9.5f, 86, 31);

            AssertEqualBounds("text-rot", -2.08f, 18.34f, 102.46f, 71.01f);
            AssertEqualBounds("text-rot", -2.08f, 18.34f, 102.46f, 71.01f);

            AssertEqualBounds("rect-rot", -50.5f, 9.5f, 21f, 11f);
            AssertEqualBounds("rect-rot", -50.5f, 9.5f, 21f, 11f);

            AssertEqualBounds("line-xform", 19.5f, -40.5f, 21, 21);
            AssertEqualBounds("line-xform", 19.5f, -40.5f, 21, 21);
        }
        void AssertEqualBoundsCore(string elementId, float x, float y, float width, float height, System.Func<string, System.Drawing.RectangleF> boundsGetter)
        {
            const float Epsilon = 0.01f;
            var elementBounds = boundsGetter(elementId);
            Assert.AreEqual(x, elementBounds.X, Epsilon);
            Assert.AreEqual(y, elementBounds.Y, Epsilon);
            Assert.AreEqual(width, elementBounds.Width, Epsilon);
            Assert.AreEqual(height, elementBounds.Height, Epsilon);
        }
        private void AssertEqualBounds(string elementId, float x, float y, float width, float height) => AssertEqualBoundsCore(elementId, x, y, width, height, id => GetElement(id).Bounds);

        private SvgVisualElement GetElement(string elementId)
        {
            if (testDocument == null)
            {
                testDocument = OpenSvg(GetXMLDocFromResource(GetFullResourceString(BoundsTestSvg)));
            }
            return testDocument.GetElementById<SvgVisualElement>(elementId);
        }
        [Test]
        public void BoundsUseParentTransforms()
        {
            var doc = SvgDocument.FromSvg<SvgDocument>(@"
<svg>
<g transform=""translate(10 20) rotate(20)"" id=""a"">
<g transform=""translate(-20 -10)"" id=""b"">
<text id=""c""><tspan id=""d"">ABC</tspan></text>
</g>
<rect x=""10"" width=""50"" height=""60"" id=""e""/>
</g>
<text id=""f"">ABC</text>
</svg>
");
            void AssertEqualBounds(string elementId, float x, float y, float width, float height) => AssertEqualBoundsCore(elementId, x, y, width, height, id => doc.GetElementById<Svg.SvgVisualElement>(id).BoundsRelativeToTop);
            var a = doc.GetElementById<SvgGroup>("a");
            var b = doc.GetElementById<SvgGroup>("b");
            var c = doc.GetElementById<SvgText>("c");
            var d = doc.GetElementById<SvgTextSpan>("d");
            var e = doc.GetElementById<SvgRectangle>("e");
            var f = doc.GetElementById<SvgText>("f");
            AssertEqualBounds("a", -5.38f, -3.18f, 72.41f, 100.73f);
            AssertEqualBounds("b", -5.38f, -3.18f, 24.87f, 14.49f);
            AssertEqualBounds("c", -5.38f, -3.18f, 24.87f, 14.49f);
            AssertEqualBounds("d", -5.38f, -3.18f, 24.87f, 14.49f);
            AssertEqualBounds("e", -1.77f, 22.78f, 68.79f, 74.76f);
            AssertEqualBounds("f", -0.01f, -8.74f, 24.07f, 8.88f);
            var aBounds = a.BoundsRelativeToTop;
            var bBounds = b.BoundsRelativeToTop;
            var cBounds = c.BoundsRelativeToTop;
            var dBounds = d.BoundsRelativeToTop;
            var eBounds = e.BoundsRelativeToTop;
            var fBounds = f.BoundsRelativeToTop;
            Assert.AreEqual(bBounds, cBounds);
            Assert.AreEqual(cBounds, dBounds);
            Assert.AreNotEqual(cBounds, fBounds);
            Assert.AreEqual(c.Bounds, f.Bounds); // Important difference between Bounds and BoundsRelativeToTop
            Assert.AreEqual(aBounds, System.Drawing.RectangleF.Union(dBounds, eBounds));
            Assert.AreNotEqual(a.Bounds, System.Drawing.RectangleF.Union(d.Bounds, e.Bounds));

            Assert.True(a.Children.Remove(e));
            Assert.AreNotEqual(aBounds, a.BoundsRelativeToTop);
            Assert.AreEqual(bBounds, b.BoundsRelativeToTop);
            Assert.AreEqual(cBounds, c.BoundsRelativeToTop);
            Assert.AreEqual(dBounds, d.BoundsRelativeToTop);
            Assert.AreNotEqual(eBounds, e.BoundsRelativeToTop);
            a.Children.Add(e);
            Assert.AreEqual(aBounds, a.BoundsRelativeToTop);
            Assert.AreEqual(bBounds, b.BoundsRelativeToTop);
            Assert.AreEqual(cBounds, c.BoundsRelativeToTop);
            Assert.AreEqual(dBounds, d.BoundsRelativeToTop);
            Assert.AreEqual(eBounds, e.BoundsRelativeToTop);

            var bBoundsRelativeToA = b.Bounds;
            var t = a.Transforms[^1];
            a.Transforms.Remove(t);
            Assert.AreNotEqual(aBounds, a.BoundsRelativeToTop);
            Assert.AreNotEqual(bBounds, b.BoundsRelativeToTop);
            Assert.AreNotEqual(cBounds, c.BoundsRelativeToTop);
            Assert.AreNotEqual(dBounds, d.BoundsRelativeToTop);
            Assert.AreNotEqual(eBounds, e.BoundsRelativeToTop);
            Assert.AreEqual(bBoundsRelativeToA, b.Bounds);
            a.Transforms.Add(t);
            Assert.AreEqual(aBounds, a.BoundsRelativeToTop);
            Assert.AreEqual(bBounds, b.BoundsRelativeToTop);
            Assert.AreEqual(cBounds, c.BoundsRelativeToTop);
            Assert.AreEqual(dBounds, d.BoundsRelativeToTop);
            Assert.AreEqual(eBounds, e.BoundsRelativeToTop);

            d.Text = "ABCDE";
            Assert.AreEqual(aBounds, a.BoundsRelativeToTop);
            Assert.AreNotEqual(bBounds, b.BoundsRelativeToTop);
            Assert.AreNotEqual(cBounds, c.BoundsRelativeToTop);
            Assert.AreNotEqual(dBounds, d.BoundsRelativeToTop);
            Assert.AreEqual(eBounds, e.BoundsRelativeToTop);
        }
    }
}
