using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Svg;
using Svg.Tests.Common;

namespace SvgW3CTestRunner
{
    public partial class RunTestsDialog : Form
    {
        private const string IssuesPrefix = "__";
        private const string ExportPrefix = "RunTest-";
        private const string ExportName   = "{0}{1}.xml";
        private const string ExportSuffix = "3.4.6"; // Current Version

        public class ViewEventArgs : EventArgs
        {
            public ViewEventArgs(int tabIndex, int listIndex)
            {
                SeletedTabIndex = tabIndex;
                SelectedListIndex = listIndex;
            }

            public int SeletedTabIndex { get; }
            public int SelectedListIndex { get; }
        }

        // Declare the delegate.
        public delegate void ViewEventHandler(object sender, ViewEventArgs e);

        // Declare the event.
        public event ViewEventHandler ViewEvent;

        private int _seletedTabIndex;
        private int _selectedListIndex;
        private List<string[]> _listItems;
        private List<TestItemList> _testsItems;

        // W3C Data folders
        private string _svgW3CBasePath;
        private string _pngW3CBasePath;

        // Issues Data folders
        private string _svgIssuesBasePath;
        private string _pngIssuesBasePath;

        private ListViewGroup testGroup;
        private ListViewGroup exceptGroup;

        private ListViewGroup positiveGroup;
        private ListViewGroup negativeGroup;

        private ListViewGroup revisitGroup;

        public RunTestsDialog()
        {
            InitializeComponent();

            // Add some groups to the ListView.
            testGroup = new ListViewGroup("Test Results");
            exceptGroup = new ListViewGroup("Exception Results");

            positiveGroup = new ListViewGroup("Positive Results");
            negativeGroup = new ListViewGroup("Negative Results");

            revisitGroup = new ListViewGroup("Revisit Results");

            listView.Groups.Add(testGroup);
            listView.Groups.Add(exceptGroup);
            listView.Groups.Add(positiveGroup);
            listView.Groups.Add(negativeGroup);
            listView.Groups.Add(revisitGroup);

            columnNumber.TextAlign = HorizontalAlignment.Right;
        }

        public int SeletedTabIndex { get => _seletedTabIndex; set => _seletedTabIndex = value; }

        public int SelectedListIndex { get => _selectedListIndex; set => _selectedListIndex = value; }

        public List<string[]> ListItems { get => _listItems; set => _listItems = value; }

        public string SvgW3CBasePath { get => _svgW3CBasePath; set => _svgW3CBasePath = value; }
        public string PngW3CBasePath { get => _pngW3CBasePath; set => _pngW3CBasePath = value; }
        public string SvgIssuesBasePath { get => _svgIssuesBasePath; set => _svgIssuesBasePath = value; }
        public string PngIssuesBasePath { get => _pngIssuesBasePath; set => _pngIssuesBasePath = value; }

        private void OnLoadDialog(object sender, EventArgs e)
        {
            _selectedListIndex = -1;

            if (_listItems != null && _listItems.Count == 4)
            {
                comboBoxSelectTab.Items.Add("Pass W3C");
                comboBoxSelectTab.Items.Add("Fail W3C");
                comboBoxSelectTab.Items.Add("Pass Other");
                comboBoxSelectTab.Items.Add("Fail Other");

                comboBoxSelectTab.SelectedIndex = _seletedTabIndex;
                buttonRun.Enabled = true;

                if (_testsItems == null || _testsItems.Count != 4)
                {
                    _testsItems = new List<TestItemList>
                    {
                        new TestItemList(TestItem.Passing),
                        new TestItemList(TestItem.Failing),
                        new TestItemList(TestItem.Passing),
                        new TestItemList(TestItem.Failing)
                    };
                }
            }

            textBoxExport.Text = ExportSuffix;

            var exportPrefix = textBoxExport.Text.Trim();
            string exportFile = string.Format(ExportName, ExportPrefix, exportPrefix);

            var W3CBasePath = Path.GetDirectoryName(_svgW3CBasePath);
            var IssuesBasePath = Path.GetDirectoryName(_svgIssuesBasePath);

            var w3cExportPath = Path.Combine(W3CBasePath, exportFile);
            var issuesExportPath = Path.Combine(IssuesBasePath, exportFile);
            if (File.Exists(w3cExportPath) && File.Exists(issuesExportPath))
            {
                var w3cLoader = new TestSerializer(_testsItems[0], _testsItems[1]);
                w3cLoader.LoadXml(w3cExportPath);

                var issuesLoader = new TestSerializer(_testsItems[2], _testsItems[3]);
                issuesLoader.LoadXml(issuesExportPath);
            }
        }

