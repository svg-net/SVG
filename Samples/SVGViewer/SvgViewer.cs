
using System.Windows.Forms;


namespace SVGViewer
{


    public partial class SVGViewer : System.Windows.Forms.Form
    {


        public SVGViewer()
        {
            InitializeComponent();
        }


        private void open_Click(object sender, System.EventArgs e)
        {
            if (openSvgFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Svg.SvgDocument svgDoc = Svg.SvgDocument.Open(openSvgFile.FileName);
                // RenderSvgToImage(svgDoc);
                RenderSvgToPdf(svgDoc);
            } // End if (openSvgFile.ShowDialog() == System.Windows.Forms.DialogResult.OK) 

        } // End Sub open_Click 


        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            using (System.IO.MemoryStream s = new System.IO.MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(textBox1.Text)))
        	{
                Svg.SvgDocument svgDoc = Svg.SvgDocument.Open<Svg.SvgDocument>(s, null);
                // RenderSvgToImage(svgDoc);
                RenderSvgToPdf(svgDoc);
            } // End if (openSvgFile.ShowDialog() == System.Windows.Forms.DialogResult.OK) 

        } // End Sub textBox1_TextChanged 


        private void RenderSvgToImage(Svg.SvgDocument svgDoc)
        {
            // var render = new DebugRenderer();
            // svgDoc.Draw(render);
            svgImage.Image = svgDoc.Draw();
            
            string outputDir;
            if (svgDoc.BaseUri == null)
                outputDir = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            else
                outputDir = System.IO.Path.GetDirectoryName(svgDoc.BaseUri.LocalPath);

            svgImage.Image.Save(System.IO.Path.Combine(outputDir, "output.png"));
        } // End Sub RenderSvg 


        private void RenderSvgToPdf(Svg.SvgDocument svgDoc)
        {
            PdfSharp.Pdf.PdfDocument doc = svgDoc.DrawPdf();

            string outputDir;
            if (svgDoc.BaseUri == null)
                outputDir = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            else
                outputDir = System.IO.Path.GetDirectoryName(svgDoc.BaseUri.LocalPath);

            doc.Save(System.IO.Path.Combine(outputDir, "output.pdf"));
        } // End Sub RenderSvgToPdf 


    } // End Class SVGViewer : System.Windows.Forms.Form 


} // End Namespace SVGViewer 
