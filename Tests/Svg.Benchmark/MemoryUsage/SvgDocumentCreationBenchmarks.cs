using BenchmarkDotNet.Attributes;

namespace Svg.Benchmark
{
    public class SvgDocumentCreationBenchmarks
    {
        [Benchmark]
        public void SvgDocument_new()
        {
            var result = new SvgDocument();
        }
    }
}