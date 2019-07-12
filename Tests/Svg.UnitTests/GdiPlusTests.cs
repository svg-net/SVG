using NUnit.Framework;

namespace Svg.UnitTests
{

    /// <summary>
    /// Simple testing of the GDI+ capabilities, just to ensure whether the checks are inmplemented correctly
    /// </summary>
    [TestFixture]
    public class GdiPlusTests : SvgTestHelper
    {
        [Test]
        public void GdiPlus_QueryCapability_YieldsTrue()
        {
            Assert.True(SvgDocument.SystemIsGdiPlusCapable(), "The gdiplus check should yield true, please validate gdi+ capabilities");
        }

        [Test]
        public void GdiPlus_EnsureCapability_YieldsNoError()
        {
            SvgDocument.EnsureSystemIsGdiPlusCapable(); //This call is a void, if everything works as expected, we won't get an exception and the test will finish.
        }
    }
}
