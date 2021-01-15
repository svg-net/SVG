using System;
using BenchmarkDotNet.Attributes;

namespace Svg.Benchmark
{
    public class SpanBenchmarks
    {
        [Benchmark(Baseline = true)]
        public void StringEqualityOperator()
        {
            var colour = "ActiveBorder".Trim().ToLowerInvariant();

            var equal = colour == "activeborder";
        }

        [Benchmark]
        public void MemoryExtensionsSequenceEqual()
        {
            var colour = "ActiveBorder".AsSpan().Trim();

            Span<char> buffer = stackalloc char[32];
            var length = colour.ToLowerInvariant(buffer);

            var span = buffer.Slice(0, length);

            var equal = span.SequenceEqual("activeborder".AsSpan());
        }
    }
}