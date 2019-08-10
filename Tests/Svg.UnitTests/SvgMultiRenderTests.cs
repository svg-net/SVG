
using NUnit.Framework;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace Svg.UnitTests
{
    //Issue #400 possible issues with multiple rendering
    [TestFixture]
    public class SvgMultiRenderTests : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("Issue_Multirender.TestFile.svg"); } }

        [Test]
        public void MultiRender_AfterDisposal_YieldsNoIssues()
        {
            var svgDocument = SvgDocument.Open(GetResourceXmlDoc(TestResource));
            using (var smallBitmap = svgDocument.Draw())
            {                
            }

            //Second rendering
            using (var smallBitmap = svgDocument.Draw())
            {                
            }

            //Also try a rendering with a different parameter set
            using (var bitmap = svgDocument.Draw(1000, 1000))
            {
            }

        }

        [Test]
        public void MultiRender_InsideOtherRender_YieldsNoIssues()
        {
            var svgDocument = SvgDocument.Open(GetResourceXmlDoc(TestResource));
            using (var smallBitmap = svgDocument.Draw())
            {
                using (var bitmap = svgDocument.Draw())
                {
                }

                //Also try with a different set of paramters
                using (var bitmap = svgDocument.Draw(1000, 1000))
                {
                }
            }
        }

    }
}
