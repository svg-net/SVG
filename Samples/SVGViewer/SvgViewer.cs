using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Svg;
using Svg.Transforms;
using System.Xml;
using System.IO;

namespace SVGViewer
{
    public partial class SVGViewer : Form
    {
        public SVGViewer()
        {
            InitializeComponent();
        }

        private void open_Click(object sender, EventArgs e)
        {
            try
            {
                if (openSvgFile.ShowDialog() == DialogResult.OK)
                {
                    SvgDocument svgDoc = SvgDocument.Open(openSvgFile.FileName);
                    RenderSvg(svgDoc);
                }
            }
            catch
            {
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var s = new MemoryStream(Encoding.UTF8.GetBytes(textBox1.Text)))
                {
                    SvgDocument svgDoc = SvgDocument.Open<SvgDocument>(s, null);
                    RenderSvg(svgDoc);
                }
            }
            catch
            {
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    (sender as TextBox).SelectAll();
                }
            }
            catch
            {
            }
        }

        private void RenderSvg(SvgDocument svgDoc)
        {
            //var render = new DebugRenderer();
            //svgDoc.Draw(render);
            svgImage.Image = svgDoc.Draw();

            string outputDir;
            if (svgDoc.BaseUri == null)
                outputDir = System.IO.Path.GetDirectoryName(Application.ExecutablePath); 
            else
                outputDir = System.IO.Path.GetDirectoryName(svgDoc.BaseUri.LocalPath);
            svgImage.Image.Save(System.IO.Path.Combine(outputDir, "output.png"));
        }
    }
}
