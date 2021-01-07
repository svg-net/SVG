using System.Globalization;
using BenchmarkDotNet.Attributes;
using Svg;
using Svg.Transforms;

namespace Svg.Benchmark
{
    public class SvgUnitConverterBenchmarks
    {
        [Benchmark]
        public void SvgUnitConverter_ConvertFrom()
        {
            SvgUnitConverter.Parse("1pt");
            SvgUnitConverter.Parse("1.25px");
            SvgUnitConverter.Parse("1pc");
            SvgUnitConverter.Parse("15px");
            SvgUnitConverter.Parse("1mm");
            SvgUnitConverter.Parse("3.543307px");
            SvgUnitConverter.Parse("1cm");
            SvgUnitConverter.Parse("35.43307px");
            SvgUnitConverter.Parse("1in");
            SvgUnitConverter.Parse("90px");
            SvgUnitConverter.Parse("15em");
            SvgUnitConverter.Parse("0.2822222mm");
            SvgUnitConverter.Parse("3990");
            SvgUnitConverter.Parse("1990");
            SvgUnitConverter.Parse("-50");
            SvgUnitConverter.Parse(".4in");
            SvgUnitConverter.Parse(".25em");
            SvgUnitConverter.Parse("10%");
            SvgUnitConverter.Parse("1%");
            SvgUnitConverter.Parse("0%");
            SvgUnitConverter.Parse("100%");
            SvgUnitConverter.Parse("1.2em");
            SvgUnitConverter.Parse("medium");
            SvgUnitConverter.Parse("x-small");
            SvgUnitConverter.Parse("xx-large");
            SvgUnitConverter.Parse("657.45");
            SvgUnitConverter.Parse("12.5");
            SvgUnitConverter.Parse("0");
            SvgUnitConverter.Parse("12");
        }
    }
}
