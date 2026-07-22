using NUnit.Framework;
using System.Drawing;
using System.Xml;

namespace Svg.UnitTests
{
    [TestFixture]
    public class CssVariablePaintServerTests
    {
        [Test]
        public void Parse_StyleBlockWithoutFallback_CreatesCssVariablePaintServer()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg'>
                <style>
                    :root{
                        --my-color: red;
                    }
                </style>
                <rect id='r' style='fill:var(--my-color)' width='10' height='10'/>
            </svg>";

            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            var rect = (SvgRectangle)doc.GetElementById("r");

            Assert.IsInstanceOf<SvgCssVariablePaintServer>(rect.Fill,
                "fill should be an SvgCssVariablePaintServer");

            var cssVar = (SvgCssVariablePaintServer)rect.Fill;

            var resolved = cssVar.Resolve(rect);
            Assert.IsInstanceOf<SvgColourServer>(resolved,
                "resolved server should be a colour server");
            Assert.AreEqual(Color.Red, ((SvgColourServer)resolved).Colour);
            Assert.AreEqual("--my-color", cssVar.VariableName);
            Assert.IsNull(cssVar.FallbackServer, "no fallback should be set");
        }

        [Test]
        public void Parse_InlineStyleTakesPrecedenceOverStyleBlock()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg' style='--my-color: blue;'>
                <style>
                    :root{
                        --my-color: red;
                    }
                </style>
                <rect id='r' style='fill:var(--my-color)' width='10' height='10'/>
            </svg>";

            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            var rect = (SvgRectangle)doc.GetElementById("r");

            Assert.IsInstanceOf<SvgCssVariablePaintServer>(rect.Fill,
                "fill should be an SvgCssVariablePaintServer");

            var cssVar = (SvgCssVariablePaintServer)rect.Fill;

