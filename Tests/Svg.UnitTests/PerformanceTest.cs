using System;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using NUnit.Framework;
using Svg.Transforms;

namespace Svg.UnitTests
{
    [TestFixture]
    public class PerformanceTest 
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = typeof(PerformanceTest).Assembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Load All W3CSvg Unit tests
        /// On My Machine this Unit Test went from 22 seconds down to 10 seconds. I set the Timeout
        /// to 25 seconds to catch Performance regressions On the Build Server it needs 9 seconds
        /// So I can move down the timeout to 15 seconds
        /// </summary>
        [Test]
        public void LoadAllW3CSvg()
        {
            var svgPath = Path.Combine(AssemblyDirectory, "..", "..", "..", "..", "W3CTestSuite", "svg");
            var files = Directory.GetFiles(svgPath, "*.svg");
            foreach (var file in files)
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        using (var stream = File.OpenRead(file))
                        {
                            if (Path.GetExtension(file) == ".svgz")
                            {
                                var gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                                SvgDocument.Open<SvgDocument>(gzipStream);    
                            }
                            else
                            {
                                SvgDocument.Open<SvgDocument>(stream);    
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception in svg file: {file} with Message: {e.Message}  {Environment.NewLine} Content: {Environment.NewLine} {File.ReadAllText(file)} ");
                    throw;
                }
            }
        }

        [Test]
        public void TransformConverterProfiler()
        {
            SvgTransformConverter.Parse("matrix(252,0,0,252,7560,11340)");
            SvgTransformConverter.Parse("matrix(-4.37114e-08,1,-1,-4.37114e-08,181,409.496)");
            SvgTransformConverter.Parse("matrix(0.74811711,0.48689734,-0.42145482,0.93331568,324.55155,94.282562)");
            SvgTransformConverter.Parse("matrix(0.879978,0.475015,-0.475015,0.879978,120.2732,-136.2899)");
            SvgTransformConverter.Parse("rotate(180), translate(-50, 0)");
            SvgTransformConverter.Parse("translate(9, 241) rotate(-90)");
            SvgTransformConverter.Parse("rotate(180 2.5 2.5) scale(0.7142857142857143,0.7142857142857143)");
        }

        [Test]
        public void UnitConverterProfiler()
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
