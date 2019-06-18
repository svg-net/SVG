using Svg.Css;
using NUnit.Framework;

namespace Svg.UnitTests
{
    /// <summary>
    ///This is a test class for CssQueryTest and is intended
    ///to contain all CssQueryTest Unit Tests
    ///</summary>
    [NUnit.Framework.TestFixture]
    public class CssQueryTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
        public void RunSpecificityTests()
        {
            TestSelectorSpecificity("*", 0x0);
            TestSelectorSpecificity("li", 0x10);
            TestSelectorSpecificity("li:first-line", 0x20);
            TestSelectorSpecificity("ul li", 0x20);
            TestSelectorSpecificity("ul ol+li", 0x30);
            TestSelectorSpecificity("h1 + *[rel=up]", 0x110);
            TestSelectorSpecificity("ul ol li.red", 0x130);
            TestSelectorSpecificity("li.red.level", 0x210);
            TestSelectorSpecificity("p", 0x010);
            TestSelectorSpecificity("div p", 0x020);
            TestSelectorSpecificity(".sith", 0x100);
            TestSelectorSpecificity("div p.sith", 0x120);
            TestSelectorSpecificity("#sith", 0x1000);
            TestSelectorSpecificity("body #darkside .sith p", 0x1120);
            TestSelectorSpecificity("body #content .data img:hover", 0x1220);
            TestSelectorSpecificity("a#a-02", 0x1010);
            TestSelectorSpecificity("a[id=\"a-02\"]", 0x0110);
            TestSelectorSpecificity("ul#nav li.active a", 0x1130);
            TestSelectorSpecificity("body.ie7 .col_3 h2 ~ h2", 0x0230);
            TestSelectorSpecificity("#footer *:not(nav) li", 0x1020);
            TestSelectorSpecificity("ul > li ul li ol li:first-letter", 0x0070);
        }
    }
}
