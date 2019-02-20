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

        private const string ResourceStringLexerTemplate = "Issue399_LexerIssue.LexerTestTemplate.svg";
        private const string LexerTemplateReplaceToken = "/*[REPLACE]*/";


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
            Assert.Inconclusive("The important after the  hex codes is not working at the moment. Re-enable these tests if the issue is resolved (#399)");
            //Important should be valid on 3 digit hex
            GenerateLexerTestFile("border-top: 1px solid #333 !important;");
            //Important should be valid on 6 digit hex
            GenerateLexerTestFile("border-top: 1px solid #009c46 !important;");
            //Whitespace should not break the parser
            GenerateLexerTestFile("border-bottom: 1px solid #009c46  ;");
        }

        /// <summary>
        ///Reference test if there is an important after a non-hex value (should never fail) and on hex without an important
        /// </summary>
        [TestMethod]
        public void Lexer_NoImportantAndImportantAfterNonHex_Success()
        {
            //Hex is working, if no !important suffixing it (#399)
            GenerateLexerTestFile("border-top: 1px solid #009c46;");
            //Important is not failing on non-hex value
            GenerateLexerTestFile("border-top: 1px solid red !important;");
        }

        [TestMethod]
        public void Lexer_FileWithInvalidHex_ColorTagIsIgnored()
        {
            Assert.Inconclusive("This test fails due to a lexer error");
            GenerateLexerTestFile("color: #0046;");
        }

        /// <summary>
        /// Load the lexer template and replace the token with a test value and try generating the svg
        /// </summary>
        /// <param name="testContent">Content to test in the template</param>
        /// <returns></returns>
        private SvgDocument GenerateLexerTestFile(string testContent)
        {
            var str = GetResourceXmlDocAsString(GetFullResourceString(ResourceStringLexerTemplate));
            str = str.Replace(LexerTemplateReplaceToken, testContent);
            var xml = GetXMLDocFromString(str);
            var doc = OpenSvg(xml);
            return doc;
        }

    }
}
