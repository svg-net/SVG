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
        public void RunSelectorTests(string baseName)
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
        public void RunSelectorsOnSvgTests(TestSvg svg)
        {
            RunSelectorTests(svg.BaseName);
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

        private void TestSelector(string selector, NonSvgElement rootNode, SvgElementFactory elementFactory)
        {
            ////SvgElementOps.NodeDebug = SvgElementOps.NodeDebug = string.Empty; // nameof(SvgElementOpsFunc.Child);

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine("Fizzler:\r\n");
            var fizzlerElements = QuerySelectorFizzlerAll(rootNode, selector, elementFactory).ToList();
            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine("ExCss:\r\n");
            var exCssElements = QuerySelectorExCssAll(rootNode, selector, elementFactory).ToList();
            Debug.WriteLine(Environment.NewLine);

            var areEqual = fizzlerElements.SequenceEqual(exCssElements);
            if (!areEqual)
            {
                Assert.IsTrue(areEqual, "should select the same elements");
            }
            else
            {
                Assert.IsTrue(areEqual, "should select the same elements");
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

        private TestSvg(string baseName)
        {
            _baseName = baseName;
        }

        public override string ToString()
        {
            return $"TestSvg - {BaseName}";
        }

        public string BaseName => _baseName;

        public static IEnumerable<TestSvg> AllSvgs()
        {
            var basePath = ImageTestDataSource.SuiteTestsFolder;
            var testSuite = Path.Combine(basePath, "W3CTestSuite", "svg");
            // Enumerate all Test Svgs
            foreach(var baseName in Directory.EnumerateFiles(testSuite))
            {
                if (Path.GetExtension(baseName) == ".svg")
                {
                    yield return new TestSvg(Path.GetFileNameWithoutExtension(baseName));
                }
            } 
        }
    }
}
