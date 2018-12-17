﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace Svg.UnitTests
{

    /// <summary>
    /// Test Class of the feature to use Private Fonts in SVGs.
    /// Based on Issue 204.
    /// </summary>
    /// <remarks>
    /// Test use the following embedded resources:
    ///   - Issue204_PrivateFont\Text.svg
    ///   - Issue204_PrivateFont\BrushScriptMT2.ttf
    /// </remarks>
    [TestClass]
    public class PrivateFontsTests : SvgTestHelper
    {
        private const string PrivateFontSvg = "Issue204_PrivateFont.Text.svg";
        private const string PrivateFont = "Issue204_PrivateFont.BrushScriptMT2.ttf";
        //private const string PrivateFontName = "Brush Script MT2";

        protected override int ExpectedSize { get { return 3200; } } //3512


        [TestMethod]
        [Ignore]
        public void TestPrivateFont()
        {
            // FIXME: Currently throws because of zero size 
            AddFontFromResource(SvgElement.PrivateFonts, GetFullResourceString(PrivateFont));
            LoadSvg(GetXMLDocFromResource(GetFullResourceString(PrivateFontSvg)));
        }


        private void AddFontFromResource(PrivateFontCollection privateFontCollection, string fullFontResourceString)
        {
            var fontBytes = GetResourceBytes(fullFontResourceString);
            var fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            privateFontCollection.AddMemoryFont(fontData, fontBytes.Length); // Add font to collection.
            Marshal.FreeCoTaskMem(fontData); // Do not forget to frees the memory block.
        }
    }
}
