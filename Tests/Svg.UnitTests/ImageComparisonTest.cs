using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Svg.UnitTests
{
    /// <summary>
    /// </summary>
    [TestClass]
    public class ImageComparisonTest
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Compares SVG images against reference PNG images from the W3C SVG 1.1 test suite.
        /// This tests 158 out of 179 passing tests - the rest will not pass
        /// the test for several reasons. 
        /// Note that with the current test there are still a lot of false positives,
        /// so this is not a definitive test for image equality yet.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"|DataDirectory|\..\..\PassingTests.csv",
            "PassingTests#csv", DataAccessMethod.Sequential)]
        public void CompareSvgImageWithReference()
        {
            var basePath = TestContext.TestRunDirectory;
            while (!basePath.ToLower().EndsWith("svg"))
            {
                basePath = Path.GetDirectoryName(basePath);
            }
            basePath = Path.Combine(basePath, "Tests", "W3CTestSuite");
            var svgBasePath = Path.Combine(basePath, "svg");
            var baseName = TestContext.DataRow[0] as string;
            var svgPath = Path.Combine(basePath, "svg", baseName + ".svg");
            var pngPath = Path.Combine(basePath, "png", baseName + ".png");
            var pngImage = Image.FromFile(pngPath);
            var svgImage = LoadSvgImage(baseName, svgPath);
            Assert.AreNotEqual(null, pngImage, "Failed to load " + pngPath);
            Assert.AreNotEqual(null, svgImage, "Failed to load " + svgPath);
            var difference = svgImage.PercentageDifference(pngImage);
            Assert.IsTrue(difference < 0.05, 
                baseName + ": Difference is " + (difference * 100.0).ToString() + "%");
        }

        /// <summary>
        /// Enable this test to output the calculate percentage difference
        /// of all considered W3C tests.
        /// Can be used to enhance the difference calculation.
        /// </summary>
        // [TestClass]
        public void RecordDiffForAllSvgImagesWithReference()
        {
            var basePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(
                TestContext.TestRunDirectory)));
            basePath = Path.Combine(basePath, "Tests", "W3CTestSuite");
            var svgBasePath = Path.Combine(basePath, "svg");
            string[] lines = File.ReadAllLines(@"..\..\..\..\Tests\Svg.UnitTests\all.csv");
            foreach (var baseName in lines)
            {
                var svgPath = Path.Combine(basePath, "svg", baseName + ".svg");
                var pngPath = Path.Combine(basePath, "png", baseName + ".png");
                if (File.Exists(pngPath) && File.Exists(svgPath))
                {
                    var pngImage = Image.FromFile(pngPath);
                    var svgImage = LoadSvgImage(baseName, svgPath);
                    if (pngImage != null && svgImage != null)
                    {
                        var difference = svgImage.PercentageDifference(pngImage);
                        Console.WriteLine(baseName + " " + (difference * 100.0).ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Load the SVG image the same way as in the SVGW3CTestRunner.
        /// </summary>
        private static Image LoadSvgImage(string fileName, string svgPath)
        {
            var doc = new SvgDocument();
            Image svgImage;
            try
            {
                doc = SvgDocument.Open(svgPath);
                if (fileName.StartsWith("__"))
                {
                    svgImage = doc.Draw();
                }
                else
                {
                    var img = new Bitmap(480, 360);
                    doc.Draw(img);
                    svgImage = img;
                }
            }
            catch (Exception)
            {
                svgImage = null;
            }
            return svgImage;
        }
    }

    /// <summary>
    /// Taken from https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
    /// and slightly modified.
    /// Image width and height, default threshold and handling of alpha values have been adapted.
    /// </summary>
    public static class ExtensionMethods
    {
        private static int ImageWidth = 64;
        private static int ImageHeight = 64;

        public static float PercentageDifference(this Image img1, Image img2, byte threshold = 10)
        {
            byte[,] differences = img1.GetDifferences(img2);

            int diffPixels = 0;

            foreach (byte b in differences)
            {
                if (b > threshold) { diffPixels++; }
            }

            return diffPixels / (float)(ImageWidth * ImageHeight);
        }

        public static Image Resize(this Image originalImage, int newWidth, int newHeight)
        {
            Image smallVersion = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(smallVersion))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return smallVersion;
        }

        public static byte[,] GetGrayScaleValues(this Image img)
        {
            using (Bitmap thisOne = (Bitmap)img.Resize(ImageWidth, ImageHeight).GetGrayScaleVersion())
            {
                byte[,] grayScale = new byte[ImageWidth, ImageHeight];

                for (int y = 0; y < ImageHeight; y++)
                {
                    for (int x = 0; x < ImageWidth; x++)
                    {
                        var pixel = thisOne.GetPixel(x, y);
                        var alpha = thisOne.GetPixel(x, y).A;
                        var gray = thisOne.GetPixel(x, y).R;
                        grayScale[x, y] = (byte)Math.Abs(gray * alpha / 255);
                    }
                }
                return grayScale;
            }
        }

        //the colormatrix needed to grayscale an image
        static readonly ColorMatrix ColorMatrix = new ColorMatrix(new float[][]
        {
            new float[] {.3f, .3f, .3f, 0, 0},
            new float[] {.59f, .59f, .59f, 0, 0},
            new float[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
        });

        public static Image GetGrayScaleVersion(this Image original)
        {
            //create a blank bitmap the same size as original
            //https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(ColorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return newBitmap;

        }

        public static byte[,] GetDifferences(this Image img1, Image img2)
        {
            Bitmap thisOne = (Bitmap)img1.Resize(ImageWidth, ImageHeight).GetGrayScaleVersion();
            Bitmap theOtherOne = (Bitmap)img2.Resize(ImageWidth, ImageHeight).GetGrayScaleVersion();
            byte[,] differences = new byte[ImageWidth, ImageHeight];
            byte[,] firstGray = thisOne.GetGrayScaleValues();
            byte[,] secondGray = theOtherOne.GetGrayScaleValues();

            for (int y = 0; y < ImageHeight; y++)
            {
                for (int x = 0; x < ImageWidth; x++)
                {
                    differences[x, y] = (byte)Math.Abs(firstGray[x, y] - secondGray[x, y]);
                }
            }
            thisOne.Dispose();
            theOtherOne.Dispose();
            return differences;
        }
    }
}
