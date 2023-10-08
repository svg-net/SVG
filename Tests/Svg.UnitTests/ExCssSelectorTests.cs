using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using ExCSS;
using Fizzler;
using Svg.Css;

namespace Svg.UnitTests
{
    [TestFixture]
    public class ExCssSelectorTests
    {
        [Test]
        [TestCase("struct-use-11-f")]
        [TestCase("struct-use-10-f")]
        [TestCase("styling-css-03-b")]
        [TestCase("__issue-280-01")]
        [TestCase("styling-css-01-b")]
        [TestCase("__issue-034-02")]
        public void RunAllSelectorTests(string baseName)
        {
            RunAllSelectorTests(baseName, null);
        }

        public void RunAllSelectorTests(string baseName, string folder)
        {
            var elementFactory = new SvgElementFactory();
            var testSuite = Path.Combine(ImageTestDataSource.SuiteTestsFolder, "W3CTestSuite");
            string basePath = folder ?? Path.Combine(testSuite, "svg");
            var svgPath = Path.Combine(basePath, baseName + ".svg");

            var styles = new List<ISvgNode>();
            using (var xmlFragment = File.Open(svgPath, FileMode.Open))
            {
                using (var xmlTextReader = new XmlTextReader(xmlFragment))
                {
                    var svgDocument = SvgDocument.Create<SvgDocument>(xmlTextReader, elementFactory, styles);

                    var rootNode = new NonSvgElement();
                    rootNode.Children.Add(svgDocument);

                    if (styles.Any())
                    {
                        var cssTotal = string.Join(Environment.NewLine, styles.Select(s => s.Content).ToArray());
                        var stylesheetParser = new StylesheetParser(true, true);
                        var stylesheet = stylesheetParser.Parse(cssTotal);

                        foreach (var selector in stylesheet.StyleRules)
                        {
                            TestSelector(selector.Selector.Text, rootNode, elementFactory);
                        }
                    }
                }
            }
        }

        [Test]
        [TestCaseSource(typeof(TestSvg), nameof(TestSvg.AllSvgs))]
        [TestCaseSource(typeof(TestSvg), nameof(TestSvg.AllImageSvgs))]
        public void RunSelectorsOnSvgTests(TestSvg svg)
        {
            RunAllSelectorTests(svg.BaseName, svg.Folder);
        }

