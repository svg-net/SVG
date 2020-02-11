using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        private static readonly List<Type> ElementTypeList = new List<Type>()
        {
            typeof(SvgCircle),
            typeof(SvgEllipse),
            typeof(SvgLine),
            typeof(SvgPolygon),
            typeof(SvgPolyline),
            typeof(SvgRectangle),
            typeof(SvgClipPath),
            typeof(SvgMask),
            typeof(SvgDefinitionList),
            typeof(SvgDescription),
            typeof(SvgDocumentMetadata),
            typeof(SvgFragment),
            typeof(SvgGroup),
            typeof(SvgImage),
            typeof(SvgSwitch),
            typeof(SvgSymbol),
            typeof(SvgTitle),
            typeof(SvgUse),
            typeof(SvgForeignObject),
            typeof(SvgFilter),
            typeof(SvgBlend),
            typeof(SvgColourMatrix),
            typeof(SvgComponentTransfer),
            typeof(SvgComposite),
            typeof(SvgConvolveMatrix),
            typeof(SvgDiffuseLighting),
            typeof(SvgDisplacementMap),
            typeof(SvgDistantLight),
            typeof(SvgFlood),
            typeof(SvgFuncA),
            typeof(SvgFuncB),
            typeof(SvgFuncG),
            typeof(SvgFuncR),
            typeof(SvgGaussianBlur),
            typeof(SvgImage),
            typeof(SvgMerge),
            typeof(SvgMergeNode),
            typeof(SvgMorphology),
            typeof(SvgOffset),
            typeof(SvgPointLight),
            typeof(SvgSpecularLighting),
            typeof(SvgSpotLight),
            typeof(SvgTile),
            typeof(SvgTurbulence),
            typeof(SvgAnchor),
            typeof(SvgColourServer),
            typeof(SvgDeferredPaintServer),
            typeof(SvgGradientStop),
            typeof(SvgLinearGradientServer),
            typeof(SvgMarker),
            typeof(SvgPatternServer),
            typeof(SvgRadialGradientServer),
            typeof(SvgPath),
            typeof(SvgScript),
            typeof(SvgFont),
            typeof(SvgFontFace),
            typeof(SvgFontFaceSrc),
            typeof(SvgFontFaceUri),
            typeof(SvgGlyph),
            typeof(SvgVerticalKern),
            typeof(SvgHorizontalKern),
            typeof(SvgMissingGlyph),
            typeof(SvgText),
            typeof(SvgTextPath),
            typeof(SvgTextRef),
            typeof(SvgTextSpan),
            typeof(NonSvgElement),
            typeof(SvgUnknownElement),
        };

        [Test]
        public void TestSvgElementDeepCopy()
        {
            foreach (var type in ElementTypeList)
                CheckDeepCopyInstance(Activator.CreateInstance(type) as SvgElement);
        }

        private void CheckDeepCopyInstance<T>(T src)
            where T : SvgElement
        {
            var dest = src.DeepCopy();
            Assert.IsInstanceOf<T>(dest);
        }

        [Test]
        public void TestDoNotDeepCopy()
        {
            Assert.AreSame(SvgPaintServer.None, SvgPaintServer.None.DeepCopy());
            Assert.AreSame(SvgPaintServer.Inherit, SvgPaintServer.Inherit.DeepCopy());
            Assert.AreSame(SvgPaintServer.NotSet, SvgPaintServer.NotSet.DeepCopy());
        }

        [Test]
        public void TestDeepCopyAttribute()
        {
            foreach (var type in ElementTypeList)
            {
                var src = Activator.CreateInstance(type) as SvgElement;
                var dest = src.DeepCopy();

                Assert.Zero(src.Attributes.Count);
                CheckDeepCopyAttribute(src, dest);
            }

            {
                var src = OpenSvg(GetResourceXmlDoc(GetFullResourceString(PureTextElementSvg)));
                var dest = (SvgDocument)src.DeepCopy<SvgDocument>();

                CheckDeepCopyAttribute(src, dest);
                CheckDeepCopyAttribute(src.Children[0], dest.Children[0]);
                CheckDeepCopyAttribute(src.Children[1], dest.Children[1]);
            }
        }

        private void CheckDeepCopyAttribute(SvgElement src, SvgElement dest)
        {
            Assert.AreEqual(src.Attributes.Count, dest.Attributes.Count);

            foreach (var attribute in src.Attributes)
            {
                Assert.IsTrue(dest.Attributes.ContainsKey(attribute.Key));
                Assert.AreEqual(attribute.Value, dest.Attributes.GetAttribute<object>(attribute.Key));
            }
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
            Assert.AreEqual(2, svgDocument.Children.Count);
            Assert.IsInstanceOf<SvgDefinitionList>(svgDocument.Children[0]);
            Assert.IsInstanceOf<SvgText>(svgDocument.Children[1]);

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
