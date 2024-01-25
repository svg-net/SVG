using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

using Svg.Tests.Common;

namespace Svg.UnitTests
{
    /// <summary>
    /// </summary>
    [TestFixture]
    public class ImageComparisonTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.TestContext = TestContext.CurrentContext;
            TestsUtils.EnsureTestsExists(ImageTestDataSource.SuiteTestsFolder).Wait();
        }

        public TestContext TestContext { get; set; }

#if NETSTANDARD || NETCOREAPP
        /// <summary>
        /// Compares SVG images against reference PNG images from the W3C SVG 1.1 test suite.
        /// This tests 158 out of 179 passing tests - the rest will not pass
        /// the test for several reasons.
        /// Note that with the current test there are still a lot of false positives,
        /// so this is not a definitive test for image equality yet.
        /// </summary>
        [Test]
        [TestCaseSource(typeof(ImageTestDataSource), nameof(ImageTestDataSource.PassingTests))]
        public void CompareSvgImageWithReference(ImageTestDataSource.TestData testData)
        {
            string basePath = testData.BasePath;
            string baseName = testData.BaseName;
            bool testSaveLoad = !baseName.StartsWith("#");
            if (!testSaveLoad)
            {
                baseName = baseName.Substring(1);
            }
            var svgPath = Path.Combine(basePath, "svg", baseName + ".svg");
            var pngPath = Path.Combine(basePath, "png", baseName + ".png");
            CompareSvgImageWithReferenceImpl(baseName, svgPath, pngPath, testSaveLoad);
        }
#else
        [Test]
        [TestCaseSource(typeof(ImageTestDataSource), "PassingTests")]
        public void CompareSvgImageWithReference(ImageTestDataSource.TestData testData)
        {
            // W3C Test Suites use external references to local fonts
            SvgDocument.ResolveExternalXmlEntites = ExternalType.Local;
            SvgDocument.ResolveExternalElements = ExternalType.Local;

            var basePath = testData.BasePath;
            while (!basePath.ToLower().EndsWith("svg"))
            {
                basePath = Path.GetDirectoryName(basePath);
            }
            // var svgBasePath = Path.Combine(basePath, "svg");
            var baseName = testData.BaseName;
            bool testSaveLoad = !baseName.StartsWith("#");
            if (!testSaveLoad)
            {
                baseName = baseName.Substring(1);
            }
            var testsRoot = Path.Combine(basePath, "Tests");
            basePath = TestsUtils.GetPath(testsRoot, baseName);
            // basePath = Path.Combine(Path.Combine(basePath, "Tests"), "Issues");
            var svgPath = Path.Combine(Path.Combine(basePath, "svg"), baseName + ".svg");
            var pngPath = Path.Combine(Path.Combine(basePath, "png"), baseName + ".png");
            CompareSvgImageWithReferenceImpl(baseName, svgPath, pngPath, testSaveLoad);
        }
