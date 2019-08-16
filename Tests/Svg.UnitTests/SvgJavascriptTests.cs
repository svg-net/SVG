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

        /// <summary>
        /// Helper to retrieve the script element from the test document (expected in the root as type SvgScript with the elementname script)
        /// Will assert the element not being null and of the correct type
        /// </summary>
        private SvgScript GetScriptElementFromDocument(SvgDocument doc)
        {
            var el = doc.Children.FirstOrDefault(c => c.ElementName == "script");
            Assert.IsNotNull(el, "Could not find the script element");
            Assert.IsAssignableFrom(typeof(SvgScript), el, "Script tag was not correctly typed, expected SvgScript as type");
            return el as SvgScript;
        }

        /// <summary>
        /// Check if a file with a script tag can correctly be read (and the types are as expected)
        /// </summary>
        [Test]
        public void SvgDocumentWithSvgScript_ReadFile_ScriptTagReadFromSvgInCorrectType()
        {
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);
            
            var circleElement = doc.Children.FirstOrDefault(c => c.ElementName == "circle");
            Assert.IsNotNull(circleElement, "Expected to find the circle element in the document");
            Assert.IsAssignableFrom(typeof(SvgCircle), circleElement, "Expected the found circle element to be of type SvgCircle");
            
            //Check if the type is correctly parsed
            var scriptElement = GetScriptElementFromDocument(doc);
            Assert.IsNotNull(scriptElement, "Expected to find the script element in the document");
        }

        /// <summary>
        /// Test whether the attributes are parsed correctly (for now we only test the scripttype)
        /// </summary>
        [Test]
        public void SvgDocumentWithSvgScript_AttributeParsing_CorrectTypeAttribute()
        {
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);
            
            var tag = GetScriptElementFromDocument(doc);
            Assert.AreEqual("text/javascript", tag.ScriptType, "Expected the type attribute to be read");
        }

        /// <summary>
        /// Retrieve the script content, this is not expected to be escaped. CDATA is handled by the SVG parser and should result in a "clean" script.
        /// </summary>
        [Test]
        public void SvgDocumentWithSvgScript_RetrieveScriptContent_ScriptContentIsNotEscaped()
        {
            var doc = SvgDocument.Open(GetXMLDocFromResource(TestResource));
            Assert.IsNotNull(doc);
            
            var tag = GetScriptElementFromDocument(doc);
            Assert.IsTrue(tag.Script.IndexOf("[DATA") < 0, "CDATA should not be in the content of the script");
        }

        /// <summary>
        /// Try to add a script to a document and check if the content was added correctly
        /// and escaped as expected.
        /// </summary>
        [Test]
        public void SvgDocumentWithSvgScript_AddScript_EscapeAndTypeAdded()
        {
            //Script is expected to be escaped
            var doc = new SvgDocument();
            var script = new SvgScript();
            script.Script = "alert('test');";
            script.ScriptType = "text/javascript";
            doc.Children.Add(script);
            var docStr = doc.GetXML();

            //Check if we found the type string and CDATA tags
            Assert.IsFalse(string.IsNullOrWhiteSpace(docStr), "Expected document to be returned");
            Assert.IsTrue(docStr.IndexOf("<script type=\"text/javascript\">") > -1, "Expected to find the script with type tag");
            Assert.IsTrue(docStr.IndexOf("<![CDATA[") > -1, "Expected to find the CDATA start");
            Assert.IsTrue(docStr.IndexOf("]]>") > -1, "Expected to find the CDATA ending");
        }
    }
}
