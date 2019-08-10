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
        /// Retrieve the script content, this is not expected to be escaped
        /// </summary>
        [Test]
        public void SvgDocumentWithSvgScript_RetrieveScriptContent_ScriptContentIsNotEscaped()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Try to add a script to a document and check if the content was added correctly
        /// and escaped as expected.
        /// </summary>
        [Test]
        public void SvgDocumentWithSvgScript_AddScript_EscapintIsAdded()
        {
            //Script is expected to be escaped
            Assert.Inconclusive();
        }

        /// <summary>
        /// Test if altering a script (reading, changing and reinserting) does not yield odd results.
        /// </summary>
        [Test]
        public void SvgDocumentWithSvgScript_AlterScriptTag_HandlesEscapingCorrectly()
        {
            //Expecting to get the script without CDATA
            //After update expect the updated script with CDATA in the results
            Assert.Inconclusive();
        }
    }
}
