using System.Text;
using Svg.Css;
using NUnit.Framework;

namespace Svg.UnitTests
{
    /// <summary>
    ///This is a test class for CssQueryTest and is intended
    ///to contain all CssQueryTest Unit Tests
    ///</summary>
    [TestFixture]
    public class CssQueryTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private void TestSelectorSpecificity(string selector, int specificity)
        {
            var parser = new ExCSS.Parser();
            var sheet = parser.Parse(selector + " {color:black}");
            Assert.AreEqual(specificity, CssQuery.GetSpecificity(sheet.StyleRules[0].Selector));
        }

        /// <summary>
        ///A test for GetSpecificity
        ///</summary>
        ///<remarks>Lifted from http://www.smashingmagazine.com/2007/07/27/css-specificity-things-you-should-know/, and http://css-tricks.com/specifics-on-css-specificity/ </remarks>
        [Test]
        [TestCase("*", 0x0)]
        [TestCase("li", 0x10)]
        [TestCase("li:first-line", 0x20)]
        [TestCase("ul li", 0x20)]
        [TestCase("ul ol+li", 0x30)]
        [TestCase("h1 + *[rel=up]", 0x110)]
        [TestCase("ul ol li.red", 0x130)]
        [TestCase("li.red.level", 0x210)]
        [TestCase("p", 0x010)]
        [TestCase("div p", 0x020)]
        [TestCase(".sith", 0x100)]
        [TestCase("div p.sith", 0x120)]
        [TestCase("#sith", 0x1000)]
        [TestCase("body #darkside .sith p", 0x1120)]
        [TestCase("body #content .data img:hover", 0x1220)]
        [TestCase("a#a-02", 0x1010)]
        [TestCase("a[id=\"a-02\"]", 0x0110)]
        [TestCase("ul#nav li.active a", 0x1130)]
        [TestCase("body.ie7 .col_3 h2 ~ h2", 0x0230)]
        [TestCase("#footer *:not(nav) li", 0x1020)]
        [TestCase("ul > li ul li ol li:first-letter", 0x0070)]
        public void RunSpecificityTests(string selector, int specifity)
        {
            TestSelectorSpecificity(selector, specifity);
        }

        [Test]
        [TestCase("font-size:13;", "font-size:13;")]
        [TestCase("font-size:13;font-style:normal;", "font-size:13;font-style:normal;")]
        [TestCase("font-size:13;font-style:normal;font-weight:bold;", "font-size:13;font-style:normal;font-weight:bold;")]
        [TestCase("font-family:Nimbus Sans L,'Arial Narrow',sans-serif;Sans L',sans-serif;", "font-family:Nimbus Sans L,'Arial Narrow',sans-serif;")]
        public void TestStyleDeclarations(string style, string expected)
        {
            var actual = new StringBuilder();

            var cssParser = new ExCSS.Parser();
            var inlineSheet = cssParser.Parse("#a{" + style + "}");
            foreach (var rule in inlineSheet.StyleRules)
                foreach (var decl in rule.Declarations)
                    actual.Append(decl.Name).Append(":").Append(decl.Term.ToString()).Append(";");

            Assert.AreEqual(expected, actual.ToString());
        }
    }
}
