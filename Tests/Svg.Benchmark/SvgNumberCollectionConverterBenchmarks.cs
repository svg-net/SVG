using System;
using BenchmarkDotNet.Attributes;

namespace Svg.Benchmark
{
    public class SvgNumberCollectionConverterBenchmarks
    {
        [Benchmark]
        public void SvgNumberCollectionConverter_Parse()
        {
#if false
            SvgNumberCollectionConverter.Parse("1.6 3.2 1.2 5");
            SvgNumberCollectionConverter.Parse("1.6,3.2,1.2,5");
#else
            SvgNumberCollectionConverter.Parse("1.6 3.2 1.2 5".AsSpan());
            SvgNumberCollectionConverter.Parse("1.6,3.2,1.2,5".AsSpan());
#endif
        }
    }
}
