using System.Drawing;
using System.IO;
using System.Xml;
using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class StyleTest
    {
        [Test]
        public void TestStyleValue()
        {
            var document = new SvgDocument();
            var text = new SvgText("Test")
            {
                X = { 0f },
                Y = { 20f },
                Fill = new SvgColourServer(Color.Blue),
                CustomAttributes = { { "style", "test0:test0" } }
            };
            text.AddStyle("test1", "test1", 0);
            document.Children.Add(text);
            using (var stream = new MemoryStream())
            {
                document.Write(stream);
                stream.Position = 0;

                var xmlDoc = new XmlDocument
                {
                    XmlResolver = new SvgDtdResolver()
                };
                xmlDoc.Load(stream);

                var attribute = xmlDoc.DocumentElement.FirstChild.Attributes["style"];
                Assert.IsNotNull(attribute);

                var styles = attribute.InnerText.Split(';');
                Assert.Contains("test0:test0", styles);
                Assert.Contains("test1:test1", styles);
                Assert.Contains("fill:blue", styles);
            }
        }
    }
}
