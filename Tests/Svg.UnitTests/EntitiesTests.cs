using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test of open SvgDocument with entities.
    /// </summary>
    [TestFixture]
    public class EntitiesTests : SvgTestHelper
    {
        private static string _testsRootPath;

        private const string EntitiesSampleSvgPath = @"..\Samples\Entities\sample.svg";

        [Test]
        public void TestOpenWithEntities()
        {
            var entities = new Dictionary<string, string>
            {
                { "entity1", "fill:red" },
                { "entity2", "fill:yellow" },
            };

            var svgPath = Path.Combine(TestsRootPath, EntitiesSampleSvgPath);
            var doc = SvgDocument.Open<SvgDocument>(svgPath, entities);
            Assert.That(doc.Children[0].Children[0].Fill, Is.EqualTo(new SvgColourServer(Color.Red)));
            Assert.That(doc.Children[0].Children[1].Fill, Is.EqualTo(new SvgColourServer(Color.Yellow)));
        }

        [Test]
        public void TestOpenWithoutEntities()
        {
            var svgPath = Path.Combine(TestsRootPath, EntitiesSampleSvgPath);
            Assert.That(() => { SvgDocument.Open<SvgDocument>(svgPath, null); },
                Throws.TypeOf<System.Xml.XmlException>().With.Message.Contains("entity"));
        }

        private static string TestsRootPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_testsRootPath))
                    return _testsRootPath;

                var path = TestContext.CurrentContext.TestDirectory;
                while (!Path.GetFileName(path).Equals("Tests"))
                    path = Path.GetDirectoryName(path);

                _testsRootPath = path;
                return _testsRootPath;
            }
        }
    }
}
