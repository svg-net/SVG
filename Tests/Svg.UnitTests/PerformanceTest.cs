using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Reflection;

using NUnit.Framework;

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

        private static readonly SvgUnitConverter UnitConverter = new SvgUnitConverter();

        [Test]
        public void UnitConverterProfiler()
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