#endif

        private void CompareSvgImageWithReferenceImpl(string baseName, string svgPath, string pngPath, bool testSaveLoad)
        {
            var svgDoc = LoadSvgDocument(svgPath);
            Assert.IsNotNull(svgDoc);
            bool useFixedSize = !baseName.StartsWith("__");

            using (var pngImage = Image.FromFile(pngPath))
            {
                using (var svgImage = LoadSvgImage(svgDoc, useFixedSize))
                {
                    Assert.AreNotEqual(null, pngImage, "Failed to load " + pngPath);
                    Assert.AreNotEqual(null, svgImage, "Failed to load " + svgPath);
                    var difference = svgImage.PercentageDifference(pngImage);
                    Assert.IsTrue(difference < 0.05, baseName + ": Difference is " + (difference * 100.0).ToString() + "%");
                }
                if (!testSaveLoad)
                {
                    // for some images, save/load is still failing
                    return;
                }

                // test save/load
                using (var memStream = new MemoryStream())
                {
                    svgDoc.Write(memStream);
                    memStream.Position = 0;
                    var baseUri = svgDoc.BaseUri;
                    svgDoc = SvgDocument.Open<SvgDocument>(memStream);
                    svgDoc.BaseUri = baseUri;
                    using (var svgImage = LoadSvgImage(svgDoc, useFixedSize))
                    {
                        Assert.IsNotNull(svgImage);
                        var difference = svgImage.PercentageDifference(pngImage);
                        Assert.IsTrue(difference < 0.05, baseName + ": Difference is " + (difference * 100.0).ToString() + "%");
                    }
                }
            }
        }

        /// <summary>
        /// Enable this test to output the calculate percentage difference
        /// of all considered W3C tests.
        /// Can be used to enhance the difference calculation.
        /// </summary>
        // [Test]
        public void RecordDiffForAllSvgImagesWithReference()
        {
            var testsRoot = ImageTestDataSource.SuiteTestsFolder;
            //string[] lines = File.ReadAllLines(@"..\..\..\..\Tests\Svg.UnitTests\all.csv");
            string[] lines = File.ReadAllLines(Path.Combine(testsRoot, @"Svg.UnitTests\AllTests.csv"));
            TestContext.Progress.WriteLine("RecordDiffForAllSvgImagesWithReference: Outputs");
            foreach (var baseName in lines)
            {
                if (baseName.Equals("BaseName"))
                    continue; // Skip the column header
                var basePath = TestsUtils.GetPath(testsRoot, baseName);

                var svgPath = Path.Combine(Path.Combine(basePath, "svg"), baseName + ".svg");
                var pngPath = Path.Combine(Path.Combine(basePath, "png"), baseName + ".png");
                if (File.Exists(pngPath) && File.Exists(svgPath))
                {
                    var svgDoc = LoadSvgDocument(svgPath);
                    bool useFixedSize = !baseName.StartsWith("__");
                    using (var pngImage = Image.FromFile(pngPath))
                    using (var svgImage = LoadSvgImage(svgDoc, useFixedSize))
                    {
                        var difference = svgImage.PercentageDifference(pngImage);
                        TestContext.Progress.WriteLine("    " + baseName + " " + (difference * 100.0).ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Load the SVG image the same way as in the SVGW3CTestRunner.
        /// </summary>
        private static Bitmap LoadSvgImage(SvgDocument svgDoc, bool usedFixedSize)
        {
            Bitmap svgImage = null;
            try
            {
                if (usedFixedSize)
                {
                    svgImage = new Bitmap(480, 360);
                    svgDoc.Draw(svgImage);
                }
                else
                {
                    svgImage = svgDoc.Draw();
                }
            }
            catch (Exception)
            {
                svgImage?.Dispose();
                throw;
            }
            return svgImage;
        }

        private static SvgDocument LoadSvgDocument(string svgPath)
        {
            return SvgDocument.Open(svgPath);
        }
    }

    /// <summary>
    /// Helper class to read the datasource for the image tests.
    /// The datasource will read the embedded resource with the Tests and will pass a TestData class with the data to test.
    /// </summary>
    public class ImageTestDataSource
    {
        public class TestData
        {
            public string BasePath { get; set; }
            public string BaseName { get; set; }
            public override string ToString()
            {
                return $"TestDataSource - {BaseName}";
            }
        }

        public static IEnumerable<TestData> PassingTests()
        {
            var basePath = SuiteTestsFolder;
            var W3CPath = Path.Combine(basePath, TestsUtils.W3CTests);
            var IssuesPath = Path.Combine(basePath, TestsUtils.IssuesTests);
            var rows = new ImageTestDataSource().LoadRowsFromResourceCsv().Skip(1); // Skip header row
            foreach (var row in rows)
            {
                var testSuite = row.StartsWith(TestsUtils.IssuesPrefix) ? IssuesPath : W3CPath;

                yield return new TestData() { BasePath = testSuite, BaseName = row };
            }
        }

        private const string ResourceIdentifier = "PassingTests.csv";

        /// <summary>
        /// Read the rows from the resource
        /// </summary>
        /// <returns></returns>
        private string[] LoadRowsFromResourceCsv()
        {
            var assembly = typeof(ImageTestDataSource).Assembly;
            string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.IndexOf(ResourceIdentifier) > -1);
            if (resourceName == null) throw new Exception($"Cannot find data resource: {ResourceIdentifier}");
            var res = string.Empty;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                res = reader.ReadToEnd();
            }

            return res.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string _basePath = null;

        /// <summary>
        /// Determine the folder the testsuite is running in.
        /// </summary>
        public static string SuiteTestsFolder
        {
            get
            {
                if (_basePath != null) return _basePath;
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                while (!basePath.ToLower().EndsWith("svg"))
                {
                    basePath = Path.GetDirectoryName(basePath);
                }

                _basePath = Path.Combine(basePath, "Tests");
                return _basePath;
            }
        }
    }
}
