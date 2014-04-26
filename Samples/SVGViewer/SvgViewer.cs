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
                SvgDocument svgDoc = SvgDocument.Open(openSvgFile.FileName);

                DrawDoc(svgDoc);
            }
        }

        private void ResizeDoc(SvgDocument document)
        {
            if (document.Height > svgImage.Image.Height)
            {
                document.Width = (int)(((double)document.Width / (double)document.Height) * (double)svgImage.Image.Height);
                document.Height = svgImage.Image.Height;
            }
        }

        private void DrawDoc(SvgDocument svgDoc)
        {
            ResizeDoc(svgDoc);
            svgImage.Image = svgDoc.Draw();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            // Need to now disable the DTD settings 
            // (seems to throw an exception, be slow at .NET 4 if you don't)
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Parse;

            XmlReader xmlReader = XmlReader.Create(new StringReader(textBox1.Text), 
                settings);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlReader);

            SvgDocument svgDoc = SvgDocument.Open(xmlDoc);

            DrawDoc(svgDoc);
        }
    }
}
