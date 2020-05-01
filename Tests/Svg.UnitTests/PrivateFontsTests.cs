using NUnit.Framework;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test Class of the feature to use Private Fonts in SVGs.
    /// Based on Issue 204.
    /// </summary>
    /// <remarks>
    /// Test uses the following embedded resources:
    ///   - Issue204_PrivateFont\Text.svg
    ///   - Issue204_PrivateFont\BrushScriptMT2.ttf
    /// </remarks>
    [TestFixture]
    public class PrivateFontsTests : SvgTestHelper
    {
        private const string PrivateFontSvg = "Issue204_PrivateFont.Text.svg";
        private const string PrivateFont = "Issue204_PrivateFont.BrushScriptMT2.ttf";

#if NETSTANDARD20
        // Private font does not work if .NET Standard.
        protected override int ExpectedSize { get { return 3000; } } // 3155
#else
        protected override int ExpectedSize { get { return 3200; } } // 3512
#endif

        [Test]
        public void TestPrivateFont()
        {
            var fontBytes = GetResourceBytes(GetFullResourceString(PrivateFont));
            SvgFontManager.PrivateFontDataList.Add(fontBytes);
            LoadSvg(GetXMLDocFromResource(GetFullResourceString(PrivateFontSvg)));
        }
    }
}
