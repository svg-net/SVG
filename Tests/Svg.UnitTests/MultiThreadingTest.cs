using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svg.Exceptions;

namespace Svg.UnitTests
{

	[TestClass()]
	public class MultiThreadingTest
	{

		private const string TestFile = @"d:\temp\test.svg";
		private const int ExpectedSize = 600000;
		private XmlDocument GetXMLDoc()
		{
			var xmlDoc = new XmlDocument();
			if(!System.IO.File.Exists(TestFile)) { Assert.Inconclusive("Test file missing"); }
			xmlDoc.LoadXml(System.IO.File.ReadAllText(TestFile));
			return xmlDoc;
		}

		[TestMethod]
		public void TestSingleThread()
		{
			LoadFile();
		}

		[TestMethod]
		public void TestMultiThread()
		{
			Parallel.For(0, 10, (x) =>
			{
				LoadFile();
			});			
			Trace.WriteLine("Done");
		}

		[TestMethod]
		[ExpectedException(typeof(SvgMemoryException))]
		public void SVGGivesMemoryExceptionOnTooManyParallelTest()
		{
			try
			{
				Parallel.For(0, 50, (x) =>
				{
					LoadFile();
				});
			}
			catch (AggregateException ex)
			{
				throw ex.InnerException;
			}
		}
		private void LoadFile()
		{
			var xml = GetXMLDoc();
			Trace.WriteLine("Reading and drawing file");
			SvgDocument d = SvgDocument.Open(xml);
			var b = d.Draw();
			Trace.WriteLine("Done reading file");
			using (var ms = new MemoryStream())
			{
				b.Save(ms, ImageFormat.Png);
				ms.Flush();
				Assert.IsTrue(ms.Length >= ExpectedSize, "File does not match expected minimum size");
			}
		}
	}
}