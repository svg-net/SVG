using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Svg;

namespace SvgW3CTestRunner
{
    public partial class View : Form
    {
        private const string TitleSVGPNG = "SVG vs PNG";
        private const string FixImage = "smiley.png";

        private const string IssuesPrefix = "__";

        //Data folders
        private static readonly string _svgW3CBasePath = Path.Combine("..", "..", "..", "..", "W3CTestSuite", "svg");
        private static readonly string _pngW3CBasePath = Path.Combine("..", "..", "..", "..", "W3CTestSuite", "png");

        //Data folders
        private static readonly string _svgIssuesBasePath = Path.Combine("..", "..", "..", "..", "Issues", "svg");
        private static readonly string _pngIssuesBasePath = Path.Combine("..", "..", "..", "..", "Issues", "png");

        [DllImport("Shlwapi.dll", EntryPoint = "PathIsDirectoryEmpty")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsDirectoryEmpty([MarshalAs(UnmanagedType.LPStr)] string directory);

        private const string W3CTestSuiteUrl
            = "https://github.com/ElinamLLC/SharpVectors-TestSuites/raw/master/Svg11.zip";

        private string[] listW3CPassing;
        private string[] listW3CFailing;

        private string[] listOtherPassing;
        private string[] listOtherFailing;

        public View()
        {
            InitializeComponent();

            this.Load += OnFormLoad;

            var screenBounds = Screen.PrimaryScreen.WorkingArea;

            int width = screenBounds.Width;
            int height = screenBounds.Height;
            if (width <= 1280)
            {
                this.Width = (int)(Math.Min(1280, width) * 0.95);
            }
            else
            {
                this.Width = (int)(width * 0.80);
            }
            this.Height = (int)(height * 0.90);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                ListBox[] listItems = {
                    lstW3CFilesPassing,
                    lstW3CFilesFailing,
                    lstFilesOtherPassing,
                    lstFilesOtherFailing
                };

                ListSearchDialog dlg = new ListSearchDialog();
                dlg.ListItems = listItems;
                dlg.SeletedTabIndex = fileTabBox.SelectedIndex;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var selectedList = listItems[dlg.SeletedTabIndex];
                    var selectedIndex = selectedList.SelectedIndex;
                    selectedList.ClearSelected();

                    fileTabBox.SelectedIndex = dlg.SeletedTabIndex;
                    selectedList.SelectedIndex = selectedIndex;
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static bool IsTestSuiteAvailable()
        {
            if (Directory.Exists(_svgW3CBasePath) == false)
            {
                return false;
            }
            if (Directory.Exists(_pngW3CBasePath) == false)
            {
                return false;
            }
            string svgDir = Path.GetFullPath(_svgW3CBasePath);
            if (!Directory.Exists(svgDir) || IsDirectoryEmpty(svgDir) == true)
            {
                return false;
            }
            string pngDir = Path.Combine(_pngW3CBasePath);
            if (!Directory.Exists(pngDir) || IsDirectoryEmpty(pngDir) == true)
            {
                return false;
            }

            return true;
        }

        private static async Task DownloadW3CTestSuite(string downloadedFilePath)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (HttpClient client = new HttpClient())
            {
                using (Stream streamToReadFrom = await client.GetStreamAsync(W3CTestSuiteUrl))
                {
                    using (Stream streamToWriteTo = new FileStream(downloadedFilePath, FileMode.CreateNew))
                    {
                        await streamToReadFrom.CopyToAsync(streamToWriteTo);
                    }
                }
            }
        }

        private async void OnFormLoad(object sender, EventArgs e)
        {
            if (!IsTestSuiteAvailable())
            {
                string svgDir = Path.GetFullPath(_svgW3CBasePath);
                var downloadedFilePath = Path.GetFullPath(Path.Combine(svgDir, "..", "Svg11.zip"));
                string destinationDirectory = Path.GetDirectoryName(downloadedFilePath);

                if (File.Exists(downloadedFilePath))
                {
                    File.Delete(downloadedFilePath);
                }

                await DownloadW3CTestSuite(downloadedFilePath);

                await Task.Delay(100);

                ZipFile.ExtractToDirectory(downloadedFilePath, destinationDirectory);

                var sourceImage = Path.Combine(destinationDirectory, "images", FixImage);
                var destImage = Path.Combine(destinationDirectory, "svg", FixImage);
                File.Copy(sourceImage, destImage);

                if (File.Exists(downloadedFilePath))
                {
                    File.Delete(downloadedFilePath);
                }
            }

            this.LoadW3CTestSuite();
            this.LoadIssuesAndPullRequests();
        }

        private void LoadW3CTestSuite()
        {
            // ignore tests pertaining to javascript or xml reading
            var passingtestsTxt = Path.Combine(_svgW3CBasePath, "..", "PassingTests.txt");
            var passes = File.ReadAllLines(passingtestsTxt).ToDictionary((f) => f, (f) => true);
            var files = from f in
                            from g in Directory.GetFiles(_svgW3CBasePath) select Path.GetFileName(g)
                        where !f.StartsWith("animate-") && !f.StartsWith("conform-viewer") &&
                        !f.Contains("-dom-") && !f.StartsWith("linking-") && !f.StartsWith("interact-") &&
                        !f.StartsWith("script-") && f.EndsWith(".svg")
                        && File.Exists(Path.Combine(_pngW3CBasePath, Path.ChangeExtension(f, "png")))
                        orderby f
                        select f;

            listW3CPassing = files.Where(f => passes.ContainsKey(f)).ToArray();
            listW3CFailing = files.Where(f => !passes.ContainsKey(f)).ToArray();
            lstW3CFilesPassing.Items.AddRange(listW3CPassing);
            lstW3CFilesFailing.Items.AddRange(listW3CFailing);
        }

        private void LoadIssuesAndPullRequests()
        {
            // ignore tests pertaining to javascript or xml reading
            var passingtestsTxt = Path.Combine(_svgIssuesBasePath, "..", "PassingTests.txt");
            var passes = File.ReadAllLines(passingtestsTxt).ToDictionary((f) => f, (f) => true);
            var files = from f in
                            from g in Directory.GetFiles(_svgIssuesBasePath) select Path.GetFileName(g)
                        where !f.StartsWith("animate-") && !f.StartsWith("conform-viewer") &&
                        !f.Contains("-dom-") && !f.StartsWith("linking-") && !f.StartsWith("interact-") &&
                        !f.StartsWith("script-") && f.EndsWith(".svg")
                        && File.Exists(Path.Combine(_pngIssuesBasePath, Path.ChangeExtension(f, "png")))
                        orderby f
                        select f;

            listOtherPassing = files.Where(f => passes.ContainsKey(f)).ToArray();
            listOtherFailing = files.Where(f => !passes.ContainsKey(f)).ToArray();
            lstFilesOtherPassing.Items.AddRange(listOtherPassing);
            lstFilesOtherFailing.Items.AddRange(listOtherFailing);
        }

        private void boxConsoleLog_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {   //click event

                var contextMenu = new System.Windows.Forms.ContextMenuStrip();
                var menuItem = new ToolStripMenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.Items.Add(menuItem);

                boxConsoleLog.ContextMenuStrip = contextMenu;
            }
        }

