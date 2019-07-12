using NUnit.Framework;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test Class of rendering SVGs with a large embedded image
    /// Based on Issue 225
    /// </summary>
    /// <remarks>
    /// Test use the following embedded resources:
    ///   - Issue225_LargeUri\Speedometer.svg
    /// </remarks>
    [TestFixture]
    public class LargeEmbeddedImageTest : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("Issue225_LargeUri.Speedometer.svg"); } }
        protected override int ExpectedSize { get { return 160000; } }

        [Test]
        public void TestImageIsRendered()
        {
            LoadSvg(GetXMLDocFromResource());
        }
    }
}
