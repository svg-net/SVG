using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestPdfSharp
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if false
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif
            PdfSharp.Drawing.XColor red = PdfSharp.Drawing.XColor.FromKnownColor(System.Drawing.KnownColor.Red);
            PdfSharp.Drawing.XRect rect1 = new PdfSharp.Drawing.XRect(0+100, 0+100, 100, 12.0+2.5);

            // PdfSharp.Drawing.XRect rect1 = new PdfSharp.Drawing.XRect(0, 0, svgWidth, svgHeight);


            PdfSharp.Drawing.XFont font = new PdfSharp.Drawing.XFont("Arial"
                    , 12.0, PdfSharp.Drawing.XFontStyle.Bold
            );



            using (PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument())
            {
                document.Info.Title = "Test Document";
                document.Info.Author = "Stefan Steiger";
                document.Info.Subject = "Test Document";
                document.Info.Keywords = "Test Document, Test, Document";

                PdfSharp.Pdf.PdfPage page = document.AddPage();
                page.Orientation = PdfSharp.PageOrientation.Landscape;


                using (PdfSharp.Drawing.XGraphics gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
                {
                    PdfSharp.Drawing.Layout.XTextFormatter tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
                    tf.Alignment = PdfSharp.Drawing.Layout.XParagraphAlignment.Left;



                    double dblLineWidth = 1.0;
                    PdfSharp.Drawing.XColor lineColor = PdfSharp.Drawing.XColorHelper.FromHtml("#FF00FF");
                    PdfSharp.Drawing.XPen pen = new PdfSharp.Drawing.XPen(lineColor, dblLineWidth);

                    gfx.DrawLine(pen, 0, 0, rect1.X, rect1.Y);
                    gfx.DrawLine(pen, rect1.X, rect1.Y - 10, rect1.X + rect1.Width, rect1.Y - 10);



                    double radius = 3.0;
                    gfx.DrawEllipse(PdfSharp.Drawing.XBrushes.Red, new PdfSharp.Drawing.XRect(rect1.X - radius, rect1.Y - radius, radius * 2, radius * 2));
                    gfx.DrawEllipse(PdfSharp.Drawing.XBrushes.Red, new PdfSharp.Drawing.XRect(rect1.X - radius + rect1.Width / 2.0, rect1.Y - radius, radius * 2, radius * 2));
                    gfx.DrawEllipse(PdfSharp.Drawing.XBrushes.Red, new PdfSharp.Drawing.XRect(rect1.X - radius + rect1.Width, rect1.Y - radius, radius * 2, radius * 2));

                    

                    PdfSharp.Drawing.XPen rect1Color = new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Green, 0.01f);
                    gfx.DrawRectangle(rect1Color, rect1);


                    string text = "Test rotation 90°";
                    tf.DrawString(text
                        , font
                        , PdfSharp.Drawing.XBrushes.Black
                        , rect1
                        , PdfSharp.Drawing.XStringFormats.TopLeft
                    );


                    gfx.Save();


                    // https://stackoverflow.com/questions/10210134/using-a-matrix-to-rotate-rectangles-individually


                    // Midpoint 
                    double xx = rect1.X + rect1.Width / 2.0;
                    double yy = rect1.Y + rect1.Height / 2.0;


                    xx = rect1.X; // +rect1.Height / 2.0; ;
                    yy = rect1.Y + rect1.Height;


                    xx = rect1.X + rect1.Width / 2.0;
                    yy = rect1.Y + rect1.Height;


                    gfx.TranslateTransform(xx, yy);
                    gfx.DrawEllipse(PdfSharp.Drawing.XBrushes.Yellow, new PdfSharp.Drawing.XRect(0 - radius, 0 - radius, radius * 2, radius * 2));
                    gfx.RotateTransform(90);
                    gfx.TranslateTransform(-xx, -yy);


                    tf.DrawString(text
                                           , font
                                           , PdfSharp.Drawing.XBrushes.Black
                                           , rect1
                                           , PdfSharp.Drawing.XStringFormats.TopLeft
                               );



                    gfx.Restore();

                    /*
                    gfx.TranslateTransform(rect1.X + (float)rect1.Width / 2, rect1.Y + (float)rect1.Height / 2);
                    gfx.RotateTransform(30);
                    gfx.TranslateTransform(-rect1.X - (float)rect1.Width / 2, -rect1.Y - (float)rect1.Height / 2);

                    tf.DrawString(text
                                           , font
                                           , PdfSharp.Drawing.XBrushes.Black
                                           , rect1
                                           , PdfSharp.Drawing.XStringFormats.TopLeft
                               );
                    */


                }


                byte[] baPdfDocument;

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    document.Save(ms, false);
                    ms.Flush();

                    // baPdfDocument = new byte[ms.Length];
                    // ms.Seek(0, System.IO.SeekOrigin.Begin);
                    // ms.Read(baPdfDocument, 0, (int)ms.Length);

                    baPdfDocument = ms.ToArray();
                } // End Using ms 


                System.IO.File.WriteAllBytes("TestFile.pdf", baPdfDocument);
            } // End Using document 

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub 


    } // End Class Program 


} // End Namespace TestPdfSharp 
