using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgNodeReaderTests : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("Issue518_Entities.Entities.svg"); } }
        //protected override int ExpectedSize { get { return 4300; } } // original image has 4314 bytes

        [Test]
        public void ReadingEntitiesFromXmlSucceeds()
        {
            var xmlDoc = GetXMLDocFromResource();
            var doc = SvgDocument.Open(xmlDoc);
            Assert.DoesNotThrow(() => doc.Draw());
        }
    }
}
