using NUnit.Framework;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Svg.UnitTests
{
    [TestFixture]
    public class MultiThreadingTest : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("Issue_Threading.TestFile.svg"); } }
        protected override int ExpectedSize { get { return 100; } }

        private void LoadFile()
        {
            LoadSvg(GetXMLDocFromResource());
        }

        [Test]
        public void LoadSVGThreading_SingleThread_YieldsNoError()
        {
            LoadFile();
        }

        [Test]
        public void LoadSVGThreading_MultiThread_YieldsNoErrorWhileInBounds()
        {
            Parallel.For(0, 10, (x) =>
            {
                LoadFile();
            });
            Trace.WriteLine("Done");
        }
    }
}
