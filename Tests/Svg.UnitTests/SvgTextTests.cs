using NUnit.Framework;
using System.IO;
using System.Xml;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgTextTests
    {
        [Test]
        public void TextPropertyAffectsSvgOutput()
        {
            var document = new SvgDocument();
            document.Children.Add(new SvgText { Text = "test1" });
            using (var stream = new MemoryStream())
            {
                document.Write(stream);
                stream.Position = 0;

                var xmlDoc = new XmlDocument();
                xmlDoc.XmlResolver = new SvgDtdResolver();
                xmlDoc.Load(stream);
                Assert.AreEqual("test1", xmlDoc.DocumentElement.FirstChild.InnerText);
            }
        }

        /// <summary>
        /// Test related to bug #473.
        /// We check if changing a coordinate invalidate the cache.
        /// We doing this indirectly by checking the Bound property, which uses the cache.
        /// The Bound coordinates must be updated after adding a X and a Y
        /// </summary>
        [Test]
        public void ChangingCoordinatesInvalidatePathCache()
        {
            SvgText text = new SvgText();
            text.Text = "Test invalidate cache";
            float origX = text.Bounds.X;
            float origY = text.Bounds.Y;
            text.X.Add(100);
            text.Y.Add(100);

            Assert.AreNotEqual(origX, text.Bounds.X);
            Assert.AreNotEqual(origY, text.Bounds.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestWritesCoordinatesForCollectionSet()
        {
            SvgText text = new SvgText();
            text.Text = "Test coordinates";
            text.X = new SvgUnitCollection { 20 };
            text.Y = new SvgUnitCollection { 30 };
            text.Dx = new SvgUnitCollection { 40 };
            text.Dy = new SvgUnitCollection { 50 };

            var xml = text.GetXML();
            Assert.IsTrue(xml.Contains("x=\"20\""));
            Assert.IsTrue(xml.Contains("y=\"30\""));
            Assert.IsTrue(xml.Contains("dx=\"40\""));
            Assert.IsTrue(xml.Contains("dy=\"50\""));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestWritesCoordinatesForCollectionChange()
        {
            SvgText text = new SvgText();
            text.Text = "Test coordinates";
            text.X.Add(20);
            text.Y.Add(30);
            text.Dx.Add(40);
            text.Dy.Add(50);

            var xml = text.GetXML();
            Assert.IsTrue(xml.Contains("x=\"20\""));
            Assert.IsTrue(xml.Contains("y=\"30\""));
            Assert.IsTrue(xml.Contains("dx=\"40\""));
            Assert.IsTrue(xml.Contains("dy=\"50\""));
        }
    }
}
