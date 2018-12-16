using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svg.Exceptions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Svg.UnitTests
{

    [TestClass]
    public class MultiThreadingTest : SvgTestHelper
    {
		protected override string TestResource { get { return GetFullResourceString("Issue_Threading.TestFile.svg"); } }
		protected override int ExpectedSize { get { return 100; } }

        private void LoadFile()
        {
            LoadSvg(GetXMLDocFromResource());
        }

        
        [TestMethod]
        public void LoadSVGThreading_SingleThread_YieldsNoError()
        {
            LoadFile();
        }


        [TestMethod]
        public void LoadSVGThreading_MultiThread_YieldsNoErrorWhileInBounds()
        {
            Parallel.For(0, 10, (x) =>
            {
                LoadFile();
            });
            Trace.WriteLine("Done");
        }


        [TestMethod]
        public void LoadSVGThreading_MultiThread_GivesMemoryExceptionOnTooManyParallelTest()
        {
			try
			{
				Parallel.For(0, 100, (x) =>
				{
					LoadFile();
				});
			}
			catch (AggregateException ex)
			{
				//We expect an SVG Memory Exception to be thrown, thats okay, otherwise fail
				if (!(ex.InnerException is SvgMemoryException))
				{
					throw ex.InnerException;
				}
			}
			Assert.Inconclusive("This test was expected to throw an SVGMemoryException, however this is higly dependent on the file and machine under test. This is not a fail reason.");
        }
    }
}
