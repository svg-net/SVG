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
		private const string ResourceStringStyleWithImportantInBorderFile = "Issue399_LexerIssue.LexerFailOnImportantInBorder.svg";
		private const string ResourceStringStyleWithoutImportantInBorderFile = "Issue399_LexerIssue.LexerSuccesOnNoImportantInBorder.svg";
		
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
		/// Stylesheet lexer fails if there is an important in the border styling
		/// </summary>
		[TestMethod]
		public void Lexer_ImportantInBorderStyle_Success()
		{
			var xml = GetXMLDocFromResource(GetFullResourceString(ResourceStringStyleWithImportantInBorderFile));
			var doc = OpenSvg(xml);
		}

		/// <summary>
		///Reference test for border style without important (should never fail)
		/// </summary>
		[TestMethod]
		public void Lexer_NoImportantInBorderStyle_Success()
		{
			var xml = GetXMLDocFromResource(GetFullResourceString(ResourceStringStyleWithoutImportantInBorderFile));
			var doc = OpenSvg(xml);
		}
	}
}