        [Test]
        [TestCase("#testId.test1", "struct-use-11-f")]
        [TestCase("*.test2", "struct-use-11-f")]
        [TestCase("circle.test3", "struct-use-11-f")]
        [TestCase(".descendant circle.test4", "struct-use-11-f")]
        [TestCase(".child", "struct-use-11-f")]
        [TestCase("circle.test5", "struct-use-11-f")]
        [TestCase(".child > circle.test5", "struct-use-11-f")]
        [TestCase(".test6:first-child", "struct-use-11-f")]
        [TestCase(".sibling + circle.test7", "struct-use-11-f")]
        [TestCase("circle[cx].test8", "struct-use-11-f")]
        [TestCase("circle[cx=\"50\"].test9", "struct-use-11-f")]
        [TestCase("circle[foo~=\"warning1\"].test10", "struct-use-11-f")]
        [TestCase("circle[lang|=\"en\"].test11", "struct-use-11-f")]
        [TestCase(".test12", "struct-use-11-f")]
        [TestCase(".twochildren:first-child", "struct-use-11-f")]
        [TestCase("defs > rect",  "struct-use-10-f")]
        [TestCase(".testclass1",  "struct-use-10-f")] 
        [TestCase("#testid1 .testclass1",  "struct-use-10-f")] 
        [TestCase("g .testClass1",  "struct-use-10-f")] 
        [TestCase("#g1 .testclass2",  "struct-use-10-f")]
        [TestCase("g#g1",  "struct-use-10-f")] 
        [TestCase("#testid2",  "struct-use-10-f")] 
        [TestCase("g#g2",  "struct-use-10-f")] 
        [TestCase(".testclass3 > rect",  "struct-use-10-f")] 
        [TestCase("#testid3 rect",  "struct-use-10-f")] 
        [TestCase("#testid3 rect#testrect3",  "struct-use-10-f")] 
        [TestCase(".mummy",  "styling-css-03-b")]                                     
        [TestCase(".mummy rect",  "styling-css-03-b")]       
        [TestCase(".mummy > .thischild",  "styling-css-03-b")]       
        [TestCase(".child",  "styling-css-03-b")]       
        [TestCase(".gap > .thischild",  "styling-css-03-b")]       
        [TestCase(".daddy",  "styling-css-03-b")]       
        [TestCase(".daddy > .tertius",  "styling-css-03-b")]       
        [TestCase(".primus + .secundus",  "styling-css-03-b")]       
        [TestCase(".daddy :first-child",  "styling-css-03-b")]       
        [TestCase(".st1", "__issue-280-01")]
        [TestCase(".st2", "__issue-280-01")]
        [TestCase(".st3", "__issue-280-01")]
        [TestCase(".st4", "__issue-280-01")]
        [TestCase(".st5", "__issue-280-01")]
        [TestCase(".st6", "__issue-280-01")]
        [TestCase(".st7", "__issue-280-01")]
        [TestCase(".st8", "__issue-280-01")]
        [TestCase(".st9", "__issue-280-01")]
        [TestCase("rect","styling-css-01-b")]
        [TestCase(".warning", "styling-css-01-b")]
        [TestCase(".bar","styling-css-01-b")]
        [TestCase(".line","__issue-034-02")]               
        [TestCase(".bold-line","__issue-034-02")]                 
        [TestCase(".thin-line","__issue-034-02")]                  
        [TestCase(".filled","__issue-034-02")]                     
        [TestCase("text.terminal","__issue-034-02")]               
        [TestCase("text.nonterminal","__issue-034-02")]            
        [TestCase("text.regexp","__issue-034-02")]                
        [TestCase("rect, circle, polygon","__issue-034-02")]       
        [TestCase("rect.terminal","__issue-034-02")]               
        [TestCase("rect.nonterminal","__issue-034-02")]            
        [TestCase("rect.text","__issue-034-02")]                   
        [TestCase("polygon.regexp","__issue-034-02")]    
        [TestCase("path:hover","__issue-315-01")]
        [TestCase("path","__issue-315-01")]
        [TestCase("text","styling-css-04-f")]
        [TestCase("rect","styling-css-04-f")]
        [TestCase("#test-frame","styling-css-04-f")]
        [TestCase("g#alpha","styling-css-04-f")]
        [TestCase("a#alpha","styling-css-04-f")]
        [TestCase("#alpha","styling-css-04-f")]
        [TestCase("#alpha-2 > rect","styling-css-04-f")]
        [TestCase("#beta rect","styling-css-04-f")]
        [TestCase("g#gamma * g * * rect","styling-css-04-f")]
        [TestCase("g#gamma * * rect","styling-css-04-f")]
        [TestCase("[stroke-width=\"1.0001\"]","styling-css-04-f")]
        [TestCase("g#delta rect[stroke-width=\"1.0002\"]","styling-css-04-f")]
        [TestCase("g#delta > rect[stroke-width=\"1.0003\"]","styling-css-04-f")]
        [TestCase("#delta + g > *","styling-css-04-f")]
        [TestCase("g#delta + g > rect + rect","styling-css-04-f")]
        [TestCase("#delta + g#epsilon * rect:first-child","styling-css-04-f")]
        [TestCase("#zeta [cursor]","styling-css-04-f")]
        [TestCase("g#zeta [cursor=\"help\"]","styling-css-04-f")]
        [TestCase("g#zeta [rx~=\"3E\"]","styling-css-04-f")]
        [TestCase("g#epsilon + g [stroke-dasharray|=\"3.1415926\"]","styling-css-04-f")]
        [TestCase("g#epsilon + g > rect.hello","styling-css-04-f")]
        [TestCase("g#eta rect:first-child","styling-css-04-f")]
        public void RunSelectorTests(string selector, string baseName)
        {
            var elementFactory = new SvgElementFactory();
            var testSuite = Path.Combine(ImageTestDataSource.SuiteTestsFolder, "W3CTestSuite");
            string basePath = testSuite;
            var svgPath = Path.Combine(basePath, "svg", baseName + ".svg");
            var styles = new List<ISvgNode>();
            using (var xmlFragment = File.Open(svgPath, FileMode.Open))
            {
                using (var xmlTextReader = new XmlTextReader(xmlFragment))
                {
                    var svgDocument = SvgDocument.Create<SvgDocument>(xmlTextReader, elementFactory, styles);

                    var rootNode = new NonSvgElement();
                    rootNode.Children.Add(svgDocument);

                    TestSelector(selector, rootNode, elementFactory);
                }
            }
        }

