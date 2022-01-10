#if !NO_SDC
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using Svg.Exceptions;

namespace Svg
{
    public partial class SvgDocument : SvgFragment
    {
        /// <summary>
        /// Skip check whether the GDI+ can be loaded.
        /// </summary>
        /// <remarks>
        /// Set to true on systems that do not support GDI+ like UWP.
        /// </remarks>
        public static bool SkipGdiPlusCapabilityCheck { get; set; }

        internal SvgFontManager FontManager { get; private set; }

        /// <summary>
        /// Validate whether the system has GDI+ capabilities (non Windows related).
        /// </summary>
        /// <returns>Boolean whether the system is capable of using GDI+</returns>
        public static bool SystemIsGdiPlusCapable()
        {
            try
            {
                EnsureSystemIsGdiPlusCapable();
            }
            catch (SvgGdiPlusCannotBeLoadedException)
            {
                return false;
            }
            catch (Exception)
            {
                // If somehow another type of exception is raised by the ensure function we will let it bubble up, since that might indicate other issues/problems
                throw;
            }
            return true;
        }

        /// <summary>
        /// Ensure that the running system is GDI capable, if not this will yield a
        /// SvgGdiPlusCannotBeLoadedException exception.
        /// </summary>
        public static void EnsureSystemIsGdiPlusCapable()
        {
            try
            {
                using (var matrix = new Matrix(0f, 0f, 0f, 0f, 0f, 0f)) { }
            }
            // GDI+ loading errors will result in TypeInitializationExceptions,
            // for readability we will catch and wrap the error
            catch (Exception e)
            {
                if (ExceptionCaughtIsGdiPlusRelated(e))
                {
                    // Throw only the customized exception if we are sure GDI+ is causing the problem
                    throw new SvgGdiPlusCannotBeLoadedException(e);
                }
                // If the Matrix creation is causing another type of exception we should just raise that one
                throw;
            }
        }

        /// <summary>
        /// Check if the current exception or one of its children is the targeted GDI+ exception.
        /// It can be hidden in one of the InnerExceptions, so we need to iterate over them.
        /// </summary>
        /// <param name="e">The exception to validate against the GDI+ check</param>
        private static bool ExceptionCaughtIsGdiPlusRelated(Exception e)
        {
            var currE = e;
            int cnt = 0; // Keep track of depth to prevent endless-loops
            while (currE != null && cnt < 10)
            {
                var typeException = currE as DllNotFoundException;
                if (typeException?.Message?.LastIndexOf("libgdiplus", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    return true;
                }
                currE = currE.InnerException;
                cnt++;
            }
            return false;
        }

        public static Bitmap OpenAsBitmap(string path)
        {
            return null;
        }

        public static Bitmap OpenAsBitmap(XmlDocument document)
        {
            return null;
        }

        private void Draw(ISvgRenderer renderer, ISvgBoundable boundable)
        {
            using (FontManager = new SvgFontManager())
            {
                renderer.SetBoundable(boundable);
                Render(renderer);
                FontManager = null;
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to render the document with.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="renderer"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(ISvgRenderer renderer)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }

            this.Draw(renderer, this);
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to be rendered to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="graphics"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(Graphics graphics)
        {
            this.Draw(graphics, null);
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to be rendered to.</param>
        /// <param name="size">The <see cref="SizeF"/> to render the document. If <c>null</c> document is rendered at the default document size.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="graphics"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(Graphics graphics, SizeF? size)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }

            using (var renderer = SvgRenderer.FromGraphics(graphics))
            {
                var boundable = size.HasValue ? (ISvgBoundable)new GenericBoundable(0, 0, size.Value.Width, size.Value.Height) : this;
                this.Draw(renderer, boundable);
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> and returns the image as a <see cref="Bitmap"/>.
        /// </summary>
        /// <returns>A <see cref="Bitmap"/> containing the rendered document.</returns>
        public virtual Bitmap Draw()
        {
            //Trace.TraceInformation("Begin Render");

            var size = Size.Round(GetDimensions());
            if (size.Width <= 0 || size.Height <= 0)
                return null;

            Bitmap bitmap = null;
            try
            {
                try
                {
                    bitmap = new Bitmap(size.Width, size.Height);
                }
                catch (ArgumentException e)
                {
                    // When processing too many files at one the system can run out of memory
                    throw new SvgMemoryException("Cannot process SVG file, cannot allocate the required memory", e);
                }

                //bitmap.SetResolution(300, 300);

                this.Draw(bitmap);
            }
            catch
            {
                bitmap?.Dispose();
                throw;
            }

            //Trace.TraceInformation("End Render");
            return bitmap;
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> into a given Bitmap <see cref="Bitmap"/>.
        /// </summary>
        public virtual void Draw(Bitmap bitmap)
        {
            //Trace.TraceInformation("Begin Render");

            using (var renderer = SvgRenderer.FromImage(bitmap))
            {
                var boundable = new GenericBoundable(0, 0, bitmap.Width, bitmap.Height);
                this.Draw(renderer, boundable);
            }

            //Trace.TraceInformation("End Render");
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> in given size and returns the image as a <see cref="Bitmap"/>.
        /// If one of rasterWidth and rasterHeight is zero, the image is scaled preserving aspect ratio,
        /// otherwise the aspect ratio is ignored.
        /// </summary>
        /// <returns>A <see cref="Bitmap"/> containing the rendered document.</returns>
        public virtual Bitmap Draw(int rasterWidth, int rasterHeight)
        {
            var svgSize = GetDimensions();
            var imageSize = svgSize;
            this.RasterizeDimensions(ref imageSize, rasterWidth, rasterHeight);

            var bitmapSize = Size.Round(imageSize);
            if (bitmapSize.Width <= 0 || bitmapSize.Height <= 0)
                return null;

            Bitmap bitmap = null;
            try
            {
                try
                {
                    bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
                }
                catch (ArgumentException e)
                {
                    // When processing too many files at one the system can run out of memory
                    throw new SvgMemoryException("Cannot process SVG file, cannot allocate the required memory", e);
                }

                using (var renderer = SvgRenderer.FromImage(bitmap))
                {
                    renderer.ScaleTransform(imageSize.Width / svgSize.Width, imageSize.Height / svgSize.Height);
                    var boundable = new GenericBoundable(0, 0, svgSize.Width, svgSize.Height);
                    this.Draw(renderer, boundable);
                }
            }
            catch
            {
                bitmap?.Dispose();
                throw;
            }

            return bitmap;
        }
    }
}
#endif
