using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Svg.UnitTests
{
  /// <summary>
  ///   Test Class of rendering SVGs with a large embedded image
  ///   Based on Issue 225
  /// </summary>
  /// <remarks>
  ///   Test use the following embedded resources:
  ///   - Issue225_LargeUri\Speedometer.svg
  /// </remarks>
  [TestClass]
  public class SmallEmbeddingImageTest : SvgTestHelper
  {
    protected override string TestResource
    {
      get
      {
        return this.GetFullResourceString("hotfix_image_data_uri.Speedometer.svg");
      }
    }

    protected override int ExpectedSize
    {
      get
      {
        return 160000;
      }
    }

    [TestMethod]
    public void TestImageIsRendered()
    {
      this.LoadSvg(this.GetXMLDocFromResource());
    }
  }
}