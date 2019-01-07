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
		private const string ResourceStringComplexFile = "Issue399_LexerIssue.ComplexStyle.svg";
		
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
		/// Test if system is able to handle the complex svg case in the testcases
		/// </summary>
		[TestMethod]
		public void Lexer_ComplexStyling_Success()
		{
			var xml = GetXMLDocFromResource(GetFullResourceString(ResourceStringComplexFile));
			var doc = OpenSvg(xml);
		}
	

	}
}