            var resolved = cssVar.Resolve(rect);
            Assert.IsInstanceOf<SvgColourServer>(resolved,
                "resolved server should be a colour server");
            Assert.AreEqual(Color.Blue, ((SvgColourServer)resolved).Colour);
            Assert.AreEqual("--my-color", cssVar.VariableName);
            Assert.IsNull(cssVar.FallbackServer, "no fallback should be set");
        }

        [Test]
        public void Parse_VarWithNoFallback_CreatesCssVariablePaintServer()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg'>
                <rect id='r' style='fill:var(--my-color)' width='10' height='10'/>
            </svg>";

            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            var rect = (SvgRectangle)doc.GetElementById("r");

            Assert.IsInstanceOf<SvgCssVariablePaintServer>(rect.Fill,
                "fill should be an SvgCssVariablePaintServer");

            var cssVar = (SvgCssVariablePaintServer)rect.Fill;
            Assert.AreEqual("--my-color", cssVar.VariableName);
            Assert.IsNull(cssVar.FallbackServer, "no fallback should be set");
        }

        /// <summary>
        /// A fill of <c>var(--my-color, red)</c> must produce a server whose
        /// <see cref="SvgCssVariablePaintServer.FallbackServer"/> is the colour red.
        /// </summary>
        [Test]
        public void Parse_VarWithColourFallback_CreatesCssVariablePaintServerWithFallback()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg'>
                <rect id='r' style='fill:var(--my-color, red)' width='10' height='10'/>
            </svg>";

            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            var rect = (SvgRectangle)doc.GetElementById("r");

            Assert.IsInstanceOf<SvgCssVariablePaintServer>(rect.Fill);

            var cssVar = (SvgCssVariablePaintServer)rect.Fill;
            Assert.AreEqual("--my-color", cssVar.VariableName);

            Assert.IsInstanceOf<SvgColourServer>(cssVar.FallbackServer,
                "fallback should be parsed as a colour server");
            Assert.AreEqual(Color.Red,
                ((SvgColourServer)cssVar.FallbackServer).Colour);
        }

        /// <summary>
        /// A CSS custom property defined directly on a parent element via its
        /// <c>style</c> attribute must be resolved when
        /// <see cref="SvgCssVariablePaintServer.Resolve"/> is called on a child element.
        /// </summary>
        [Test]
        public void Resolve_VariableDefinedOnParentInlineStyle_ResolvesToColour()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg'>
                <g id='g' style='--brand-color:blue'>
                    <rect id='r' style='fill:var(--brand-color)' width='10' height='10'/>
                </g>
            </svg>";

            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            var rect = (SvgRectangle)doc.GetElementById("r");

            Assert.IsInstanceOf<SvgCssVariablePaintServer>(rect.Fill);

            var cssVar = (SvgCssVariablePaintServer)rect.Fill;
            var resolved = cssVar.Resolve(rect);

            Assert.IsInstanceOf<SvgColourServer>(resolved,
                "resolved server should be a colour server");
            Assert.AreEqual(Color.Blue, ((SvgColourServer)resolved).Colour);
        }

        /// <summary>
        /// When the CSS variable is not defined anywhere in the ancestor chain the
        /// fallback paint server must be returned.
        /// </summary>
        [Test]
        public void Resolve_UndefinedVariable_ReturnsFallback()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg'>
                <rect id='r' style='fill:var(--missing, green)' width='10' height='10'/>
            </svg>";

            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            var rect = (SvgRectangle)doc.GetElementById("r");

            var cssVar = (SvgCssVariablePaintServer)rect.Fill;
            var resolved = cssVar.Resolve(rect);

            Assert.IsInstanceOf<SvgColourServer>(resolved);
            Assert.AreEqual(Color.Green, ((SvgColourServer)resolved).Colour);
        }

        /// <summary>
        /// When the variable is undefined and no fallback is given,
        /// <see cref="SvgPaintServer.None"/> must be returned.
        /// </summary>
        [Test]
        public void Resolve_UndefinedVariableNoFallback_ReturnsNone()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg'>
                <rect id='r' style='fill:var(--missing)' width='10' height='10'/>
            </svg>";

            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            var rect = (SvgRectangle)doc.GetElementById("r");

            var cssVar = (SvgCssVariablePaintServer)rect.Fill;
            var resolved = cssVar.Resolve(rect);

            Assert.AreSame(SvgPaintServer.None, resolved);
        }

        /// <summary>
        /// <see cref="SvgCssVariablePaintServer.ToString"/> must round-trip to the
        /// canonical <c>var()</c> CSS function syntax.
        /// </summary>
        [Test]
        public void ToString_NoFallback_ProducesVarSyntax()
        {
            var server = new SvgCssVariablePaintServer("--my-color");
            Assert.AreEqual("var(--my-color)", server.ToString());
        }

        [Test]
        public void ToString_WithFallback_ProducesVarSyntaxWithFallback()
        {
            var fallback = new SvgColourServer(Color.Red);
            var server = new SvgCssVariablePaintServer("--my-color", fallback);
            Assert.AreEqual("var(--my-color, Red)", server.ToString());
        }

        /// <summary>
        /// A CSS custom property defined via <c>:root</c> in a <c><style></style></c> block must
        /// be resolvable at runtime but must not be duplicated as an inline <c>style</c>
        /// attribute on the <c><svg></c> element when serialized. After a round-trip
        /// (parse → serialize → re-parse) the variable must still resolve correctly.
        /// </summary>
        [Test]
        public void RoundTrip_StyleBlockCustomProperty_NotDuplicatedAsInlineStyle()
        {
            const string svg = @"<svg xmlns='http://www.w3.org/2000/svg'>
                <style id='s'>
                    :root{
                        --my-color: red;
                    }
                </style>
                <rect id='r' style='fill:var(--my-color)' width='10' height='10'/>
            </svg>";

            // 1. Parse and verify the custom property resolves.
            var doc = SvgDocument.FromSvg<SvgDocument>(svg);
            Assert.IsTrue(doc.TryGetCustomProperty("--my-color", out var value),
                "custom property should be resolvable on the document");
            Assert.AreEqual("red", value);

            // 2. Serialize and inspect the raw XML.
            var xml = doc.GetXML();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("svg", "http://www.w3.org/2000/svg");

            var svgNode = xmlDoc.DocumentElement;
            var inlineStyle = svgNode.GetAttribute("style");
            Assert.IsFalse(inlineStyle.Contains("--my-color"),
                "stylesheet-originated custom property must not appear as inline style");

            var styleEl = xmlDoc.SelectSingleNode("//svg:style[@id='s']", nsMgr);
            Assert.IsNotNull(styleEl, "style element should be preserved");
            StringAssert.Contains("--my-color", styleEl.InnerText,
                "custom property should remain inside the <style> element");

            // 3. Re-parse and verify the variable still resolves.
            var doc2 = SvgDocument.FromSvg<SvgDocument>(xml);
            var rect2 = (SvgRectangle)doc2.GetElementById("r");

            Assert.IsInstanceOf<SvgCssVariablePaintServer>(rect2.Fill,
                "fill should survive the round-trip as an SvgCssVariablePaintServer");

            var cssVar2 = (SvgCssVariablePaintServer)rect2.Fill;
            var resolved2 = cssVar2.Resolve(rect2);
            Assert.IsInstanceOf<SvgColourServer>(resolved2,
                "resolved server should be a colour server after round-trip");
            Assert.AreEqual(Color.Red, ((SvgColourServer)resolved2).Colour,
                "custom property should still resolve to red after round-trip");
        }
    }
}
