using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml;

namespace Svg.UnitTests
{
    public abstract class SvgTestHelper
    {
        /// <summary>
        /// Test file path.
        /// </summary>
        [Obsolete("Try not to use the file loader, please use the resource loader to ensure working of tests on all systems")]
        protected virtual string TestFile
        {
            get
            {
                const string msg = "Test file not overridden.";
                Assert.Inconclusive(msg);
                throw new NotImplementedException(msg);
            }
        }

        /// <summary>
        /// Full Unit Test resource string for test file. 
        /// </summary>
        /// <remarks>
        /// For the full Unit Test resource string you can use <see cref="GetFullResourceString(string)"/>.
        /// </remarks>
        protected virtual string TestResource
        {
            get
            {
                const string msg = "Test resource not overridden.";
                Assert.Inconclusive(msg);
                throw new NotImplementedException(msg);
            }
        }

        /// <summary>
        /// Expected size of svg file after drawing.
        /// </summary>
        protected virtual int ExpectedSize
        {
            get
            {
                const string msg = "Expected size not overridden.";
                Assert.Inconclusive(msg);
                throw new NotImplementedException(msg);
            }
        }

        /// <summary>
        /// Get full Unit Test resource string.
        /// </summary>
        /// <param name="resourcePath">Resource path.</param>
        /// <returns>Full resource string.</returns>
        /// <example>
        /// var s = GetFullResourceString("Issue204_PrivateFont.Text.svg");
        /// // s content: "Svg.UnitTests.Resources.Issue204_PrivateFont.Text.svg"
        /// </example>
        protected virtual string GetFullResourceString(string resourcePath)
        {
            const string DefaultResourcesDir = "Resources";
            return string.Format("{0}.{1}.{2}",
                this.GetType().Assembly.GetName().Name,
                DefaultResourcesDir,
                resourcePath);
        }

        /// <summary>
        /// Get embedded resource as stream from Unit Test resources.
        /// </summary>
        /// <param name="fullResourceString">Full Unit Test resource string.</param>
        /// <returns>Embedded resource data steam.</returns>
        /// <remarks>Do not forget to close, dispose the stream.</remarks>
        protected virtual Stream GetResourceStream(string fullResourceString)
        {
            Trace.WriteLine("Get resource data.");
            var s = this.GetType().Assembly.GetManifestResourceStream(fullResourceString);
            if (s == null)
                Assert.Fail("Unable to find embedded resource", fullResourceString);
            Trace.WriteLine("Done getting resource data.");
            return s;
        }

        /// <summary>
        /// Get embedded resource as byte array from Unit Test resources.
        /// </summary>
        /// <param name="fullResourceString">Full Unit Test resource string.</param>
        /// <returns>Embedded resource data bytes.</returns>
        protected virtual byte[] GetResourceBytes(string fullResourceString)
        {
            using (var s = GetResourceStream(fullResourceString))
            {
                var resource = new byte[s.Length];
                s.Read(resource, 0, (int)s.Length);
                return resource;
            }
        }

        /// <summary>
        /// Get embedded resource as xml document from Unit Test resources.
        /// </summary>
        /// <param name="fullResourceString">Full Unit Test resource string.</param>
        /// <returns>Embedded resource data xml document.</returns>
        protected virtual XmlDocument GetResourceXmlDoc(string fullResourceString)
        {
            using (var s = GetResourceStream(fullResourceString))
            {
                Trace.WriteLine("Load XmlDocument from resource data.");
                var xmlDoc = new XmlDocument();
                xmlDoc.XmlResolver = new SvgDtdResolver();
                xmlDoc.Load(s);
                Trace.WriteLine("Done XmlDocument loading from resource data.");
                return xmlDoc;
            }
        }

        /// <summary>
        /// Get embedded resource as string from Unit Test resources.
        /// </summary>
        /// <param name="fullResourceString">Full Unit Test resource string.</param>
        /// <returns>Embedded resource data xml as string.</returns>
        protected virtual string GetResourceXmlDocAsString(string fullResourceString)
        {
            using (var s = GetResourceStream(fullResourceString))
            {
                Trace.WriteLine("Load XmlDocument content from resource data.");
                using (var reader = new StreamReader(s, Encoding.UTF8))
                {
                    string value = reader.ReadToEnd();
                    Trace.WriteLine("Done XmlDocument content loading from resource data.");
                    return value;
                }
            }
        }

        /// <summary>
        /// Get xml document from <see cref="TestFile"/>.
        /// </summary>
        /// <returns>File data as xml document.</returns>
        [Obsolete("Try not to use the file loader, please use the resource loader to ensure working of tests on all systems")]
        protected virtual XmlDocument GetXMLDocFromFile()
        {
            return GetXMLDocFromFile(TestFile);
        }

        /// <summary>
        /// Get xml document from file.
        /// </summary>
        /// <param name="file">File to load.</param>
        /// <returns>File data as xml document.</returns>
        [Obsolete("Try not to use the file loader, please use the resource loader to ensure working of tests on all systems")]
        protected virtual XmlDocument GetXMLDocFromFile(string file)
        {
            if (!File.Exists(file))
                Assert.Fail("Test file missing.", file);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(File.ReadAllText(file));
            return xmlDoc;
        }

        /// <summary>
        /// Get the xml document from an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual XmlDocument GetXMLDocFromString(string input)
        {
            Trace.WriteLine("Load XmlDocument from input data.");
            var xmlDoc = new XmlDocument();
            xmlDoc.XmlResolver = new SvgDtdResolver();
            xmlDoc.LoadXml(input);
            Trace.WriteLine("Done XmlDocument loading from resource data.");
            return xmlDoc;
        }

