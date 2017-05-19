
// #define AS_IMAGE


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Svg
{


    /// <summary>
    /// Convenience wrapper around a graphics object
    /// </summary>
    public sealed class SvgRenderer : IDisposable, IGraphicsProvider, ISvgRenderer
    {

        #region helpers


        public static System.Drawing.Drawing2D.Matrix ConvertMatrix(PdfSharp.Drawing.XMatrix mat)
        {
            // return new double[] { this.m11, this.m12, this.m21, this.m22, this.offsetX, this.offsetY };
            double[] elem = mat.GetElements();

            var ret = new System.Drawing.Drawing2D.Matrix(
                  (float)elem[0]
                , (float)elem[1]
                , (float)elem[2]
                , (float)elem[3]
                , (float)elem[4]
                , (float)elem[5]
            );

            return ret;
        }


        public static PdfSharp.Drawing.XMatrix ConvertMatrix(System.Drawing.Drawing2D.Matrix value)
        {
            float[] elem = value.Elements;

            return new PdfSharp.Drawing.XMatrix(
                  (double)elem[0]
                , (double)elem[1]
                , (double)elem[2]
                , (double)elem[3]
                , (double)elem[4]
                , (double)elem[5]
            );
        }


        public static PdfSharp.Drawing.XGraphicsPath ConvertPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            PdfSharp.Drawing.XGraphicsPath path2 = null;

            try
            {
                path2 = new PdfSharp.Drawing.XGraphicsPath(
                    path.PathPoints, path.PathTypes, (PdfSharp.Drawing.XFillMode)path.FillMode
                );

            }
            catch (System.Exception ex)
            {
                // System.Console.WriteLine(ex.Message);
                return null;
            }


            return path2;
        }

        public static PdfSharp.Drawing.XBrush ConvertBrush(System.Drawing.Brush brush)
        {
            PdfSharp.Drawing.XColor color = PdfSharp.Drawing.XColors.Transparent;
            // PdfSharp.Drawing.XColor color = PdfSharp.Drawing.XColors.HotPink;

            if (object.ReferenceEquals(brush, typeof(System.Drawing.SolidBrush)))
            {
                System.Drawing.SolidBrush sb = (System.Drawing.SolidBrush)brush;
                color = PdfSharp.Drawing.XColor.FromArgb(sb.Color);
            }

            return new PdfSharp.Drawing.XSolidBrush(color);
        }


        public static PdfSharp.Drawing.XPoint ConvertPoint(System.Drawing.Point point)
        {
            return new PdfSharp.Drawing.XPoint(point.X, point.Y);
        }


        public static PdfSharp.Drawing.XPen ConvertPen(System.Drawing.Pen pen)
        {
            var col = PdfSharp.Drawing.XColor.FromArgb(pen.Color);
            return new PdfSharp.Drawing.XPen(col, pen.Width);
        }


        public static PdfSharp.Drawing.XRect ConvertRectangle(System.Drawing.Rectangle rect)
        {
            return new PdfSharp.Drawing.XRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static PdfSharp.Drawing.XRect ConvertRectangle(System.Drawing.RectangleF rect)
        {
            return new PdfSharp.Drawing.XRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static PdfSharp.Drawing.XGraphicsUnit ConvertUnit(System.Drawing.GraphicsUnit graphicsUnit)
        {
            /*
            public enum PdfSharp.Drawing.XGraphicsUnit
            {
                Point = 0, // World
                Inch = 1, // Display
                Millimeter = 2, // Pixel 
                Centimeter = 3, // Point 
                Presentation = 4, // Inch 
            }
            */

            /*
            public enum System.Drawing.GraphicsUnit
            {
                World = 0, // Specifies the world coordinate system unit as the unit of measure.
                // Specifies the unit of measure of the display device. 
                // Typically pixels for video displays, and 1/100 inch for printers.
                Display = 1,
                Pixel = 2, 
                Point = 3, // Specifies a printer's point (1/72 inch) as the unit of measure.
                Inch = 4,
                Document = 5, // Specifies the document unit (1/300 inch) as the unit of measure.
                Millimeter = 6,
            }
            */

            if (graphicsUnit == GraphicsUnit.Millimeter)
                return PdfSharp.Drawing.XGraphicsUnit.Millimeter;
            else if (graphicsUnit == GraphicsUnit.Inch)
                return PdfSharp.Drawing.XGraphicsUnit.Inch;
            else if (graphicsUnit == GraphicsUnit.Point)
                return PdfSharp.Drawing.XGraphicsUnit.Point;
            else if (graphicsUnit == GraphicsUnit.Pixel)
                return PdfSharp.Drawing.XGraphicsUnit.Point;

            return PdfSharp.Drawing.XGraphicsUnit.Point;
        }

        #endregion





#if AS_IMAGE
        private Graphics _innerGraphics;
#else
        private PdfSharp.Drawing.XGraphics _innerGraphics;
        System.Drawing.Image img;
        System.Drawing.Graphics ggg;
#endif



        private Stack<ISvgBoundable> _boundables = new Stack<ISvgBoundable>();

        public void SetBoundable(ISvgBoundable boundable)
        {
            _boundables.Push(boundable);
        }

        public ISvgBoundable GetBoundable()
        {
            return _boundables.Peek();
        }

        public ISvgBoundable PopBoundable()
        {
            return _boundables.Pop();
        }


        public float DpiY
        {
            get
            {
#if AS_IMAGE
                return _innerGraphics.DpiY;
#else
                return 300.0f;
#endif
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ISvgRenderer"/> class.
        /// </summary>
        private SvgRenderer(Graphics graphics)
        {
#if AS_IMAGE
            this._innerGraphics = graphics;
#endif
        }

        public double m_width;
        public double m_height;



        private SvgRenderer(PdfSharp.Drawing.XGraphics graphics, double width, double height)
        {
#if !AS_IMAGE
            this.m_width = width;
            this.m_height = height;
            this._innerGraphics = graphics;

            img = new Bitmap((int)width, (int)height);
            ggg = System.Drawing.Graphics.FromImage(img);
#endif
        }




        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
#if AS_IMAGE
            _innerGraphics.DrawImage(image, destRect, srcRect, graphicsUnit);
#else
            using (PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromGdiPlusImage(image))
            {
                _innerGraphics.DrawImage(img, ConvertRectangle(destRect), ConvertRectangle(srcRect), ConvertUnit(graphicsUnit));
            }
#endif
        }


        public void DrawImageUnscaled(Image image, Point location)
        {
#if AS_IMAGE
            this._innerGraphics.DrawImageUnscaled(image, location);
#else
            using (PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromGdiPlusImage(image))
            {
                this._innerGraphics.DrawImage(img, ConvertPoint(location));
            }
#endif
        }


        public void DrawPath(Pen pen, GraphicsPath path)
        {
#if AS_IMAGE
            this._innerGraphics.DrawPath(pen, path);
#else
            var p = ConvertPath(path);
            if (p != null)
                this._innerGraphics.DrawPath(ConvertPen(pen), p);
#endif
        }


        public void FillPath(Brush brush, GraphicsPath path)
        {
#if AS_IMAGE
            this._innerGraphics.FillPath(brush, path);
#else
            var p = ConvertPath(path);
            if (p != null)
                this._innerGraphics.DrawPath(ConvertBrush(brush), p);
#endif

        }


        public void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append)
        {
#if AS_IMAGE
            this._innerGraphics.RotateTransform(fAngle, order);
#else
            this._innerGraphics.RotateTransform((double)fAngle, (PdfSharp.Drawing.XMatrixOrder)order);
#endif
        }


        public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append)
        {
#if AS_IMAGE
            this._innerGraphics.ScaleTransform(sx, sy, order);
#else
            this._innerGraphics.ScaleTransform((double)sx, (double)sy, (PdfSharp.Drawing.XMatrixOrder)order);
#endif
        }



        public Region GetClip()
        {
#if AS_IMAGE
            return this._innerGraphics.Clip;
#else
            // System.Drawing.Graphics g = null;
            // System.Drawing.Region reg = null;

            //return this._innerGraphics.Clip;
            // throw new System.NotImplementedException("GetClip");

            // var rect = new System.Drawing.RectangleF(0.0f,0.0f, (float)this.m_width, (float)this.m_height);
            // return new System.Drawing.Region(rect);

            return this.m_clip;
#endif
        }


        private System.Drawing.Region m_clip;

        private System.Collections.Generic.Stack<PdfSharp.Drawing.XGraphicsState> m_ClipStates = new Stack<PdfSharp.Drawing.XGraphicsState>();

        public void SetClip(Region region, CombineMode combineMode = CombineMode.Replace)
        {
#if AS_IMAGE
            this._innerGraphics.SetClip(region, combineMode);
#else
            this.m_clip = region;

            // https://forums.getpaint.net/index.php?/topic/4767-gdi-questions-how-to-get-a-path-from-a-region/
            // https://searchcode.com/codesearch/view/1106323/

            if (region == null)
            {
                if (this.m_ClipStates.Count < 1)
                    return;

                var gss = this.m_ClipStates.Pop();
                if (gss != null)
                    this._innerGraphics.Restore(gss);

                return;
            }



            System.Drawing.RectangleF bnds = region.GetBounds(this.ggg);
            PdfSharp.Drawing.XRect rr = ConvertRectangle(bnds);

            var gs = this._innerGraphics.Save();
            this.m_ClipStates.Push(gs);
            this._innerGraphics.IntersectClip(rr);

            //System.Drawing.Drawing2D.Matrix unitmatrix = null;

            ////region.GetBounds()
            //System.Drawing.RectangleF[] rects = region.GetRegionScans(unitmatrix);


            // RegionData rd = region.GetRegionData();
            // System.Console.WriteLine(rd);

            //throw new NotImplementedException("SetClip");
#endif
        }


        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append)
        {
#if AS_IMAGE
            this._innerGraphics.TranslateTransform(dx, dy, order);
#else
            this._innerGraphics.TranslateTransform((double)dx, (double)dy, (PdfSharp.Drawing.XMatrixOrder)order);
#endif
        }


        public SmoothingMode SmoothingMode
        {
            get
            {
#if AS_IMAGE
                return this._innerGraphics.SmoothingMode;
#else
                return System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#endif
            }
            set
            {
#if AS_IMAGE
                this._innerGraphics.SmoothingMode = value;
#endif
            }
        }


        public Matrix Transform
        {
            get
            {
#if AS_IMAGE
                return this._innerGraphics.Transform;
#else
                return ConvertMatrix(this._innerGraphics.Transform);
#endif
            }

            set
            {
#if AS_IMAGE
                this._innerGraphics.Transform = value;
#else
                this._innerGraphics.Transform = ConvertMatrix(value);
#endif
            }
        }


        public void Dispose()
        {
            this._innerGraphics.Dispose();
        }


        Graphics IGraphicsProvider.GetGraphics()
        {
#if AS_IMAGE
            return _innerGraphics;
#else
            // throw new NotImplementedException("foo");
            // return null;
            return ggg;
#endif
        }


        public static void testtest()
        {
            PdfSharp.Drawing.XColor red = PdfSharp.Drawing.XColor.FromKnownColor(System.Drawing.KnownColor.Red);
            PdfSharp.Drawing.XRect rect1 = new PdfSharp.Drawing.XRect(0, 0, 200, 500);

            // PdfSharp.Drawing.XRect rect1 = new PdfSharp.Drawing.XRect(0, 0, svgWidth, svgHeight);


            PdfSharp.Drawing.XFont font = new PdfSharp.Drawing.XFont("Arial"
                    , 12.0, PdfSharp.Drawing.XFontStyle.Bold
            );


            using (PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument())
            {
                document.Info.Title = "Family Tree";
                document.Info.Author = "FamilyTree Ltd. - Stefan Steiger";
                document.Info.Subject = "Family Tree";
                document.Info.Keywords = "Family Tree, Genealogical Tree, Genealogy, Bloodline, Pedigree";

                PdfSharp.Pdf.PdfPage page = document.AddPage();
                page.Orientation = PdfSharp.PageOrientation.Landscape;


                using (PdfSharp.Drawing.XGraphics gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
                {
                    PdfSharp.Drawing.Layout.XTextFormatter tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
                    tf.Alignment = PdfSharp.Drawing.Layout.XParagraphAlignment.Left;

                    string text = "Hello world";
                    tf.DrawString(text
                                           , font
                                           , PdfSharp.Drawing.XBrushes.Black
                                           , rect1
                                           , PdfSharp.Drawing.XStringFormats.TopLeft
                               );


                    double dblLineWidth = 1.0;
                    PdfSharp.Drawing.XColor lineColor = Rendering.Pdf.XColorHelper.FromHtml("#FF00FF");
                    PdfSharp.Drawing.XPen pen = new PdfSharp.Drawing.XPen(lineColor, dblLineWidth);

                    double xNew = 0;
                    double yNew = 0;

                    gfx.DrawLine(pen, xNew, yNew + rect1.Height, rect1.X + rect1.Width / 2.0, rect1.Y);
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


                System.IO.File.WriteAllBytes("FamilyTree.pdf", baPdfDocument);
            }
        }



        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> from which to create the new <see cref="ISvgRenderer"/>.</param>
        public static ISvgRenderer FromImage(Image image)
        {
            var g = Graphics.FromImage(image);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.TextContrast = 1;

            return new SvgRenderer(g);
        }


        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to create the renderer from.</param>
        public static ISvgRenderer FromGraphics(Graphics graphics)
        {
            return new SvgRenderer(graphics);
        }


        public static ISvgRenderer FromPdf(PdfSharp.Pdf.PdfDocument doc)
        {
            PdfSharp.Pdf.PdfPage page = doc.Pages[0];
            PdfSharp.Drawing.XGraphics graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
            graphics.MUH = PdfSharp.Pdf.PdfFontEncoding.Unicode;

            return new SvgRenderer(graphics, page.Width.Point, page.Height.Point);
        }


        public static ISvgRenderer FromPdfGraphics(Graphics graphics)
        {
            return new SvgRenderer(graphics);
        }


        public static ISvgRenderer FromNull()
        {
            var img = new Bitmap(1, 1);
            return SvgRenderer.FromImage(img);
        }


    }


}
