using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace Svg.UnitTests
{
    public abstract class SvgTestHelper
    {

        /// <summary>
        /// Test file path.
        /// </summary>
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
                xmlDoc.Load(s);
                Trace.WriteLine("Done XmlDocument loading from resource data.");
                return xmlDoc;
            }
        }


        /// <summary>
        /// Get xml document from <see cref="TestFile"/>.
        /// </summary>
        /// <returns>File data as xml document.</returns>
        protected virtual XmlDocument GetXMLDocFromFile()
        {
            return GetXMLDocFromFile(TestFile);
        }


        /// <summary>
        /// Get xml document from file.
        /// </summary>
        /// <param name="file">File to load.</param>
        /// <returns>File data as xml document.</returns>
        protected virtual XmlDocument GetXMLDocFromFile(string file)
        {
            if (!File.Exists(file))
                Assert.Fail("Test file missing.", file);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(File.ReadAllText(file));
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
            var svgDoc = SvgDocument.Open(xml);
            Trace.WriteLine("Done SvgDocument open xml.");

            Trace.WriteLine("Draw svg.");
            var img = svgDoc.Draw();
            Trace.WriteLine("Done drawing.");

            CheckSvg(svgDoc, img);
        }


        /// <summary>
        /// Check svg file data.
        /// </summary>
        /// <param name="svgDoc">Svg document.</param>
        /// <param name="img">Image of svg file from <paramref name="svgDoc"/>.</param>
        protected virtual void CheckSvg(SvgDocument svgDoc, Bitmap img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Flush();
                Assert.IsTrue(ms.Length >= ExpectedSize, "Svg file does not match expected minimum size.");
            }
        }
    }
}
