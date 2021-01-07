using System.Globalization;
using BenchmarkDotNet.Attributes;
using Svg;
using Svg.Transforms;

namespace Svg.Benchmarks
{
    public class SvgUnitConverterBenchmarks
    {
        private static readonly SvgUnitConverter UnitConverter = new SvgUnitConverter();

        [Benchmark]
        public void SvgUnitConverter_ConvertFrom()
        {
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1pt");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1.25px");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1pc");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "15px");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1mm");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "3.543307px");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1cm");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "35.43307px");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1in");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "90px");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "15em");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "0.2822222mm");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "3990");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1990");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "-50");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, ".4in");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, ".25em");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "10%");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1%");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "0%");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "100%");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "1.2em");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "medium");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "x-small");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "xx-large");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "657.45");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "12.5");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "0");
            UnitConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "12");
        }
    }
}