        void CopyAction(object sender, EventArgs e)
        {
            if (boxConsoleLog.SelectedText != null && boxConsoleLog.SelectedText != "")
            {
                //Clipboard.SetText(boxConsoleLog.SelectedText.Replace("\n", "\r\n"));

                boxConsoleLog.Copy();
            }
        }

        private void OnW3CSelectedIndexChanged(object sender, EventArgs e)
        {
#if NET5_0_OR_GREATER
            if (!OperatingSystem.IsWindows())
                return;
#endif

            this.ClearPictureBoxes();

            //render svg
            var lstFiles = sender as ListBox;
            if (lstFiles.SelectedIndex < 0)
            {
                return;
            }

            var fileName = lstFiles.SelectedItem.ToString();
            if (fileName.StartsWith("#")) return;

            this.Cursor = Cursors.WaitCursor;

            //display png
            var png = Image.FromFile(Path.Combine(_pngW3CBasePath, Path.ChangeExtension(fileName, "png")));
            picPng.Image = png;

            var doc = new SvgDocument();
            try
            {
                Debug.Print(fileName);
                doc = SvgDocument.Open(Path.Combine(_svgW3CBasePath, fileName));
                if (fileName.StartsWith(IssuesPrefix))
                {
                    picSvg.Image = doc.Draw();
                }
                else
                {
                    var img = new Bitmap(480, 360);
                    doc.Draw(img);
                    picSvg.Image = img;
                }

                this.boxConsoleLog.AppendText("WC3 TEST " + fileName + "\n");
                this.boxDescription.Text = GetDescription(doc);

            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG RENDERING ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSvg.Image = null;
            }

            //save load
            try
            {
                using (var memStream = new MemoryStream())
                {
                    doc.Write(memStream);
                    memStream.Position = 0;
                    var baseUri = doc.BaseUri;
                    doc = SvgDocument.Open<SvgDocument>(memStream);
                    doc.BaseUri = baseUri;

                    if (fileName.StartsWith(IssuesPrefix))
                    {
                        picSaveLoad.Image = doc.Draw();
                    }
                    else
                    {
                        var img = new Bitmap(480, 360);
                        doc.Draw(img);
                        picSaveLoad.Image = img;
                    }
                }
            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG SERIALIZATION ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSaveLoad.Image = null;
            }

            //compare svg to png
            try
            {
                picSVGPNG.Image = BitmapUtils.PixelDiff((Bitmap)picPng.Image, (Bitmap)picSvg.Image);
                var difference = picSvg.Image.PercentageDifference(picPng.Image);
                var percentage = Math.Round(difference * 100.0, 2);
                labelSVGPNG.Text = $"{TitleSVGPNG} - Difference is {percentage}%";
                labelSVGPNG.ForeColor = percentage > 5.0 ? Color.Crimson : Color.Black;
            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG TO PNG COMPARISON ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSVGPNG.Image = null;
                labelSVGPNG.Text = $"{TitleSVGPNG} - Exception occurred";
                labelSVGPNG.ForeColor = Color.Red;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void OnIssuesSelectedIndexChanged(object sender, EventArgs e)
        {
#if NET5_0_OR_GREATER
            if (!OperatingSystem.IsWindows())
                return;
#endif
            this.ClearPictureBoxes();

            //render svg
            var lstFiles = sender as ListBox;
            if (lstFiles.SelectedIndex < 0)
            {
                return;
            }

            var fileName = lstFiles.SelectedItem.ToString();
            if (fileName.StartsWith("#")) return;

            this.Cursor = Cursors.WaitCursor;

            //display png
            var png = Image.FromFile(Path.Combine(_pngIssuesBasePath, Path.ChangeExtension(fileName, "png")));
            picPng.Image = png;

            var doc = new SvgDocument();
            try
            {
                Debug.Print(fileName);
                doc = SvgDocument.Open(Path.Combine(_svgIssuesBasePath, fileName));
                if (fileName.StartsWith(IssuesPrefix))
                {
                    var svgImage = doc.Draw();
                    // Check for a large difference in image size, if not nearly equal recreate it
                    if (Math.Abs(svgImage.Width - png.Width) > 10 || Math.Abs(svgImage.Height - png.Height) > 10)
                    {
                        svgImage.Dispose();
                        svgImage = new Bitmap(png.Width, png.Height);
                        doc.Draw(svgImage);
                    }
                    picSvg.Image = svgImage;
                }
                else
                {
                    var img = new Bitmap(480, 360);
                    doc.Draw(img);
                    picSvg.Image = img;
                }

                this.boxConsoleLog.AppendText("Issues/Pull-Requests TEST " + fileName + "\n");
                this.boxDescription.Text = GetDescription(doc);

                var difference = picSvg.Image.PercentageDifference(picPng.Image);
                var percentage = Math.Round(difference * 100.0, 2);
                labelSVGPNG.Text = $"{TitleSVGPNG} - Difference is {percentage}%";
                labelSVGPNG.ForeColor = percentage > 5.0 ? Color.Crimson : Color.Black;
            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG RENDERING ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSvg.Image = null;
                labelSVGPNG.Text = $"{TitleSVGPNG} - Exception occurred";
                labelSVGPNG.ForeColor = Color.Red;
            }

            //save load
            try
            {
                using (var memStream = new MemoryStream())
                {
                    doc.Write(memStream);
                    memStream.Position = 0;
                    var baseUri = doc.BaseUri;
                    doc = SvgDocument.Open<SvgDocument>(memStream);
                    doc.BaseUri = baseUri;

                    if (fileName.StartsWith(IssuesPrefix))
                    {
                        picSaveLoad.Image = doc.Draw();
                    }
                    else
                    {
                        var img = new Bitmap(480, 360);
                        doc.Draw(img);
                        picSaveLoad.Image = img;
                    }
                }
            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG SERIALIZATION ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSaveLoad.Image = null;
            }

            //compare svg to png
            try
            {
                picSVGPNG.Image = BitmapUtils.PixelDiff((Bitmap)picPng.Image, (Bitmap)picSvg.Image);
            }
            catch (Exception ex)
            {
                this.boxConsoleLog.AppendText("Result: TEST FAILED\n");
                this.boxConsoleLog.AppendText("SVG TO PNG COMPARISON ERROR for " + fileName + "\n");
                this.boxConsoleLog.AppendText(ex.ToString());
                picSVGPNG.Image = null;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ClearPictureBoxes()
        {
            PictureBox[] pictureBoxes = {
                picSvg,
                picPng,
                picSaveLoad,
                picSVGPNG
            };
            foreach (var pictureBox in pictureBoxes)
            {
                var pictureImage = pictureBox.Image;
                pictureBox.Image = null;
                pictureImage?.Dispose();
            }

            labelSVGPNG.Text = TitleSVGPNG;
            labelSVGPNG.ForeColor = Color.Black;
        }

        private void fileTabBox_TabIndexChanged(object sender, EventArgs e)
        {
            this.ClearPictureBoxes();
        }

        private SvgElement GetChildWithDescription(SvgElement element, string description)
        {
            var docElements = element.Children.Where(child => child is NonSvgElement && (child as NonSvgElement).Name == description);
            return docElements.Count() > 0 ? docElements.First() : null;
        }

        private string GetDescription(SvgDocument document)
        {
            string description = string.Empty;
            var testCaseElement = GetChildWithDescription(document, "SVGTestCase");
            if (testCaseElement != null)
            {
                var descriptionElement = GetChildWithDescription(testCaseElement, "testDescription");
                if (descriptionElement != null)
                {
                    var regex = new Regex("\r\n *");
                    var descriptionLines = new List<string>();
                    foreach (var child in descriptionElement.Children)
                    {
                        if (child.Content != null)
                            descriptionLines.Add(regex.Replace(child.Content, " "));
                    }
                    return string.Join("\n", descriptionLines.ToArray());
                }
            }
            return description;
        }

        private void OnClickRunTests(object sender, EventArgs e)
        {
            List<string[]> listItems = new List<string[]>{
                listW3CPassing,
                listW3CFailing,
                listOtherPassing,
                listOtherFailing
            };

            var dlg = new RunTestsDialog();
            dlg.ListItems = listItems;
            dlg.SeletedTabIndex = fileTabBox.SelectedIndex;

            dlg.SvgW3CBasePath = _svgW3CBasePath;
            dlg.PngW3CBasePath = _pngW3CBasePath;
            dlg.SvgIssuesBasePath = _svgIssuesBasePath;
            dlg.PngIssuesBasePath = _pngIssuesBasePath;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                //var selectedList = listItems[dlg.SeletedTabIndex];
                //var selectedIndex = selectedList.SelectedIndex;
                //selectedList.ClearSelected();

                //fileTabBox.SelectedIndex = dlg.SeletedTabIndex;
                //selectedList.SelectedIndex = selectedIndex;
            }
        }

        private void OnClickSearch(object sender, EventArgs e)
        {
            ListBox[] listItems = {
                lstW3CFilesPassing,
                lstW3CFilesFailing,
                lstFilesOtherPassing,
                lstFilesOtherFailing
            };

            ListSearchDialog dlg = new ListSearchDialog();
            dlg.ListItems = listItems;
            dlg.SeletedTabIndex = fileTabBox.SelectedIndex;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var selectedList = listItems[dlg.SeletedTabIndex];
                var selectedIndex = selectedList.SelectedIndex;
                selectedList.ClearSelected();

                fileTabBox.SelectedIndex = dlg.SeletedTabIndex;
                selectedList.SelectedIndex = selectedIndex;
            }
        }
    }
}
