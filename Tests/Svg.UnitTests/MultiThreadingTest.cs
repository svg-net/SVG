using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			bool valid = true;
			Parallel.For(0, 3, (x) =>
			{
				LoadFile();
			});
			Assert.IsTrue(valid, "One or more of the runs was invalid");
			Trace.WriteLine("Done");
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