        private void OnShownDialog(object sender, EventArgs e)
        {
            this.ClearListSelection(false);
        }

        private void OnSelectedTabChanged(object sender, EventArgs e)
        {
            this.ClearListSelection(true);

            if (_testsItems == null || _testsItems.Count != 4)
            {
                return;
            }

            _seletedTabIndex = comboBoxSelectTab.SelectedIndex;
            if (_seletedTabIndex < 0 || _seletedTabIndex > 3)
            {
                return;
            }

            TestItemList testItems = _testsItems[_seletedTabIndex];
            if (testItems.Count != 0)
            {
                this.FillListView(testItems);
                return;
            }
        }

        private void OnClickRun(object sender, EventArgs e)
        {
            this.ClearListSelection(true);

            if (_listItems == null || _listItems.Count != 4)
            {
                return;
            }
            _seletedTabIndex = comboBoxSelectTab.SelectedIndex;
            if (_seletedTabIndex < 0 || _seletedTabIndex > 3)
            {
                return;
            }

            TestItemList testItems = _testsItems[_seletedTabIndex];
            if (testItems.Count != 0)
            {
                testItems.Clear();
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                RunTests(testItems, _seletedTabIndex);
            }
            finally
            {
                this.FillListView(testItems);
                this.Cursor = Cursors.Default;
            }
        }

