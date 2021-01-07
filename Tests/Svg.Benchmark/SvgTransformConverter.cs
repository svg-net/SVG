using System.Globalization;
using BenchmarkDotNet.Attributes;
using Svg;
using Svg.Transforms;

namespace Svg.Benchmarks
{
    public class SvgTransformConverterBenchmarks
    {
        private static readonly SvgTransformConverter TransformConverter = new SvgTransformConverter();

        [Benchmark]
        public void SvgTransformConverter_ConvertFrom_Matrix_1()
        {
            TransformConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "matrix(252,0,0,252,7560,11340)");
        }

        [Benchmark]
        public void SvgTransformConverter_ConvertFrom_Matrix_2()
        {
            TransformConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "matrix(-4.37114e-08,1,-1,-4.37114e-08,181,409.496)");
        }

        [Benchmark]
        public void SvgTransformConverter_ConvertFrom_Matrix_3()
        {
            TransformConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "matrix(0.74811711,0.48689734,-0.42145482,0.93331568,324.55155,94.282562)");
        }

        [Benchmark]
        public void SvgTransformConverter_ConvertFrom_Matrix_4()
        {
            TransformConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "matrix(0.879978,0.475015,-0.475015,0.879978,120.2732,-136.2899)");
        }

        [Benchmark]
        public void SvgTransformConverter_ConvertFrom_Rotate_Translate()
        {
            TransformConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "rotate(180), translate(-50, 0)");
        }

        [Benchmark]
        public void SvgTransformConverter_ConvertFrom_Translate_Rotate()
        {
            TransformConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "translate(9, 241) rotate(-90)");
        }

        [Benchmark]
        public void SvgTransformConverter_ConvertFrom_Matrix_Rotate_Scale()
        {
            TransformConverter.ConvertFrom(null, CultureInfo.InvariantCulture, "rotate(180 2.5 2.5) scale(0.7142857142857143,0.7142857142857143)");
        }
    }
}
