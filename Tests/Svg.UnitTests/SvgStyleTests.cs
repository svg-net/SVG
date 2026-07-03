using NUnit.Framework;
using System.IO;
using System.Linq;

namespace Svg.UnitTests
{
    /// <summary>
    /// Testing the capabilities to handle SVG documents with style elements and the option to add
    /// styles to the SVG document.
    /// </summary>
    [TestFixture]
    public class SvgStyleTests : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("StyleTag.TestFile.svg"); } }

        private SvgStyle GetStyleElementFromDocument(SvgDocument doc)
        {
            var el = doc.Children.FirstOrDefault(c => c.ElementName == "style");
            Assert.IsNotNull(el, "Could not find the style element");
            Assert.IsAssignableFrom(typeof(SvgStyle), el, "Style tag was not correctly typed, expected SvgStyle as type");
            return el as SvgStyle;
        }

        [Test]
        public void SvgDocumentWithSvgStyle_ReadFile_StyleTagReadFromSvgInCorrectType()
        {
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);

            var circleElement = doc.Children.FirstOrDefault(c => c.ElementName == "circle");
            Assert.IsNotNull(circleElement, "Expected to find the circle element in the document");
            Assert.IsAssignableFrom(typeof(SvgCircle), circleElement, "Expected the found circle element to be of type SvgCircle");

            var styleElement = GetStyleElementFromDocument(doc);
            Assert.IsNotNull(styleElement, "Expected to find the style element in the document");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_AttributeParsing_CorrectTypeAttribute()
        {
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);

            var tag = GetStyleElementFromDocument(doc);
            Assert.AreEqual("text/css", tag.StyleType, "Expected the type attribute to be read");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_AttributeParsing_CorrectMediaAttribute()
        {
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);

            var tag = GetStyleElementFromDocument(doc);
            Assert.AreEqual("all", tag.Media, "Expected the media attribute to be read");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_RetrieveStyleContent_ContentIsNotEmpty()
        {
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);

            var tag = GetStyleElementFromDocument(doc);
            Assert.IsFalse(string.IsNullOrWhiteSpace(tag.Content), "Expected the style content to not be empty");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_AddStyle_CDataAndTypeAdded()
        {
            var doc = new SvgDocument();
            var style = new SvgStyle();
            style.Content = "circle { fill: red; }";
            style.StyleType = "text/css";
            doc.Children.Add(style);
            var docStr = doc.GetXML();

            Assert.IsFalse(string.IsNullOrWhiteSpace(docStr), "Expected document to be returned");
            Assert.IsTrue(docStr.IndexOf("<style type=\"text/css\">") > -1, "Expected to find the style with type tag");
            Assert.IsTrue(docStr.IndexOf("<![CDATA[") > -1, "Expected to find the CDATA start");
            Assert.IsTrue(docStr.IndexOf("]]>") > -1, "Expected to find the CDATA ending");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_AddStyleWithMedia_MediaAttributeInOutput()
        {
            var doc = new SvgDocument();
            var style = new SvgStyle();
            style.Content = ".cls { fill: blue; }";
            style.StyleType = "text/css";
            style.Media = "screen";
            doc.Children.Add(style);
            var docStr = doc.GetXML();

            Assert.IsTrue(docStr.IndexOf("media=\"screen\"") > -1, "Expected to find the media attribute");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_EmptyContent_NoCDataWritten()
        {
            var doc = new SvgDocument();
            var style = new SvgStyle();
            style.StyleType = "text/css";
            doc.Children.Add(style);
            var docStr = doc.GetXML();

            Assert.IsTrue(docStr.IndexOf("<style") > -1, "Expected to find the style tag");
            Assert.IsTrue(docStr.IndexOf("<![CDATA[") < 0, "Expected no CDATA when content is empty");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_DeepCopy_ReturnsNewInstance()
        {
            var style = new SvgStyle();
            style.Content = "circle { fill: red; }";
            style.StyleType = "text/css";
            style.Media = "all";

            var copy = style.DeepCopy();

            Assert.IsNotNull(copy, "Expected deep copy to return a non-null element");
            Assert.IsInstanceOf<SvgStyle>(copy, "Expected deep copy to return an SvgStyle instance");
            Assert.AreNotSame(style, copy, "Expected deep copy to return a different instance");
        }

        [Test]
        public void SvgDocumentWithSvgStyle_RoundTrip_PreservesContent()
        {
            var doc = new SvgDocument();
            var style = new SvgStyle();
            style.Content = ".test { fill: green; }";
            style.StyleType = "text/css";
            doc.Children.Add(style);

            using (var stream = new MemoryStream())
            {
                doc.Write(stream);
                stream.Position = 0;

                var reloaded = SvgDocument.Open<SvgDocument>(stream);
                var reloadedStyle = GetStyleElementFromDocument(reloaded);
                Assert.IsTrue(reloadedStyle.Content.Contains(".test { fill: green; }"),
                    "Expected the CSS content to survive a write/read round-trip");
            }
        }

        [Test]
        public void SvgDocument_OpenFromString_StyleElementIsParsed()
        {
            var svg = "<svg xmlns=\"http://www.w3.org/2000/svg\"><style type=\"text/css\">.a{fill:red}</style><rect class=\"a\" width=\"10\" height=\"10\"/></svg>";
            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            Assert.IsNotNull(doc);

            var styleElement = doc.Children.FirstOrDefault(c => c.ElementName == "style");
            Assert.IsNotNull(styleElement, "Expected to find the style element");
            Assert.IsInstanceOf<SvgStyle>(styleElement, "Expected style element to be of type SvgStyle");
        }
    }
}
