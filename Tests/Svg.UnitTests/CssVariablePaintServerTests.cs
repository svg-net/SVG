using NUnit.Framework;
using System.Drawing;

namespace Svg.UnitTests
{
    [TestFixture]
    public class CssVariablePaintServerTests
    {
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
    }
}
