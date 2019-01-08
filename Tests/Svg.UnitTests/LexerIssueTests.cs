using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg.UnitTests
{

	/// <summary>
	/// Some complexer stylesheets seemed to crash the lexer and result in errors (#399)
	/// Found several issues in the lexer and parser of the SVG file. 
	/// 
	/// This test class will test against these issues to prevent the bug from reoccuring
	/// </summary>
	[TestClass]
	public class LexerIssueTests : SvgTestHelper
	{
		private const string ResourceStringEmptyDTagFile = "Issue399_LexerIssue.EmptyDTag.svg";
		private const string ResourceStringStyleWithImportantAfterHex = "Issue399_LexerIssue.LexerTestImportantAfterHex.svg";
		private const string ResourceStringStyleWithImportantAfterNonHex = "Issue399_LexerIssue.LexerTestImportantAfterNonHex.svg";
		private const string ResourceStringStyleWithInvalidHex = "Issue399_LexerIssue.LexerTestFailOnInvalidHex.svg";
	
		/// <summary>
		/// We encountered issues in the example file that were caused by an empty d tag in some of the elements
		/// </summary>
		[TestMethod]
		public void Lexer_FileWithEmptyDAttribute_Success()
		{
			var xml = GetXMLDocFromResource(GetFullResourceString(ResourceStringEmptyDTagFile));
			var doc = OpenSvg(xml);
		}

		/// <summary>
		/// Stylesheet lexer fails if there is an important after a hex value, this tests the 3 and 6 lenght variants in the file
		/// </summary>
		[TestMethod]
		public void Lexer_ImportantAfterHex_Success()
		{
			var xml = GetXMLDocFromResource(GetFullResourceString(ResourceStringStyleWithImportantAfterHex));
			var doc = OpenSvg(xml);
		}

		/// <summary>
		///Reference test if there is an important after a non-hex value (should never fail) and on hex without an important
		/// </summary>
		[TestMethod]
		public void Lexer_NoImportantAndImportantAfterNonHex_Success()
		{
			var xml = GetXMLDocFromResource(GetFullResourceString(ResourceStringStyleWithImportantAfterNonHex));
			var doc = OpenSvg(xml);
		}

		[TestMethod]
		public void Lexer_FileWithInvalidHex_FailsWithError()
		{
			try
			{
				var xml = GetXMLDocFromResource(GetFullResourceString(ResourceStringStyleWithInvalidHex));
				var doc = OpenSvg(xml);
				Assert.Fail("Expected the xml to fail due to invalid HEX value");
			}
			catch (ArgumentOutOfRangeException)
			{
				//Success, we expected this
			}
		}	
	}
}
