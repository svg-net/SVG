using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Svg;
using System.Xml;
using Svg.Transforms;

namespace XMLOutputTester
{
    public partial class Form1 : Form
    {
        private SvgDocument FSvgDoc;

        public Form1()
        {
            InitializeComponent();


            FSvgDoc = new SvgDocument
            {
                Width = 500,
                Height = 500
            };

            FSvgDoc.ViewBox = new SvgViewBox(-250, -250, 500, 500);

            var group = new SvgGroup();
            FSvgDoc.Children.Add(group);

            group.Children.Add(new SvgCircle
            {
                Radius = 100,
                Fill = new SvgColourServer(Color.Red),
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 2
            });

            var stream = new MemoryStream();
            FSvgDoc.Write(stream);
            textBox1.Text = Encoding.UTF8.GetString(stream.GetBuffer());

            pictureBox1.Image = FSvgDoc.Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(SVG)|*.svg";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FSvgDoc.Write(saveFileDialog1.FileName);
            }
        }
    }
}
