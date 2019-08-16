using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Xml;
using Svg.Document_Structure;
using Svg.FilterEffects;

namespace Svg.UnitTests
{
    /// <seealso cref="SvgTestHelper" />
    [TestFixture]
    public class SvgElementDeepCopyTest : SvgTestHelper
    {
        private const string PureTextElementSvg = "Issue_TextElement.Text.svg";

        [Test]
        public void TestSvgElementDeepCopy()
        {
            CheckDeepCopyInstance(new SvgCircle());
            CheckDeepCopyInstance(new SvgEllipse());
            CheckDeepCopyInstance(new SvgImage());
            CheckDeepCopyInstance(new SvgLine());
            CheckDeepCopyInstance(new SvgPolygon());
            CheckDeepCopyInstance(new SvgPolyline());
            CheckDeepCopyInstance(new SvgRectangle());
            CheckDeepCopyInstance(new SvgClipPath());
            CheckDeepCopyInstance(new SvgMask());
            CheckDeepCopyInstance(new SvgDefinitionList());
            CheckDeepCopyInstance(new SvgDescription());
            CheckDeepCopyInstance(new SvgDocumentMetadata());
            CheckDeepCopyInstance(new SvgFragment());
            CheckDeepCopyInstance(new SvgGroup());
            CheckDeepCopyInstance(new SvgSwitch());
            CheckDeepCopyInstance(new SvgSymbol());
            CheckDeepCopyInstance(new SvgTitle());
            CheckDeepCopyInstance(new SvgUse());
            CheckDeepCopyInstance(new SvgForeignObject());
            CheckDeepCopyInstance(new SvgFilter());
            CheckDeepCopyInstance(new SvgColourMatrix());
            CheckDeepCopyInstance(new SvgGaussianBlur());
            CheckDeepCopyInstance(new SvgMerge());
            CheckDeepCopyInstance(new SvgMergeNode());
            CheckDeepCopyInstance(new SvgOffset());
            CheckDeepCopyInstance(new SvgGradientStop());
            CheckDeepCopyInstance(new SvgLinearGradientServer());
            CheckDeepCopyInstance(new SvgMarker());
            CheckDeepCopyInstance(new SvgPatternServer());
            CheckDeepCopyInstance(new SvgRadialGradientServer());
            CheckDeepCopyInstance(new SvgPath());
            CheckDeepCopyInstance(new SvgFont());
            CheckDeepCopyInstance(new SvgFontFace());
            CheckDeepCopyInstance(new SvgFontFaceSrc());
            CheckDeepCopyInstance(new SvgFontFaceUri());
            CheckDeepCopyInstance(new SvgGlyph());
            CheckDeepCopyInstance(new SvgVerticalKern());
            CheckDeepCopyInstance(new SvgHorizontalKern());
            CheckDeepCopyInstance(new SvgMissingGlyph());
            CheckDeepCopyInstance(new SvgText());
            CheckDeepCopyInstance(new SvgTextPath());
            CheckDeepCopyInstance(new SvgTextRef());
            CheckDeepCopyInstance(new SvgTextSpan());
        }

        private void CheckDeepCopyInstance<T>(T src)
            where T : SvgElement
        {
            var dest = src.DeepCopy();
            Assert.IsInstanceOf<T>(dest);
        }

        /// <summary>
        /// Tests that the deep copy of a <see cref="SvgText"/> is done correctly where the
        /// text element has contains only text and now other elements like <see cref="SvgTextSpan"/>.
        /// </summary>
        [Test]
        public void TestSvgTextElementDeepCopy()
        {
            var svgDocument = OpenSvg(GetResourceXmlDoc(GetFullResourceString(PureTextElementSvg)));
            CheckDocument(svgDocument);

            var deepCopy = (SvgDocument)svgDocument.DeepCopy<SvgDocument>();
            CheckDocument(deepCopy);
        }

        /// <summary>
        /// Checks the document if it contains the correct information when exported to XML.
        /// </summary>
        /// <param name="svgDocument">The SVG document to check.</param>
        private static void CheckDocument(SvgDocument svgDocument)
        {
            //TODO: check if the assignablefrom indeed is the correct replacement
            Assert.AreEqual(2, svgDocument.Children.Count);
            Assert.IsAssignableFrom(typeof(SvgDefinitionList), svgDocument.Children[0]);
            Assert.IsAssignableFrom(typeof(SvgText), svgDocument.Children[1]);

            var textElement = (SvgText)svgDocument.Children[1];
            Assert.AreEqual("IP", textElement.Content);

            var memoryStream = new MemoryStream();
            svgDocument.Write(memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);

            var xmlDocument = new XmlDocument();
            xmlDocument.XmlResolver = new SvgDtdResolver();
            xmlDocument.Load(memoryStream);

            // the first node is the added DTD declaration
            Assert.AreEqual(3, xmlDocument.ChildNodes.Count);
            var svgNode = xmlDocument.ChildNodes[2];

            // Filter all significant whitespaces.
            var svgChildren = svgNode.ChildNodes
                .OfType<XmlNode>()
                .Where(item => item.GetType() != typeof(XmlSignificantWhitespace))
                .OfType<XmlNode>()
                .ToArray();

            Assert.AreEqual(2, svgChildren.Length);
            var textNode = svgChildren[1];

            Assert.AreEqual("text", textNode.Name);
            Assert.AreEqual("IP", textNode.InnerText);
        }
    }
}