        [Test]
        [TestCase("*:first-child")]
        [TestCase("p:first-child")]
        [TestCase("*:last-child")]
        [TestCase("p:last-child")]
        [TestCase("*:only-child")]
        [TestCase("p:only-child")]
        [TestCase("*:empty")]
        [TestCase(":nth-child(2)")]
        [TestCase("*:nth-child(2)")]
        [TestCase("p:nth-child(2)")]
        [TestCase(":nth-last-child(2)")]
        [TestCase("#myDiv :nth-last-child(2)")]
        [TestCase("span:nth-last-child(3)")]
        [TestCase("span:nth-last-child(2)")]
        [TestCase("p.hiclass,a")]
        [TestCase("p.hiclass, a")]
        [TestCase("p.hiclass , a")]
        [TestCase("p.hiclass ,a")]
        [TestCase("#myDiv")]
        [TestCase("div#myDiv")]
        [TestCase("#theBody #myDiv")]
        [TestCase("#theBody #whatwhatwhat")]
        [TestCase("#whatwhatwhat #someOtherDiv")]
        [TestCase("#myDiv *")]
        [TestCase("div#myDiv")]
        [TestCase("#theBody #myDiv")]
        [TestCase("#theBody #whatwhatwhat")]
        [TestCase("#whatwhatwhat #someOtherDiv")]
        [TestCase("#myDiv *")]
        [TestCase("#theBody>#myDiv")]
        [TestCase("#theBody>#someOtherDiv")]
        [TestCase("#myDiv>*")]
        [TestCase("#someOtherDiv>*")]
        [TestCase("*")]
        [TestCase("body")]
        [TestCase("p")]
        [TestCase("head p")]
        [TestCase("div p")]
        [TestCase("div a")]
        [TestCase("div p a")]
        [TestCase("div p a")]
        [TestCase("div div")]
        [TestCase("form input")]
        [TestCase(".checkit")]
        [TestCase(".omg.ohyeah")]
        [TestCase("p.ohyeah")]
        [TestCase("p.ohyeah")]
        [TestCase("div .ohyeah")]
        [TestCase("div > p")]
        [TestCase("div> p")]
        [TestCase("div >p")]
        [TestCase("div>p")]
        [TestCase("div > p.ohyeah")]
        [TestCase("p > *")]
        [TestCase("div > * > *")]
        [TestCase("a + span")]
        [TestCase("a+ span")]
        [TestCase("a +span")]
        [TestCase("a+span")]
        [TestCase("a + span, div > p")]
        [TestCase("div ~ form")]
        [TestCase("div[id]")]
        [TestCase("div[id=\"someOtherDiv\"]")]
        [TestCase("p[class~=\"ohyeah\"]")]
        [TestCase("p[class~='']")]
        [TestCase("span[class|=\"separated\"]")]
        [TestCase("[class=\"checkit\"]")]
        [TestCase("*[class=\"checkit\"]")]
        [TestCase("*[class=\"checkit\"]")]
        [TestCase("*[class^=check]")]
        [TestCase("*[class^='']")]
        [TestCase("*[class$=it]")]
        [TestCase("*[class$='']")]
        [TestCase("*[class*=heck]")]
        [TestCase("*[class*='']")]
        [TestCase("p[class!='hiclass']")]
        [TestCase(":nth-of-type(2)")]
        [TestCase(":nth-last-of-type(2)")]
        [TestCase(":not(p)")]
        [TestCase(":root")]
        public void RunSelectorTests(string selector)
        {
            string baseName = "struct-use-11-f";
            var elementFactory = new SvgElementFactory();
            var testSuite = Path.Combine(ImageTestDataSource.SuiteTestsFolder, "W3CTestSuite");
            string basePath = testSuite;
            var svgPath = Path.Combine(basePath, "svg", baseName + ".svg");
            var styles = new List<ISvgNode>();
            using (var xmlFragment = File.Open(svgPath, FileMode.Open))
            {
                using (var xmlTextReader = new XmlTextReader(xmlFragment))
                {
                    var svgDocument = SvgDocument.Create<SvgDocument>(xmlTextReader, elementFactory, styles);

                    var rootNode = new NonSvgElement();
                    rootNode.Children.Add(svgDocument);

                    TestSelector(selector, rootNode, elementFactory);
                }
            }
        }

