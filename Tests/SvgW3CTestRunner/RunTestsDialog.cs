using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Svg;

namespace SvgW3CTestRunner
{
    public partial class RunTestsDialog : Form
    {
        private const string IssuesPrefix = "__";

        private sealed class TestItem
        {
            public int number;
            public string fileName;
            public bool exception;
            public double percentage;

            public TestItem(int number, string fileName)
            {
                this.number = number;
                this.fileName = fileName;
            }

            public string[] ToList()
            {
                return new string[] {
                    number.ToString(),
                    fileName,
                    exception ? "Yes" : "No",
                    percentage.ToString()
                };
            }
        }


        private int _seletedTabIndex;
        private List<string[]> _listItems;

        private List<TestItem> _testItems;

        //Data folders
        private string _svgW3CBasePath;
        private string _pngW3CBasePath;

        //Data folders
        private string _svgIssuesBasePath;
        private string _pngIssuesBasePath;

        private ListViewGroup testGroup;
        private ListViewGroup exceptionGroup;

        public RunTestsDialog()
        {
            InitializeComponent();

            // Add some groups to the ListView.
            testGroup = new ListViewGroup("Test Results");
            exceptionGroup = new ListViewGroup("Exception Results");
            listView.Groups.Add(testGroup);
            listView.Groups.Add(exceptionGroup);

            columnNumber.TextAlign = HorizontalAlignment.Right;
        }

        public int SeletedTabIndex { get => _seletedTabIndex; set => _seletedTabIndex = value; }
        public List<string[]> ListItems { get => _listItems; set => _listItems = value; }

        public string SvgW3CBasePath { get => _svgW3CBasePath; set => _svgW3CBasePath = value; }
        public string PngW3CBasePath { get => _pngW3CBasePath; set => _pngW3CBasePath = value; }
        public string SvgIssuesBasePath { get => _svgIssuesBasePath; set => _svgIssuesBasePath = value; }
        public string PngIssuesBasePath { get => _pngIssuesBasePath; set => _pngIssuesBasePath = value; }

        private void OnLoadDialog(object sender, EventArgs e)
        {
            if (_listItems != null && _listItems.Count == 4)
            {
                comboBoxSelectTab.Items.Add("Pass W3C");
                comboBoxSelectTab.Items.Add("Fail W3C");
                comboBoxSelectTab.Items.Add("Pass Other");
                comboBoxSelectTab.Items.Add("Fail Other");

                comboBoxSelectTab.SelectedIndex = _seletedTabIndex;
                buttonRun.Enabled = true;
            }
        }

        private void OnShownDialog(object sender, EventArgs e)
        {
            richTextBox.Select();
        }

        private void OnClickRun(object sender, EventArgs e)
        {
            if (_testItems == null || _testItems.Count != 0)
            {
                _testItems = new List<TestItem>();
            }
            if (_listItems == null || _listItems.Count != 4)
            {
                return;
            }

            richTextBox.Clear();
            listView.Items.Clear();

            _seletedTabIndex = comboBoxSelectTab.SelectedIndex;

            this.Cursor = Cursors.WaitCursor;
            string fileName = string.Empty;
            try
            {
                var selectedItems = _listItems[_seletedTabIndex];
                for (int index = 0; index < selectedItems.Length; index++)
                {
                    fileName = selectedItems[index];
                    RunTest(fileName);
                }
            }
            catch (Exception ex)
            {
                if (richTextBox.TextLength != 0)
                {
                    richTextBox.AppendText(Environment.NewLine);
                }
                richTextBox.AppendText($"Exception: {fileName}" + Environment.NewLine);
                richTextBox.AppendText(ex.ToString());
                richTextBox.AppendText(Environment.NewLine);
            }
            finally
            {
                foreach (var testItem in _testItems)
                {
                    ListViewItem viewItem = listView.Items.Add(new ListViewItem(testItem.ToList(), testGroup));
                    viewItem.Tag = fileName;

                    if (testItem.exception)
                    {
                        viewItem.BackColor = Color.Crimson;
                        listView.Items.Add(new ListViewItem(testItem.ToList(), exceptionGroup));
                    }
                }

                this.Cursor = Cursors.Default;
            }
        }

        private void RunTest(string fileName)
        {
#if NET5_0_OR_GREATER
            if (!OperatingSystem.IsWindows())
                return;
#endif

            TestItem testItem = new TestItem(_testItems.Count, fileName);
            _testItems.Add(testItem);

            var isIssue = fileName.StartsWith(IssuesPrefix);

            var pngBasePath = isIssue ? _pngIssuesBasePath : _pngW3CBasePath;
            var svgBasePath = isIssue ? _svgIssuesBasePath : _svgW3CBasePath;

            Image pngImage = null;
            Image svgImage = null;
            try
            {
                pngImage = Image.FromFile(Path.Combine(pngBasePath, Path.ChangeExtension(fileName, "png")));

                var doc = new SvgDocument();
                doc = SvgDocument.Open(Path.Combine(svgBasePath, fileName));
                if (isIssue)
                {
                    svgImage = doc.Draw();
                    // Check for a large difference in image size, if not nearly equal recreate it
                    if (Math.Abs(svgImage.Width - pngImage.Width) > 10 || Math.Abs(svgImage.Height - pngImage.Height) > 10)
                    {
                        svgImage.Dispose();
                        svgImage = new Bitmap(pngImage.Width, pngImage.Height);
                        doc.Draw((Bitmap)svgImage);
                    }
                }
                else
                {
                    svgImage = new Bitmap(480, 360);
                    doc.Draw((Bitmap)svgImage);
                }

                var difference = svgImage.PercentageDifference(pngImage);
                var percentage = Math.Round(difference * 100.0, 2);

                testItem.percentage = percentage;
            }
            catch (Exception ex)
            {
                testItem.exception = true;

                if (richTextBox.TextLength != 0)
                {
                    richTextBox.AppendText(Environment.NewLine);
                }
                richTextBox.AppendText($"Exception: {fileName}" + Environment.NewLine);
                richTextBox.AppendText(ex.ToString());
                richTextBox.AppendText(Environment.NewLine);
            }
            finally
            {
                pngImage?.Dispose();
                svgImage?.Dispose();

                testItem.number = _testItems.Count;
            }
        }
    }
}
