using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg.UnitTests
{
    [TestFixture]
    public class TextAnchorChunkTests
    {
        // Each glyph has exactly one em of ink and advance. These are real
        // SVG font paths, independent of the installed fonts and their metrics.
        private static SvgDocument Document(string anchor, string content, string coordinates = "x='100' y='100'", bool narrowGlyphs = false)
        {
            var outline = narrowGlyphs ? "M0 0H500V1000H0Z" : "M0 0H1000V1000H0Z";
            return SvgDocument.FromSvg<SvgDocument>(
                "<svg xmlns='http://www.w3.org/2000/svg' width='300' height='200'>" +
                "<defs><font horiz-adv-x='1000'><font-face font-family='AnchorTest' units-per-em='1000' ascent='800' descent='200'/>" +
                "<missing-glyph d='" + outline + "'/><glyph unicode='a' d='" + outline + "'/>" +
                "<glyph unicode='b' d='" + outline + "'/><glyph unicode='c' d='" + outline + "'/>" +
                "<glyph unicode='d' d='" + outline + "'/><glyph unicode=' ' d=''/></font></defs>" +
                "<text id='text' " + coordinates + " font-family='AnchorTest' font-size='20' text-anchor='" + anchor + "'>" +
                content + "</text></svg>");
        }

        private static void Layout(SvgDocument document)
        {
            document.GetElementById<SvgText>("text").Path(null);
        }

        private static RectangleF Ink(SvgDocument document, string id)
        {
            var path = document.GetElementById<SvgTextBase>(id).Path(null);
            Assert.That(path.PointCount, Is.GreaterThan(0), "Expected real glyph outlines for " + id);
            return path.GetBounds();
        }

        private static List<float> GlyphLefts(SvgDocument document, string id)
        {
            var path = document.GetElementById<SvgTextBase>(id).Path(null);
            var lefts = new List<float>();
            using (var iterator = new GraphicsPathIterator(path))
            {
                int start, end;
                bool closed;
                while (iterator.NextSubpath(out start, out end, out closed) > 0)
                    lefts.Add(path.PathPoints.Skip(start).Take(end - start + 1).Min(p => p.X));
            }
            return lefts;
        }

        [TestCase("start", 0)]
        [TestCase("middle", -30)]
        [TestCase("end", -60)]
        public void SiblingSpansUseOneAnchoredChunk(string anchor, float offset)
        {
            var doc = Document(anchor, "<tspan id='a'>a</tspan><tspan id='b'>b</tspan><tspan id='c'>c</tspan>");
            Layout(doc);
            Assert.That(Ink(doc, "a").X, Is.EqualTo(100 + offset).Within(0.01));
            Assert.That(Ink(doc, "b").X, Is.EqualTo(120 + offset).Within(0.01));
            Assert.That(Ink(doc, "c").X, Is.EqualTo(140 + offset).Within(0.01));
        }

        [TestCase("start", 0)]
        [TestCase("middle", -40)]
        [TestCase("end", -80)]
        public void NestedAndFollowingSiblingSpansShareTheChunk(string anchor, float offset)
        {
            var doc = Document(anchor, "<tspan id='a'>a</tspan><tspan id='outer'><tspan id='b'>b</tspan>" +
                "<tspan id='c'>c</tspan></tspan><tspan id='d'>d</tspan>");
            Layout(doc);
            for (var i = 0; i < 4; i++)
                Assert.That(Ink(doc, "abcd"[i].ToString()).X, Is.EqualTo(100 + i * 20 + offset).Within(0.01));
        }

        [TestCase("start", 0)]
        [TestCase("middle", -30)]
        [TestCase("end", -60)]
        public void ParentTextBeforeAndAfterASpanIsNotLostOrShiftedTwice(string anchor, float offset)
        {
            var doc = Document(anchor, "a<tspan id='b'>b</tspan>c");
            Layout(doc);
            Assert.That(Ink(doc, "text").X, Is.EqualTo(100 + offset).Within(0.01));
            Assert.That(Ink(doc, "text").Width, Is.EqualTo(60).Within(0.01));
            Assert.That(Ink(doc, "b").X, Is.EqualTo(120 + offset).Within(0.01));
        }

        [TestCase("start", 0, 0)]
        [TestCase("middle", -10, -20)]
        [TestCase("end", -20, -40)]
        public void AbsoluteXStartsAChunkIncludingItsFollowingSpan(string anchor, float firstOffset, float secondOffset)
        {
            var doc = Document(anchor, "<tspan id='a'>a</tspan><tspan id='b' x='200'>b</tspan><tspan id='c'>c</tspan>");
            Layout(doc);
            Assert.That(Ink(doc, "a").X, Is.EqualTo(100 + firstOffset).Within(0.01));
            Assert.That(Ink(doc, "b").X, Is.EqualTo(200 + secondOffset).Within(0.01));
            Assert.That(Ink(doc, "c").X, Is.EqualTo(220 + secondOffset).Within(0.01));
        }

        [TestCase("start", 0, 0)]
        [TestCase("middle", -10, -20)]
        [TestCase("end", -20, -40)]
        public void AbsoluteYStartsAnIndependentChunk(string anchor, float firstOffset, float secondOffset)
        {
            var doc = Document(anchor, "<tspan id='a'>a</tspan><tspan id='b' y='150'>b</tspan><tspan id='c'>c</tspan>");
            Layout(doc);
            Assert.That(Ink(doc, "a").X, Is.EqualTo(100 + firstOffset).Within(0.01));
            Assert.That(Ink(doc, "b").X, Is.EqualTo(120 + secondOffset).Within(0.01));
            Assert.That(Ink(doc, "c").X, Is.EqualTo(140 + secondOffset).Within(0.01));
            Assert.That(Ink(doc, "b").Bottom, Is.EqualTo(150).Within(0.01));
        }

        [TestCase("start", 0)]
        [TestCase("middle", -32.5f)]
        [TestCase("end", -65)]
        public void RelativeOffsetsDoNotStartAChunk(string anchor, float offset)
        {
            var doc = Document(anchor, "<tspan id='a'>a</tspan><tspan id='b' dx='5' dy='5'>b</tspan><tspan id='c'>c</tspan>");
            Layout(doc);
            Assert.That(Ink(doc, "a").X, Is.EqualTo(100 + offset).Within(0.01));
            Assert.That(Ink(doc, "b").X, Is.EqualTo(125 + offset).Within(0.01));
            Assert.That(Ink(doc, "c").X, Is.EqualTo(145 + offset).Within(0.01));
        }

        [TestCase("start", 0, 0)]
        [TestCase("middle", -10, -20)]
        [TestCase("end", -20, -40)]
        public void MultipleAbsoluteCoordinatesRetainSeparatePathFragments(string anchor, float firstOffset, float secondOffset)
        {
            var doc = Document(anchor, "abc", "x='100 200' y='100'");
            Layout(doc);
            var lefts = GlyphLefts(doc, "text");
            Assert.That(lefts, Has.Count.EqualTo(3));
            Assert.That(lefts[0], Is.EqualTo(100 + firstOffset).Within(0.01));
            Assert.That(lefts[1], Is.EqualTo(200 + secondOffset).Within(0.01));
            Assert.That(lefts[2], Is.EqualTo(220 + secondOffset).Within(0.01));
        }

        [Test]
        public void SpanFontSizeFillAndIndividualTransformStillRenderSeparately()
        {
            var doc = Document("middle", "<tspan id='a' fill='red'>a</tspan>" +
                "<tspan id='b' font-size='40' fill='blue' transform='translate(7 3)'>b</tspan>" +
                "<tspan id='c' fill='green'>c</tspan>");
            Layout(doc);
            Assert.That(Ink(doc, "a").X, Is.EqualTo(60).Within(0.01));
            Assert.That(Ink(doc, "b").X, Is.EqualTo(80).Within(0.01));
            Assert.That(Ink(doc, "b").Width, Is.EqualTo(40).Within(0.01));
            Assert.That(Ink(doc, "c").X, Is.EqualTo(120).Within(0.01));
            using (var image = doc.Draw())
            {
                Assert.That(image.GetPixel(70, 90).ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
                Assert.That(image.GetPixel(100, 80).ToArgb(), Is.EqualTo(Color.Blue.ToArgb()));
                Assert.That(image.GetPixel(82, 90).A, Is.Zero, "The blue span must retain its individual translate.");
                Assert.That(image.GetPixel(130, 90).ToArgb(), Is.EqualTo(Color.Green.ToArgb()));
            }
        }

        [Test]
        public void AnEmptySpanDoesNotEndTheAnchoredChunk()
        {
            var doc = Document("middle", "<tspan text-anchor='end'> </tspan><tspan id='a'>a</tspan><tspan/><tspan id='b'>b</tspan>");
            Layout(doc);
            Assert.That(Ink(doc, "a").X, Is.EqualTo(80).Within(0.01));
            Assert.That(Ink(doc, "b").X, Is.EqualTo(100).Within(0.01));
        }

        [TestCase("start", 0)]
        [TestCase("middle", -20)]
        [TestCase("end", -40)]
        public void InkWidthDoesNotChangeAdvanceWhenTextIsSplit(string anchor, float offset)
        {
            var whole = Document(anchor, "ab", narrowGlyphs: true);
            var split = Document(anchor, "<tspan id='a'>a</tspan><tspan id='b'>b</tspan>", narrowGlyphs: true);
            Layout(whole);
            Layout(split);
            var lefts = GlyphLefts(whole, "text");
            Assert.That(lefts, Has.Count.EqualTo(2));
            Assert.That(Ink(whole, "text").Width, Is.EqualTo(30).Within(0.01));
            for (var i = 0; i < 2; i++)
            {
                Assert.That(lefts[i], Is.EqualTo(100 + i * 20 + offset).Within(0.01));
                Assert.That(Ink(split, "ab"[i].ToString()).X, Is.EqualTo(lefts[i]).Within(0.01));
                Assert.That(Ink(split, "ab"[i].ToString()).Width, Is.EqualTo(10).Within(0.01));
            }
        }

        [TestCase("start", 0, 0)]
        [TestCase("middle", -10, -20)]
        [TestCase("end", -20, -40)]
        public void AdvanceRemainsLogicalAfterAnAbsoluteReset(string anchor, float firstOffset, float secondOffset)
        {
            var whole = Document(anchor, "<tspan id='a'>a</tspan><tspan id='bc' x='200'>bc</tspan>", narrowGlyphs: true);
            var split = Document(anchor, "<tspan id='a'>a</tspan><tspan id='b' x='200'>b</tspan><tspan id='c'>c</tspan>", narrowGlyphs: true);
            Layout(whole);
            Layout(split);
            Assert.That(Ink(whole, "a").X, Is.EqualTo(100 + firstOffset).Within(0.01));
            Assert.That(Ink(split, "a").X, Is.EqualTo(Ink(whole, "a").X).Within(0.01));
            var lefts = GlyphLefts(whole, "bc");
            Assert.That(lefts, Has.Count.EqualTo(2));
            for (var i = 0; i < 2; i++)
            {
                Assert.That(lefts[i], Is.EqualTo(200 + i * 20 + secondOffset).Within(0.01));
                Assert.That(Ink(split, "bc"[i].ToString()).X, Is.EqualTo(lefts[i]).Within(0.01));
            }
        }

        [TestCase("start", 0)]
        [TestCase("middle", -30)]
        [TestCase("end", -60)]
        public void PreservedSpacesContributeAdvanceWithoutInk(string anchor, float offset)
        {
            var whole = Document(anchor, "a b", "x='100' y='100' xml:space='preserve'", true);
            var split = Document(anchor, "<tspan id='a'>a</tspan><tspan> </tspan><tspan id='b'>b</tspan>",
                "x='100' y='100' xml:space='preserve'", true);
            Layout(whole);
            Layout(split);
            var lefts = GlyphLefts(whole, "text");
            Assert.That(lefts, Has.Count.EqualTo(2));
            Assert.That(lefts[0], Is.EqualTo(100 + offset).Within(0.01));
            Assert.That(lefts[1], Is.EqualTo(140 + offset).Within(0.01));
            Assert.That(Ink(split, "a").X, Is.EqualTo(lefts[0]).Within(0.01));
            Assert.That(Ink(split, "b").X, Is.EqualTo(lefts[1]).Within(0.01));
        }
    }
}
