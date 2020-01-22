using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgNumberCollectionTests
    {
        [Test]
        public void ToStringReturnsValidString()
        {
            var collection = new SvgNumberCollection
            {
                1.6f, 3.2f,
                1.2f, 5f
            };
            Assert.AreEqual("1.6 3.2 1.2 5", collection.ToString());
        }
    }
}
