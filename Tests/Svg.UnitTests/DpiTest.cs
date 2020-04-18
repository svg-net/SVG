using NUnit.Framework;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test that basic graphics operations work. Currently only supported
    /// on Windows, macOS, and Linux.
    /// </summary>
    [TestFixture]
    public class DpiTest
    {
        /// <summary>
        /// We should get a valid dpi (probably 72, 96 or similar).
        /// </summary>
        [Test]
        public void TestDpiAboveZero()
        {
            Assert.IsTrue(SvgDocument.PointsPerInch > 0);
        }
    }
}
