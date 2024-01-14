using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.IO.Compression;

namespace Svg.UnitTests
{
    internal static class TestsUtils
    {
        private const string FixImage = "smiley.png";

        private const string SvgExt = "*.svg";
        private const string PngExt = "*.png";

        public static string IssuesPrefix = "__";
        public static string IssuesTests = "Issues";
        public static string W3CTests = "W3CTestSuite";

        private const string W3CTestSuiteUrl
            = "https://github.com/ElinamLLC/SharpVectors-TestSuites/raw/master/Svg11.zip";

        public static string GetPath(string testsRoot, string baseName)
        {
            if (baseName.StartsWith(IssuesPrefix))
            {
                return Path.Combine(testsRoot, IssuesTests);
            }
            return Path.Combine(testsRoot, W3CTests);
        }

        public async static Task EnsureTestsExists(string testsRoot)
        {
            var testsPath = Path.Combine(testsRoot, W3CTests);
            if (!IsTestSuiteAvailable(testsPath))
            {
                var downloadedFilePath = Path.GetFullPath(Path.Combine(testsPath, "Svg11.zip"));
                string destinationDirectory = Path.GetDirectoryName(downloadedFilePath);

                if (File.Exists(downloadedFilePath))
                {
                    File.Delete(downloadedFilePath);
                }

                await DownloadW3CTestSuite(downloadedFilePath);

                ZipFile.ExtractToDirectory(downloadedFilePath, destinationDirectory);

                if (File.Exists(downloadedFilePath))
                {
                    File.Delete(downloadedFilePath);
                }

                var sourceImage = Path.Combine(destinationDirectory, "images", FixImage);
                var destImage = Path.Combine(destinationDirectory, "svg", FixImage);
                File.Copy(sourceImage, destImage);
            }
        }

        private static bool IsDirectoryEmpty(string path, string searchPattern)
        {
            return !Directory.EnumerateFileSystemEntries(path, searchPattern).Any();
        }

        private static bool IsTestSuiteAvailable(string path)
        {
            var svgW3CBasePath = Path.Combine(path, "svg");
            var pngW3CBasePath = Path.Combine(path, "png");

            if (Directory.Exists(svgW3CBasePath) == false)
            {
                return false;
            }
            if (Directory.Exists(pngW3CBasePath) == false)
            {
                return false;
            }
            string svgDir = Path.GetFullPath(svgW3CBasePath);
            if (!Directory.Exists(svgDir) || IsDirectoryEmpty(svgDir, SvgExt) == true)
            {
                return false;
            }
            string pngDir = Path.Combine(pngW3CBasePath);
            if (!Directory.Exists(pngDir) || IsDirectoryEmpty(pngDir, PngExt) == true)
            {
                return false;
            }

            return true;
        }

        private static async Task DownloadW3CTestSuite(string downloadedFilePath)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (HttpClient client = new HttpClient())
            {
                using (Stream streamToReadFrom = await client.GetStreamAsync(W3CTestSuiteUrl))
                {
                    using (Stream streamToWriteTo = new FileStream(downloadedFilePath, FileMode.CreateNew))
                    {
                        await streamToReadFrom.CopyToAsync(streamToWriteTo);
                    }
                }
            }
        }
    }
}
