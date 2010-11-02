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
                SvgDocument svgDoc = SvgDocument.Open(openSvgFile.FileName);
                svgDoc.Transforms = new SvgTransformCollection();
                svgDoc.Transforms.Add(new SvgScale(2, 2));
                svgDoc.Width = new SvgUnit(svgDoc.Width.Type, svgDoc.Width * 2);
                svgDoc.Height = new SvgUnit(svgDoc.Height.Type, svgDoc.Height * 2);
                svgImage.Image = svgDoc.Draw();
            }
        }
    }
}
