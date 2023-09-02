using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BenchmarkDotNet.Attributes;
using ExCSS;
using Svg.Css;

namespace Svg.Benchmark
{
    public class CssQueryPerformanceBenchmark
    {
        private readonly List<ISvgNode> _styles;
        private readonly SvgDocument _svgDokument;
        private readonly List<IStyleRule> _rules;
        private SvgElementFactory _svgElementFactory;

        private Stream Open(string name) => typeof(Program).Assembly.GetManifestResourceStream($"Svg.Benchmark.Assets.{name}");

        CssQueryPerformanceBenchmark()
        {
            using var stream = Open("struct-use-11-f");
            // Don't close the stream via a dispose: that is the client's job.
            var reader = new SvgTextReader(stream, null)
            {
                XmlResolver = new SvgDtdResolver(),
                WhitespaceHandling = WhitespaceHandling.Significant,
                DtdProcessing = DtdProcessing.Ignore,
            };

            _styles = new List<ISvgNode>();
            _svgElementFactory = new SvgElementFactory();
            _svgDokument = SvgDocument.Create<SvgDocument>(reader, _svgElementFactory, _styles);

            var cssTotal = string.Join(Environment.NewLine, _styles.Select(s => s.Content).ToArray());
            var stylesheetParser = new StylesheetParser(true, true, tolerateInvalidValues: true);
            var stylesheet = stylesheetParser.Parse(cssTotal);
            _rules = stylesheet.StyleRules.ToList();
        }

        [Benchmark]
        public void SelectorPerformanceBenchmark()
        {
            var rootNode = new NonSvgElement();
            rootNode.Children.Add(_svgDokument);

            foreach (var rule in _rules)
            {
                rootNode.QuerySelectorAll(rule.Selector, _svgElementFactory);
            }
        }


    }
}
