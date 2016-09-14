using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Svg.UnitTests {

    [TestClass]
    public class SvgPointCollectionTests 
    {

        [TestMethod]
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