        private void TestSelector(string selector, NonSvgElement rootNode, SvgElementFactory elementFactory)
        {
            ////SvgElementOps.NodeDebug = SvgElementOps.NodeDebug = string.Empty; // nameof(SvgElementOpsFunc.Child);
            bool fizzler = !(selector.Contains("nth-child") || selector.Contains("nth-last-child"));

            List<SvgElement> fizzlerElements = null;
            Debug.WriteLine(Environment.NewLine);
            try
            {
                Debug.WriteLine("Fizzler:\r\n");
                fizzlerElements = QuerySelectorFizzlerAll(rootNode, selector, elementFactory).OrderBy(f => f.ElementName).ToList();
                Debug.WriteLine(Environment.NewLine);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                fizzler = false;
            }

            Debug.WriteLine("ExCss:\r\n");
            var exCssElements = QuerySelectorExCssAll(rootNode, selector, elementFactory).OrderBy(f => f.ElementName).ToList();
            Debug.WriteLine(Environment.NewLine);

            if (fizzler)
            {
                var areEqualFizzler = fizzlerElements.SequenceEqual(exCssElements);
                Assert.IsTrue(areEqualFizzler, "should select the same elements");
            }
            else
            {
                Assert.Inconclusive("Fizzler can't handle this selector");
            }
        }

        private IEnumerable<SvgElement> QuerySelectorExCssAll(NonSvgElement elem, string selector, SvgElementFactory elementFactory)
        {
            var stylesheetParser = new StylesheetParser(true, true);
            var stylesheet = stylesheetParser.Parse(selector + " {color:black}");
            var exCssSelector = stylesheet.StyleRules.First().Selector;
            return elem.QuerySelectorAll(exCssSelector, elementFactory);
        }

        private IEnumerable<SvgElement> QuerySelectorFizzlerAll(NonSvgElement elem, string selector, SvgElementFactory elementFactory)
        {
            var generator = new SelectorGenerator<SvgElement>(new SvgElementOps(elementFactory));
            Fizzler.Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(elem, 1));
        }
    }

    public class TestSvg
    {
        private readonly string _baseName;
        private readonly string _folder;

        private TestSvg(string baseName, string folder)
        {
            _baseName = baseName;
            _folder = folder;
        }

        public override string ToString()
        {
            return $"TestSvg - {BaseName}";
        }

        public string BaseName => _baseName;
        public string Folder => _folder;

        public static IEnumerable<TestSvg> AllSvgs()
        {
            var basePath = ImageTestDataSource.SuiteTestsFolder;
            var testSuite = Path.Combine(basePath, "W3CTestSuite", "svg");
            // Enumerate all Test Svgs
            foreach(var baseName in Directory.EnumerateFiles(testSuite))
            {
                if (Path.GetExtension(baseName) == ".svg")
                {
                    yield return new TestSvg(Path.GetFileNameWithoutExtension(baseName), testSuite);
                }
            } 
        }

        public static IEnumerable<TestSvg> AllImageSvgs()
        {
            var basePath = ImageTestDataSource.SuiteTestsFolder;
            var testSuite = Path.Combine(basePath, "W3CTestSuite", "images");
            // Enumerate all Test Svgs
            foreach(var baseName in Directory.EnumerateFiles(testSuite))
            {
                if (Path.GetExtension(baseName) == ".svg")
                {
                    yield return new TestSvg(
                        Path.GetFileNameWithoutExtension(baseName),
                        testSuite);
                }
            } 
        }
    }
}