        /// <summary>
        /// Get xml document from <see cref="TestResource"/>.
        /// </summary>
        /// <returns>Resource data as xml document.</returns>
        protected virtual XmlDocument GetXMLDocFromResource()
        {
            return GetResourceXmlDoc(TestResource);
        }

        /// <summary>
        /// Get xml document from resource.
        /// </summary>
        /// <param name="fullResourceString">Full Unit Test resource string.</param>
        /// <returns>Resource data as xml document.</returns>
        protected virtual XmlDocument GetXMLDocFromResource(string fullResourceString)
        {
            return GetResourceXmlDoc(fullResourceString);
        }

        /// <summary>
        /// Load, draw and check svg file.
        /// </summary>
        /// <param name="xml">Svg file data.</param>
        protected virtual void LoadSvg(XmlDocument xml)
        {
            Trace.WriteLine("SvgDocument open xml.");
            var svgDoc = OpenSvg(xml);
            Trace.WriteLine("Done SvgDocument open xml.");

            Trace.WriteLine("Draw svg.");
            var img = DrawSvg(svgDoc);
            Trace.WriteLine("Done drawing.");

            CheckSvg(svgDoc, img);
        }

        /// <summary>
        /// Open SVG document from XML document.
        /// </summary>
        /// <param name="xml">XML document.</param>
        /// <returns>Open SVG document.</returns>
        protected virtual SvgDocument OpenSvg(XmlDocument xml)
        {
            return SvgDocument.Open(xml);
        }

        /// <summary>
        /// Draw SVG.
        /// </summary>
        /// <param name="svgDoc">SVG document to draw.</param>
        /// <returns>SVG as image.</returns>
        protected virtual Image DrawSvg(SvgDocument svgDoc)
        {
            return svgDoc.Draw();
        }

        /// <summary>
        /// Check svg file data.
        /// </summary>
        /// <param name="svgDoc">Svg document.</param>
        /// <param name="img">Image of svg file from <paramref name="svgDoc"/>.</param>
        protected virtual void CheckSvg(SvgDocument svgDoc, Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Flush();
                Assert.IsTrue(ms.Length >= ExpectedSize, $"Svg file size {ms.Length} does not match expected minimum size (expected {ExpectedSize}).");
            }
        }

        /// <summary>
        /// Compare Images.
        /// </summary>
        /// <param name="img1">Image 1.</param>
        /// <param name="img2">Image 2.</param>
        /// <returns>If images are completely equal: true; otherwise: false</returns>
        protected virtual bool ImagesAreEqual(Bitmap img1, Bitmap img2)
        {
            float imgEqualPercentage; // To ignore.
            return ImagesAreEqual(img1, img2, out imgEqualPercentage);
        }

        /// <summary>
        /// Compare Images.
        /// </summary>
        /// <param name="img1">Image 1.</param>
        /// <param name="img2">Image 2.</param>
        /// <param name="imgEqualPercentage">Image equal value in percentage. 0.0% == completely unequal. 100.0% == completely equal.</param>
        /// <returns>If images are completely equal: true; otherwise: false</returns>
        protected virtual bool ImagesAreEqual(Bitmap img1, Bitmap img2, out float imgEqualPercentage)
        {
            Bitmap imgDiff; // To ignore.
            return ImagesAreEqual(img1, img2, out imgEqualPercentage, out imgDiff);
        }

        /// <summary>
        /// Compare Images.
        /// </summary>
        /// <param name="img1">Image 1.</param>
        /// <param name="img2">Image 2.</param>
        /// <param name="imgEqualPercentage">Image equal value in percentage. 0.0% == completely unequal. 100.0% == completely equal.</param>
        /// <param name="imgDiff">Image with red pixel where <paramref name="img1"/> and <paramref name="img2"/> are unequal.</param>
        /// <returns>If images are completely equal: true; otherwise: false</returns>
        protected virtual bool ImagesAreEqual(Bitmap img1, Bitmap img2, out float imgEqualPercentage, out Bitmap imgDiff)
        {
            // Defaults.
            var diffColor = Color.Red;

            // Reset.
            imgEqualPercentage = 0;
            imgDiff = null;

            // Requirements.
            if (img1 == null)
                return false;
            if (img2 == null)
                return false;
            if (img1.Size.Width < 1 && img1.Height < 1)
                return false;
            if (!img1.Size.Equals(img2.Size))
                return false;

            // Compare bitmaps.
            imgDiff = new Bitmap(img1.Size.Width, img1.Size.Height);
            int diffPixelCount = 0;
            for (int i = 0; i < img1.Width; ++i)
            {
                for (int j = 0; j < img1.Height; ++j)
                {
                    Color color;
                    if ((color = img1.GetPixel(i, j)) == img2.GetPixel(i, j))
                    {
                        imgDiff.SetPixel(i, j, color);
                    }
                    else
                    {
                        ++diffPixelCount;
                        imgDiff.SetPixel(i, j, diffColor);
                    }
                }
            }

            // Calculate percentage.
            int totalPixelCount = img1.Width * img1.Height;
            var imgDiffFactor = ((float)diffPixelCount / totalPixelCount);
            imgEqualPercentage = imgDiffFactor * 100;

            return (imgDiffFactor == 1f);
        }
    }
}
