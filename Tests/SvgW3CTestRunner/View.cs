using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Svg;

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
            var files = (from f in (from g in Directory.GetFiles(_svgBasePath)
                                    select Path.GetFileName(g))
                         where !f.StartsWith("animate-") && !f.StartsWith("conform-viewer") &&
                            !f.Contains("-dom-") && !f.StartsWith("linking-") && !f.StartsWith("interact-")
                         orderby f
                         select (object)f);
            lstFiles.Items.AddRange(files.ToArray());
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fileName = lstFiles.SelectedItem.ToString();
            var doc = SvgDocument.Open(_svgBasePath + fileName);
            picSvg.Image = doc.Draw();
            var png = Image.FromFile(_pngBasePath + Path.GetFileNameWithoutExtension(fileName) + ".png");
            picPng.Image = png;
        }
    }
}
