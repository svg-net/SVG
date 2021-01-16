using BenchmarkDotNet.Attributes;

namespace Svg.Benchmark
{
    public class SvgDocumentCreationBenchmarks
    {
        [Benchmark(Baseline = true)]
        public void SvgDocument_new()
        {
            var result = new SvgDocument();
        }

        private static string EmptySvg = @"<svg xmlns='http://www.w3.org/2000/svg'></svg>";

        [Benchmark]
        public void SvgDocument_new_FromSvg_Empty()
        {
            var doc = SvgDocument.FromSvg<SvgDocument>(EmptySvg);
        }

        [Benchmark]
        public void SvgDocument_new_FromSvg_Empty_Fast()
        {
            SvgDocument.SkipGdiPlusCapabilityCheck = true;
            SvgDocument.DisableDtdProcessing = true;
            SvgDocument.PointsPerInch = 96;
            var doc = SvgDocument.FromSvg<SvgDocument>(EmptySvg);
            SvgDocument.DisableDtdProcessing = false;
            SvgDocument.SkipGdiPlusCapabilityCheck = false;
        }
    }
}
