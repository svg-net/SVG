using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgPointCollectionTests
    {
        [Test]
        public void ToStringReturnsValidString()
        {
            var collection = new SvgPointCollection
            {
                new SvgUnit(1.6f), new SvgUnit(3.2f),
                new SvgUnit(1.2f), new SvgUnit(5f)
            };
            Assert.AreEqual("1.6,3.2 1.2,5", collection.ToString());
        }
    }
}
