using System.Drawing;
using System.Globalization;
using NUnit.Framework;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test class to test the Switch element with "systemLanguage"
    /// as the selector attribute.
    /// </summary>
    [TestFixture]
    public class SwitchTest : SvgTestHelper
    {
        private const string SwitchTestSvg = "Issue1176_Switch.SwitchTest.svg";

        [Test]
        [TestCase("en-US", "Red")]
        [TestCase("en-BR", "Blue")]
        [TestCase("ru-RU", "Green")]
        [TestCase("es-ES", "Green")]
        [TestCase("ja-JP", "Yellow")]
        public void TestSystemLanguageSwitch(string languageCode, string expectedColor)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(languageCode, false);
                var testDocument = OpenSvg(GetXMLDocFromResource(GetFullResourceString(SwitchTestSvg)));
                var pngName = GetFullResourceString($"Issue1176_Switch.{expectedColor}.png");
                var expectedPngStream = GetResourceStream(pngName);
                var expectedImage = new Bitmap(expectedPngStream);
                var actualImage = new Bitmap(DrawSvg(testDocument));
                Assert.That(ImagesAreEqual(expectedImage, actualImage, out _));
            }
            finally
            {
                CultureInfo.CurrentCulture = currentCulture;
            }
        }
    }
}