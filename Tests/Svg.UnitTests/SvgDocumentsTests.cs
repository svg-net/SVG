using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgDocumentTests
    {
        [Test]
        public void GetPointsPerInchDpiTests()
        {
            var pointsPerInch = SvgDocument.PointsPerInch;
            SvgDocument.PointsPerInch = 1;
            var testPointsPerInch = SvgDocument.PointsPerInch;
            Assert.AreEqual(1, testPointsPerInch, "Test sets the Dpi to 1");

            // resets the System Dpi
            SvgDocument.PointsPerInch = pointsPerInch;
            var comparePointsPerInch = SvgDocument.PointsPerInch;
            Assert.AreEqual(pointsPerInch, comparePointsPerInch, "After setting to null the default Implementation should be taken");
        }

        [Test]
        public void WriteNamespaces()
        {
            //Write a minimal SVG document with some addtional meta data
            //Test namespaces and prefixes are applied correctly

            const string NAMESPACE_RDF = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            const string NAMESPACE_DC = "http://purl.org/dc/elements/1.1/";

            SvgDocument svg = new SvgDocument();

            SvgUnknownElement metadata = new SvgUnknownElement("metadata");
            NonSvgElement rdfMetadata = new NonSvgElement("RDF", NAMESPACE_RDF);
            rdfMetadata.Namespaces["rdf"] = NAMESPACE_RDF;

            NonSvgElement rdfMetadataDescription = new NonSvgElement("Description", NAMESPACE_RDF);
            rdfMetadataDescription.Children.Add(new NonSvgElement("creator", NAMESPACE_DC) { Content = "Hello World" });
            rdfMetadata.Children.Add(rdfMetadataDescription);
            metadata.Children.Add(rdfMetadata);
            svg.Children.Add(metadata);

            string xmlString;
            using (MemoryStream ms = new MemoryStream())
            using (StreamReader reader = new StreamReader(ms))
            {
                svg.Write(ms);
                ms.Position = 0;
                xmlString = reader.ReadToEnd();
            }

            Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">
<svg version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:xml=""http://www.w3.org/XML/1998/namespace"">
  <metadata>
    <rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
      <rdf:Description>
        <creator xmlns=""http://purl.org/dc/elements/1.1/"">Hello World</creator>
      </rdf:Description>
    </rdf:RDF>
  </metadata>
</svg>", xmlString);
        }

        [Test]
        public void MetadataRoundTripPreservesStructuredContent()
        {
            const string input = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"" xmlns:dc=""http://purl.org/dc/elements/1.1/"">
    <metadata>
    <rdf:RDF>
        <rdf:Description>
        <dc:title>Roundtrip Title</dc:title>
        </rdf:Description>
    </rdf:RDF>
    </metadata>
</svg>";

            var svg = SvgDocument.FromSvg<SvgDocument>(input);
            Assert.That(svg.Children[0], Is.TypeOf<SvgDocumentMetadata>());

            var output = svg.GetXML();

            StringAssert.Contains("<metadata>", output);
            StringAssert.Contains("<rdf:RDF", output);
            StringAssert.Contains("Roundtrip Title", output);
            StringAssert.Contains("http://purl.org/dc/elements/1.1/", output);
            AssertSvgXmlEquivalent(input, output);
        }

        private static void AssertSvgXmlEquivalent(string expectedXml, string actualXml)
        {
            var expected = LoadNormalizedSvg(expectedXml);
            var actual = LoadNormalizedSvg(actualXml);

            var expectedMetadata = GetMetadataElement(expected);
            var actualMetadata = GetMetadataElement(actual);

            Assert.That(expectedMetadata, Is.Not.Null);
            Assert.That(actualMetadata, Is.Not.Null);
            Assert.AreEqual(expectedMetadata.OuterXml, actualMetadata.OuterXml);
        }

        private static XmlDocument LoadNormalizedSvg(string xml)
        {
            var normalized = StripXmlDeclarationAndDoctype(xml);
            var doc = new XmlDocument { PreserveWhitespace = false };
            doc.LoadXml(normalized);
            return doc;
        }

        private static XmlElement GetMetadataElement(XmlDocument document)
        {
            return document.SelectSingleNode("/*[local-name()='svg']/*[local-name()='metadata']") as XmlElement;
        }

        private static string StripXmlDeclarationAndDoctype(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var sb = new StringBuilder();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var trimmed = line.TrimStart();
                    if (trimmed.StartsWith("<?xml", StringComparison.Ordinal) ||
                        trimmed.StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    sb.AppendLine(line);
                }

                return sb.ToString().Trim();
            }
        }
    }
}
