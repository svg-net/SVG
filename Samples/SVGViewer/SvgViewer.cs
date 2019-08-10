using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Svg;

namespace SVGViewer
{
    public partial class SVGViewer : Form
    {
        public SVGViewer()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (openSvgFile.ShowDialog() == DialogResult.OK)
                {
                    var svgDoc = SvgDocument.Open(openSvgFile.FileName);
                    RenderSvg(svgDoc);

                    textBox1.TextChanged -= TextBox1_TextChanged;
                    try
                    {
                        var xmlDoc = new XmlDocument
                        {
                            XmlResolver = null
                        };
                        xmlDoc.Load(openSvgFile.FileName);
                        textBox1.Text = xmlDoc.InnerXml;
                    }
                    finally
                    {
                        textBox1.TextChanged += TextBox1_TextChanged;
                    }
                }
            }
            catch
            {
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var svgDoc = SvgDocument.FromSvg<SvgDocument>(textBox1.Text);
                RenderSvg(svgDoc);
            }
            catch
            {
            }
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
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
            if (svgImage.Image != null)
                svgImage.Image.Dispose();

            //using (var render = new DebugRenderer())
            //    svgDoc.Draw(render);
            svgImage.Image = svgDoc.Draw();

            var baseUri = svgDoc.BaseUri;
            var outputDir = Path.GetDirectoryName(baseUri != null && baseUri.IsFile ? baseUri.LocalPath : Application.ExecutablePath);
            svgImage.Image.Save(Path.Combine(outputDir, "output.png"));
            //svgDoc.Write(Path.Combine(outputDir, "output.svg"));
        }
    }
}