        private void OnClickExport(object sender, EventArgs e)
        {
            if (_testsItems == null || _testsItems.Count != 4)
            {
                return;
            }
            if (_listItems == null || _listItems.Count != 4)
            {
                return;
            }
            var exportPrefix = textBoxExport.Text.Trim();
            string exportFile = string.Format(ExportName, ExportPrefix, exportPrefix);

            var W3CBasePath = Path.GetDirectoryName(_svgW3CBasePath);
            var IssuesBasePath = Path.GetDirectoryName(_svgIssuesBasePath);
            var w3cExportPath = Path.Combine(W3CBasePath, exportFile);
            var issuesExportPath = Path.Combine(IssuesBasePath, exportFile);
            if (File.Exists(w3cExportPath) || File.Exists(issuesExportPath))
            {
                if (MessageBox.Show(this, "The specified suffix files already exist.\nDo you want to overwrite?", 
                    "Run Tests", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                for (int i = 0; i < _testsItems.Count; i++)
                {
                    TestItemList testItems = _testsItems[i];
                    if (testItems.Count == 0)
                    {
                        RunTests(testItems, i);
                    }
                }

                var w3cSerializer = new TestSerializer(_testsItems[0], _testsItems[1]);
                w3cSerializer.WriteXml(w3cExportPath);

                var issuesSerializer = new TestSerializer(_testsItems[2], _testsItems[3]);
                issuesSerializer.WriteXml(issuesExportPath);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void OnClickSelectedCopy(object sender, EventArgs e)
        {
            var fileName = labelSelected.Text;
            if (string.IsNullOrWhiteSpace(fileName) || _selectedListIndex < 0 || _seletedTabIndex < 0)
            {
                return;
            }
            var isIssue = fileName.StartsWith(IssuesPrefix);

            var svgBasePath = Path.GetFullPath(isIssue ? _svgIssuesBasePath : _svgW3CBasePath);
            Clipboard.SetText(Path.Combine(svgBasePath, fileName));
        }

        private void OnClickSelectedView(object sender, EventArgs e)
        {
            var fileName = labelSelected.Text;
            if (string.IsNullOrWhiteSpace(fileName) || _selectedListIndex < 0 || _seletedTabIndex < 0)
            {
                return;
            }

            ViewEvent?.Invoke(this, new ViewEventArgs(_seletedTabIndex, _selectedListIndex));
        }

        private void OnSelectedListIndexChanged(object sender, EventArgs e)
        {
            var selectedIndices = listView.SelectedIndices;
            if (selectedIndices.Count == 0)
            {
                this.ClearListSelection(false);
            }
            else
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                TestItem testItem = selectedItem.Tag as TestItem;
                if (testItem == null)
                {
                    this.ClearListSelection(false);
                    return;
                }
                _selectedListIndex = testItem.Index;
                labelSelected.Text = testItem.FileName;
                buttonSelectedCopy.Enabled = true;
                buttonSelectedView.Enabled = true;
            }
        }

        private void OnListItemClick(object sender, EventArgs e)
        {
            this.OnClickSelectedView(sender, e);
        }

        private void OnMenuItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == viewSelectedItem)
            {
                this.OnClickSelectedView(sender, e);
                return;
            }
            if (e.ClickedItem == copySelectedFilePath)
            {
                this.OnClickSelectedCopy(sender, e);
                return;
            }
            if (e.ClickedItem == copySelectedFileName)
            {
                var fileName = labelSelected.Text;
                if (string.IsNullOrWhiteSpace(fileName) || _selectedListIndex < 0 || _seletedTabIndex < 0)
                {
                    return;
                }

                Clipboard.SetText(fileName);
            }
        }

        private void OnMenuOpening(object sender, CancelEventArgs e)
        {
            var menuItems = new ToolStripItem[]
            {
                copySelectedFileName,
                copySelectedFilePath,
                toolStripSeparator1,
                viewSelectedItem
            };
            var fileName = labelSelected.Text;
            if (string.IsNullOrWhiteSpace(fileName) || _selectedListIndex < 0 || _seletedTabIndex < 0)
            {
                foreach (var menuItem in menuItems)
                {
                    menuItem.Enabled = false;
                }
                return;
            }

            foreach (var menuItem in menuItems)
            {
                menuItem.Enabled = true;
            }
        }

        private void ClearListSelection(bool clearList)
        {
            _selectedListIndex = -1;
            labelSelected.Text = "";
            buttonSelectedCopy.Enabled = false;
            buttonSelectedView.Enabled = false;

            if (clearList)
            {
                richTextBox.Clear();
                listView.Items.Clear();
            }
        }

        private void FillListView(TestItemList testItems)
        {
            listView.Items.Clear();
            if (testItems == null || testItems.Count == 0)
            {
                return;
            }

            foreach (var testItem in testItems)
            {
                var testState = TestState.None;
                var testRef = testItems[testItem.FileName];
                if (testRef != null)
                {
                    testState = testItem.GetState(testRef);
                }

                var viewItem = listView.Items.Add(new ListViewItem(testItem.ToList(testRef), testGroup));
                viewItem.Tag = testItem;

                if (testState == TestState.Positive)
                {
                    viewItem.BackColor = Color.LightGreen;
                }
                else if (testState == TestState.Negative)
                {
                    viewItem.BackColor = Color.Yellow;
                }

                if (testItem.IsException)
                {
                    viewItem.BackColor = Color.Crimson;
                    var exceptItem = listView.Items.Add(new ListViewItem(testItem.ToList(testRef), exceptGroup));
                    exceptItem.Tag = testItem;
                }

                if (testItems.Category == TestItem.Passing)
                {
                    if (testItem.Percentage > 5.0)
                    {
                        var revisitItem = listView.Items.Add(new ListViewItem(testItem.ToList(testRef), revisitGroup));
                        revisitItem.Tag = testItem;
                    }
                }
                else if (testItems.Category == TestItem.Failing)
                {
                    if (!testItem.IsException && testItem.Percentage < 5.0)
                    {
                        var revisitItem = listView.Items.Add(new ListViewItem(testItem.ToList(testRef), revisitGroup));
                        revisitItem.Tag = testItem;
                    }
                }

                if (testState == TestState.None || testItem.IsException)
                {
                    continue;
                }

                if (testState == TestState.Positive)
                {
                    var positiveItem = listView.Items.Add(new ListViewItem(testItem.ToList(testRef), positiveGroup));
                    positiveItem.Tag = testItem;
                }
                else if (testState == TestState.Negative)
                {
                    var negativeItem = listView.Items.Add(new ListViewItem(testItem.ToList(testRef), negativeGroup));
                    negativeItem.Tag = testItem;
                }
            }
        }

        private void RunTests(TestItemList testItems, int seletedTabIndex)
        {
            string fileName = string.Empty;
            try
            {
                var selectedItems = _listItems[seletedTabIndex];
                for (int index = 0; index < selectedItems.Length; index++)
                {
                    fileName = selectedItems[index];
                    RunTest(fileName, testItems);
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
        }

        private void RunTest(string fileName, TestItemList testItems, bool log = true)
        {
#if NET5_0_OR_GREATER
            if (!OperatingSystem.IsWindows())
                return;
#endif

            TestItem testItem = new TestItem(testItems.Count, fileName);
            testItems.Add(testItem);

            var isIssue = fileName.StartsWith(IssuesPrefix);

            var pngBasePath = Path.GetFullPath(isIssue ? _pngIssuesBasePath : _pngW3CBasePath);
            var svgBasePath = Path.GetFullPath(isIssue ? _svgIssuesBasePath : _svgW3CBasePath);

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
                    if (Math.Abs(svgImage.Width - pngImage.Width) > 10
                        || Math.Abs(svgImage.Height - pngImage.Height) > 10)
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

                testItem.Percentage = percentage;
            }
            catch (Exception ex)
            {
                testItem.IsException = true;
                testItem.Percentage = -1;

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
            }
        }
    }
}
