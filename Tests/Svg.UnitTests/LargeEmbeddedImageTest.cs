using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svg.DataTypes;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

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
    [TestClass]
    public class LargeEmbeddedImageTest : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("Issue225_LargeUri.Speedometer.svg"); } }
        protected override int ExpectedSize { get { return 160000; } } 

        [TestMethod]
        public void TestImageIsRendered()
        {
            LoadSvg(GetXMLDocFromResource());
        }
    }
}
