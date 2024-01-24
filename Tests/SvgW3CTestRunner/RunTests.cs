using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace SvgW3CTestRunner
{
    enum TestState
    {
        None,
        Positive,
        Negative
    }

    sealed class TestItem
    {
        public const string XmlTag = "test";

        public const string Passing = "Passing";
        public const string Failing = "Failing";

        public readonly int Index;
        public string FileName;
        public bool IsException;
        public double Percentage;

        public TestItem(int index, string fileName)
        {
            this.Index = index;
            this.FileName = fileName;
        }

        public TestItem(int index, XmlReader reader)
        {
            this.Index = index;
            if (reader != null)
            {
                this.ReadXml(reader);
            }
        }

        public TestState GetState(TestItem refItem)
        {
            if (refItem == null)
            {
                return TestState.None;
            }
            if (refItem.IsException == this.IsException
                && refItem.Percentage == this.Percentage)
            {
                return TestState.None;
            }
            if (refItem.IsException != this.IsException)
            {
                // If the previous throws exception, then no exception is better
                return refItem.IsException ? TestState.Positive : TestState.Negative;
            }
            if (refItem.Percentage != this.Percentage)
            {
                // For the percentage difference, the lower the better
                bool better = refItem.Percentage > this.Percentage;
                return better ? TestState.Positive : TestState.Negative;
            }
            return TestState.None;
        }

        public string[] ToList(TestItem refItem = null)
        {
            string IsExceptionRef = string.Empty;
            string PercentageRef = string.Empty;
            if (refItem != null)
            {
                IsExceptionRef = refItem.IsException ? "Yes" : "No";
                PercentageRef = refItem.Percentage.ToString();
            }

            return new string[] {
                    "",
                    (Index + 1).ToString(),
                    FileName,
                    IsException ? "Yes" : "No",
                    Percentage.ToString(),
                    IsExceptionRef,
                    PercentageRef
                };
        }
        public void ReadXml(XmlReader reader)
        {
            if (reader == null || reader.NodeType != XmlNodeType.Element)
            {
                return;
            }
            if (!string.Equals(reader.Name, XmlTag, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // <test name="test.svg" exception="false" percent="0"/>
            this.FileName = reader.GetAttribute("name");
            this.IsException = XmlConvert.ToBoolean(reader.GetAttribute("exception"));
            this.Percentage = Math.Round(double.Parse(reader.GetAttribute("percent")), 2);
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
            {
                return;
            }

            // <test name="test.svg" exception="false" percent="0"/>
            writer.WriteStartElement(XmlTag);
            writer.WriteAttributeString("name", this.FileName);
            writer.WriteAttributeString("exception", this.IsException ? "true" : "false");
            writer.WriteAttributeString("percent", this.Percentage.ToString());
            writer.WriteEndElement();
        }
    }

    sealed class TestItemList : List<TestItem>
    {
        public const string XmlTag = "tests";

        private readonly string _category;

        private IDictionary<string, TestItem> _references;

        public TestItemList(string category)
        {
            _category = category;
        }

        public TestItem this[string fileName]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return null;
                }
                if (_references != null && _references.ContainsKey(fileName))
                {
                    return _references[fileName];
                }
                return null;
            }
        }

        public string Category { get => _category; }

        public IDictionary<string, TestItem> References { get => _references; set => _references = value; }

        public void ReadXml(XmlReader reader)
        {
            var comparer = StringComparison.OrdinalIgnoreCase;
            if (reader == null || reader.NodeType != XmlNodeType.Element)
            {
                return;
            }
            if (!string.Equals(reader.Name, XmlTag, comparer))
            {
                return;
            }

            // <tests category="Passing/Failing">
            //  <test name="test.svg" exception="false" percent="0"/>
            // </tests>
            string category = reader.GetAttribute("category");
            if (!string.IsNullOrWhiteSpace(category) && category.Equals(_category))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (string.Equals(reader.Name, TestItem.XmlTag, comparer))
                        {
                            var testItem = new TestItem(this.Count, reader);
                            this.Add(testItem);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (string.Equals(reader.Name, XmlTag, comparer))
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
            {
                return;
            }

            writer.WriteStartElement(XmlTag);
            writer.WriteAttributeString("category", _category);

            for (int i = 0; i < this.Count; i++)
            {
                var testItem = this[i];
                if (testItem != null)
                {
                    testItem.WriteXml(writer);
                }
            }

            writer.WriteEndElement();
        }
    }

    sealed class TestSerializer
    {
        public const string XmlTag = "testrun";

        private readonly TestItemList _passingTests;
        private readonly TestItemList _failingTests;

        private string _version;
        private DateTime _date;

        private Dictionary<string, TestItem> _passingRefs;
        private Dictionary<string, TestItem> _failingRefs;

        public TestSerializer(TestItemList passingTests, TestItemList failingTests)
        {
            _passingTests = passingTests;
            _failingTests = failingTests;

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            _date = DateTime.Now;
            _version = fvi.FileVersion;
        }

        public string Version { get => _version; }
        public DateTime Date { get => _date; }

        public void WriteXml(string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.Encoding = Encoding.UTF8;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                this.WriteXml(writer);
            }
        }

        public void ReadXml(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || File.Exists(filePath) == false)
            {
                return;
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = false;
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                this.ReadXml(reader);
            }
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader == null || reader.NodeType != XmlNodeType.Element)
            {
                return;
            }
            var comparer = StringComparison.OrdinalIgnoreCase;
            if (!string.Equals(reader.Name, XmlTag, comparer))
            {
                return;
            }

            // <tests version="" date="">
            //  <test name="test.svg" exception="false" percent="0"/>
            // </tests>
            string version = reader.GetAttribute("version");
            string date = reader.GetAttribute("date");
            if (!string.IsNullOrWhiteSpace(version) && !string.IsNullOrWhiteSpace(date))
            {
                _version = version;
                _date = XmlConvert.ToDateTime(date, XmlDateTimeSerializationMode.RoundtripKind);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (string.Equals(reader.Name, TestItemList.XmlTag, comparer))
                        {
                            string category = reader.GetAttribute("category");
                            if (category.Equals(TestItem.Passing))
                            {
                                _passingTests.ReadXml(reader);
                            }
                            else if (category.Equals(TestItem.Failing))
                            {
                                _failingTests.ReadXml(reader);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (string.Equals(reader.Name, XmlTag, comparer))
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null || _passingTests == null || _failingTests == null)
            {
                return;
            }

            writer.WriteStartElement(XmlTag);
            writer.WriteAttributeString("version", _version);
            writer.WriteAttributeString("date", XmlConvert.ToString(_date, XmlDateTimeSerializationMode.RoundtripKind));

            _passingTests.WriteXml(writer);
            _failingTests.WriteXml(writer);

            writer.WriteEndElement();
        }

        public void LoadXml(string filePath)
        {
            _passingRefs = new Dictionary<string, TestItem>(StringComparer.OrdinalIgnoreCase);
            _failingRefs = new Dictionary<string, TestItem>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(filePath) || File.Exists(filePath) == false)
            {
                return;
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = false;
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                reader.MoveToContent();
                this.LoadXml(reader);
            }

            _passingTests.References = _passingRefs;
            _failingTests.References = _failingRefs;
        }

        private void LoadXml(XmlReader reader)
        {
            if (reader == null || reader.NodeType != XmlNodeType.Element)
            {
                return;
            }
            var comparer = StringComparison.OrdinalIgnoreCase;
            if (!string.Equals(reader.Name, TestSerializer.XmlTag, comparer))
            {
                return;
            }

            // <tests version="" date="">
            //  <test name="test.svg" exception="false" percent="0"/>
            // </tests>
            string version = reader.GetAttribute("version");
            string date = reader.GetAttribute("date");
            if (!string.IsNullOrWhiteSpace(version) && !string.IsNullOrWhiteSpace(date))
            {
                _version = version;
                _date = XmlConvert.ToDateTime(date, XmlDateTimeSerializationMode.RoundtripKind);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (string.Equals(reader.Name, TestItemList.XmlTag, comparer))
                        {
                            string category = reader.GetAttribute("category");
                            if (category.Equals(TestItem.Passing))
                            {
                                this.LoadXml(reader, TestItem.Passing);
                            }
                            else if (category.Equals(TestItem.Failing))
                            {
                                this.LoadXml(reader, TestItem.Failing);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (string.Equals(reader.Name, TestSerializer.XmlTag, comparer))
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void LoadXml(XmlReader reader, string targetCategory)
        {
            var comparer = StringComparison.OrdinalIgnoreCase;
            if (reader == null || reader.NodeType != XmlNodeType.Element)
            {
                return;
            }
            if (!string.Equals(reader.Name, TestItemList.XmlTag, comparer))
            {
                return;
            }

            int itemCount = 0;

            // <tests category="Passing/Failing">
            //  <test name="test.svg" exception="false" percent="0"/>
            // </tests>
            string category = reader.GetAttribute("category");
            if (!string.IsNullOrWhiteSpace(category) && category.Equals(targetCategory))
            {
                var targetDict = category == TestItem.Passing ? _passingRefs : _failingRefs;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (string.Equals(reader.Name, TestItem.XmlTag, comparer))
                        {
                            var testItem = new TestItem(itemCount, reader);
                            targetDict.Add(testItem.FileName, testItem);

                            itemCount++;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (string.Equals(reader.Name, TestItemList.XmlTag, comparer))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
