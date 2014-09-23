using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Svg;
using System.Diagnostics;

namespace SvgW3CTestRunner
{
    public partial class View : Form
    {
        private const string _svgBasePath = @"..\..\..\W3CTestSuite\svg\";
        private const string _pngBasePath = @"..\..\..\W3CTestSuite\png\";

        public View()
        {
            InitializeComponent();
            // ignore tests pertaining to javascript or xml reading
            var passes = File.ReadAllLines(_svgBasePath + @"..\PassingTests.txt").ToDictionary((f) => f, (f) => true);
            var files = (from f in
                             (from g in Directory.GetFiles(_svgBasePath)
                                    select Path.GetFileName(g))
                         where !f.StartsWith("animate-") && !f.StartsWith("conform-viewer") &&
                            !f.Contains("-dom-") && !f.StartsWith("linking-") && !f.StartsWith("interact-") &&
                            !f.StartsWith("script-")
                         orderby f
                         select (object)f);
            files = files.Where((f) => !passes.ContainsKey((string)f)).Union(Enumerable.Repeat((object)"## PASSING ##", 1)).Union(files.Where((f) => passes.ContainsKey((string)f)));

            lstFiles.Items.AddRange(files.ToArray());
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fileName = lstFiles.SelectedItem.ToString();
            if (fileName.StartsWith("#")) return;
            try
            {
                Debug.Print(fileName);
                var doc = SvgDocument.Open(_svgBasePath + fileName);
                if (fileName.StartsWith("__"))
                {
                    picSvg.Image = doc.Draw(); 
                }
                else
                {
                    var img = new Bitmap(480, 360);
                    doc.Draw(img);
                    picSvg.Image = img; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                picSvg.Image = null;
            }
            
            var png = Image.FromFile(_pngBasePath + Path.GetFileNameWithoutExtension(fileName) + ".png");
            picPng.Image = png;
        }
    }
}
