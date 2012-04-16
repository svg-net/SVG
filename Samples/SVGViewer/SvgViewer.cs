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
            if (openSvgFile.ShowDialog() == DialogResult.OK)
            {
                
            }
        }

        private string FXML = "";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            var s = new MemoryStream(UTF8Encoding.Default.GetBytes(textBox1.Text));
            SvgDocument svgDoc = SvgDocument.Open(s, null);

            svgDoc.Transforms = new SvgTransformCollection();
            svgDoc.Transforms.Add(new SvgScale(1, 1));
            svgDoc.Width = new SvgUnit(svgDoc.Width.Type, svgDoc.Width * 0.25f);
            svgDoc.Height = new SvgUnit(svgDoc.Height.Type, svgDoc.Height);
            svgImage.Image = svgDoc.Draw();

        }
    }
}
