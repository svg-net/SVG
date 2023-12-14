using System.IO;

using NUnit.Framework;

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
    }
}
