using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Svg;

namespace XMLOutputTester
{
    public partial class Form1 : Form
    {
        private SvgDocument svgDoc;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            svgDoc = new SvgDocument
            {
                Width = 500,
                Height = 500,
                ViewBox = new SvgViewBox(-250, -250, 500, 500),
            };

            var group = new SvgGroup();
            svgDoc.Children.Add(group);

            group.Children.Add(new SvgCircle
            {
                Radius = 100,
                Fill = new SvgColourServer(Color.Red),
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 2
            });

            var stream = new MemoryStream();
            svgDoc.Write(stream);
            textBox1.Text = Encoding.UTF8.GetString(stream.GetBuffer());

            pictureBox1.Image = svgDoc.Draw();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                svgDoc.Write(saveFileDialog1.FileName);
            }
        }
    }
}
