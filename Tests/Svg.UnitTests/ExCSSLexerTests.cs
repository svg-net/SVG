using NUnit.Framework;
using System.Linq;

namespace Svg.UnitTests
{
    /// <summary>
    ///This is a test class for CssQueryTest and is intended
    ///to contain all CssQueryTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ExCSSLexerTests
    {
        [Test]
        public void LexerStylesheet()
        {
            //
            TestStylesheetReader("#a{fill:#000000;font-family:ZWBIQX+HelveticaUnicodeMS;}", 14);
            TestStylesheetParser("#a{fill:#000000;font-family:ZWBIQX+HelveticaUnicodeMS;}", 2, "#a");
            TestStylesheetReader("ul ol+li{fill:#000000;font-family:ZWBIQX+HelveticaUnicodeMS;}", 18);
            TestStylesheetParser("ul ol+li{fill:#000000;font-family:ZWBIQX+HelveticaUnicodeMS;}", 2, "ul ol+li");
        }
        private void TestStylesheetReader(string css, int length)
        {
            var lexer = new ExCSS.Lexer(new ExCSS.StylesheetReader(css));
            var tokens = lexer.Tokens.ToArray();
            Assert.AreEqual(length, tokens.Length);
        }
        private void TestStylesheetParser(string css, int declarationsCount, string selector)
        {
            var styleSheet = new ExCSS.Parser().Parse(css);
            var styleRules = styleSheet.StyleRules.ToArray();
            Assert.AreEqual(1, styleRules.Length);
            Assert.AreEqual(declarationsCount, styleRules[0].Declarations.Count);
            Assert.AreEqual(selector, styleRules[0].Selector.ToString());
            Assert.AreEqual(Svg.ExCSS.RuleType.Style, styleRules[0].RuleType);
        }
    }
}
