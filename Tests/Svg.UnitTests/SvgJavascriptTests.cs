using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace Svg.UnitTests
{
    /// <summary>
    /// Testing the capabilities to handle SVG documents with Javascript and the option to add
    /// scripts to the SVG document.
    /// </summary>
    [TestFixture]
    public class SvgJavascriptTests : SvgTestHelper
    {
        protected override string TestResource { get { return GetFullResourceString("ScriptTag.TestFile.svg"); } } 

        [Test]
        public void SvgDocument_ReadFileWithScript_YieldsNoError()
        {
            //Expect to read a document with JS without problem
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);
            var circleElement = doc.Children.FirstOrDefault(c => c.ElementName == "circle");
            Assert.IsNotNull(circleElement, "Expected to find the circle element in the document");
            Assert.IsAssignableFrom(typeof(SvgCircle), circleElement, "Expected the found circle element to be of type SvgCircle");
            
            var scriptElement = doc.Children.FirstOrDefault(c => c.ElementName == "script");
            Assert.IsNotNull(scriptElement, "Expected to find the script element in the document");
            Assert.IsAssignableFrom(typeof(SvgScript), scriptElement, "Expected the found script element to be of type SvgScript");            
        }

        [Test]
        public void SvgDocument_FileWithScript_CanRetrieveScriptTag()
        {
            //Expected to retrieve the script tag(s) from the document
            Assert.Inconclusive();
        }

        [Test]
        public void SvgDocument_AddScript_AddScriptWithEscapingToSvg()
        {
            //Script is expected to be escaped
            Assert.Inconclusive();
        }

        [Test]
        public void SvgDocument_AlterScriptTag_HandlesEscapingCorrectly()
        {
            //Expecting to get the script without CDATA
            //After update expect the updated script with CDATA in the results
            Assert.Inconclusive();
        }
    }
}
