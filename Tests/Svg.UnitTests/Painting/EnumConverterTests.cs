using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svg.FilterEffects;

namespace Svg.UnitTests.Painting
{
    [TestClass]
    public class EnumConverterTests : SvgTestHelper
    {
        private static readonly EnumBaseConverter<SvgBlendMode> DashedConverter = new EnumBaseConverter<SvgBlendMode>(SvgBlendMode.Normal, EnumBaseConverter<SvgBlendMode>.CaseHandling.DashedLowerCase);
        private static readonly EnumBaseConverter<SvgBlendMode> LowerCaseConverter = new EnumBaseConverter<SvgBlendMode>(SvgBlendMode.Normal, EnumBaseConverter<SvgBlendMode>.CaseHandling.LowerCase);
        private static readonly EnumBaseConverter<SvgBlendMode> CamelCaseConverter = new EnumBaseConverter<SvgBlendMode>(SvgBlendMode.Normal, EnumBaseConverter<SvgBlendMode>.CaseHandling.CamelCase);

        [TestMethod]
        public void DashedToStringWorksForSingleWord()
        {
            var value = SvgBlendMode.Color;
            var str = DashedConverter.ConvertToString(value);
            Assert.AreEqual("color", str);
        }

        [TestMethod]
        public void DashedToStringWorksForMultipleWords()
        {
            var value = SvgBlendMode.SoftLight;
            var str = DashedConverter.ConvertToString(value);
            Assert.AreEqual("soft-light", str);
        }

        [TestMethod]
        public void LowerToStringWorksForSingleWord()
        {
            var value = SvgBlendMode.Color;
            var str = LowerCaseConverter.ConvertToString(value);
            Assert.AreEqual("color", str);
        }

        [TestMethod]
        public void LowerToStringWorksForMultipleWords()
        {
            var value = SvgBlendMode.SoftLight;
            var str = LowerCaseConverter.ConvertToString(value);
            Assert.AreEqual("softlight", str);
        }

        [TestMethod]
        public void CamelToStringWorksForSingleWord()
        {
            var value = SvgBlendMode.Color;
            var str = CamelCaseConverter.ConvertToString(value);
            Assert.AreEqual("color", str);
        }

        [TestMethod]
        public void CamelToStringWorksForMultipleWords()
        {
            var value = SvgBlendMode.SoftLight;
            var str = CamelCaseConverter.ConvertToString(value);
            Assert.AreEqual("softLight", str);
        }
    }
}
