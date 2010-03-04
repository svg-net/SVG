using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Xml;

namespace Svg
{
    /// <summary>
    /// The class used to create and load SVG documents.
    /// </summary>
    public class SvgDocument : SvgFragment, ITypeDescriptorContext
    {
        public static readonly int PPI = 96;

        /// <summary>
        /// Gets a <see cref="string"/> containing the XLink namespace (http://www.w3.org/1999/xlink).
        /// </summary>
        public static readonly string XLinkNamespace = "http://www.w3.org/1999/xlink";

        private SvgElementIdManager _idManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgDocument"/> class.
        /// </summary>
        public SvgDocument()
        {
            Ppi = 96;
        }

        /// <summary>
        /// Gets an <see cref="SvgElementIdManager"/> for this document.
        /// </summary>
        protected internal virtual SvgElementIdManager IdManager
        {
            get
            {
                if (_idManager == null)
                {
                    _idManager = new SvgElementIdManager(this);
                }

                return _idManager;
            }
        }

        /// <summary>
        /// Gets or sets the Pixels Per Inch of the rendered image.
        /// </summary>
        public int Ppi { get; set; }

        #region ITypeDescriptorContext Members

        IContainer ITypeDescriptorContext.Container
        {
            get { throw new NotImplementedException(); }
        }

        object ITypeDescriptorContext.Instance
        {
            get { return this; }
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {
            throw new NotImplementedException();
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get { throw new NotImplementedException(); }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Retrieves the <see cref="SvgElement"/> with the specified ID.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID of the element to find.</param>
        /// <returns>An <see cref="SvgElement"/> of one exists with the specified ID; otherwise false.</returns>
        public virtual SvgElement GetElementById(string id)
        {
            return IdManager.GetElementById(id);
        }

        /// <summary>
        /// Retrieves the <see cref="SvgElement"/> with the specified ID.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID of the element to find.</param>
        /// <returns>An <see cref="SvgElement"/> of one exists with the specified ID; otherwise false.</returns>
        public virtual TSvgElement GetElementById<TSvgElement>(string id) where TSvgElement : SvgElement
        {
            return (this.GetElementById(id) as TSvgElement);
        }

        /// <summary>
        /// Opens the document at the specified path and loads the SVG contents.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the path of the file to open.</param>
        /// <returns>An <see cref="SvgDocument"/> with the contents loaded.</returns>
        /// <exception cref="FileNotFoundException">The document at the specified <paramref name="path"/> cannot be found.</exception>
        public static SvgDocument Open(string path)
        {
            return Open(path, null);
        }

        /// <summary>
        /// Opens the document at the specified path and loads the SVG contents.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the path of the file to open.</param>
        /// <param name="entities">A dictionary of custom entity definitions to be used when resolving XML entities within the document.</param>
        /// <returns>An <see cref="SvgDocument"/> with the contents loaded.</returns>
        /// <exception cref="FileNotFoundException">The document at the specified <paramref name="path"/> cannot be found.</exception>
        public static SvgDocument Open(string path, Dictionary<string, string> entities)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified document cannot be found.", path);
            }

            return Open(File.OpenRead(path), entities);
        }

        /// <summary>
        /// Attempts to open an SVG document from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the SVG document to open.</param>
        public static SvgDocument Open(Stream stream)
        {
            return Open(stream, null);
        }

        /// <summary>
        /// Opens an SVG document from the specified <see cref="Stream"/> and adds the specified entities.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the SVG document to open.</param>
        /// <param name="entities">Custom entity definitions.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> parameter cannot be <c>null</c>.</exception>
        public static SvgDocument Open(Stream stream, Dictionary<string, string> entities)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            //Trace.TraceInformation("Begin Read");

            using (var reader = new SvgTextReader(stream, entities))
            {
                var elementStack = new Stack<SvgElement>();
                var value = new StringBuilder();
                bool elementEmpty;
                SvgElement element = null;
                SvgElement parent;
                SvgDocument svgDocument = null;
                reader.XmlResolver = new SvgDtdResolver();
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while (reader.Read())
                {
                    try
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                // Does this element have a value or children
                                // (Must do this check here before we progress to another node)
                                elementEmpty = reader.IsEmptyElement;
                                // Create element
                                if (elementStack.Count > 0)
                                {
                                    element = SvgElementFactory.CreateElement(reader, svgDocument);
                                }
                                else
                                {
                                    element = SvgElementFactory.CreateDocument(reader);
                                    svgDocument = (SvgDocument)element;
                                }

                                if (element == null)
                                {
                                    continue;
                                }

                                // Add to the parents children
                                if (elementStack.Count > 0)
                                {
                                    parent = elementStack.Peek();
                                    parent.Children.Add(element);
                                }

                                // Push element into stack
                                elementStack.Push(element);

                                // Need to process if the element is empty
                                if (elementEmpty)
                                {
                                    goto case XmlNodeType.EndElement;
                                }

                                break;
                            case XmlNodeType.EndElement:
                                // Skip if no element was created and is not the closing tag for the last
                                // known element
                                if (element == null && reader.LocalName != elementStack.Peek().ElementName)
                                {
                                    continue;
                                }
                                // Pop the element out of the stack
                                element = elementStack.Pop();

                                if (value.Length > 0)
                                {
                                    element.Content = value.ToString();
                                    // Reset content value for new element
                                    value = new StringBuilder();
                                }
                                break;
                            case XmlNodeType.CDATA:
                            case XmlNodeType.Text:
                                value.Append(reader.Value);
                                break;
                        }
                    }
                    catch (Exception exc)
                    {
                        Trace.TraceError(exc.Message);
                    }
                }

                //Trace.TraceInformation("End Read");
                return svgDocument;
            }
        }

        /// <summary>
        /// Opens an SVG document from the specified <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="document">The <see cref="XmlDocument"/> containing the SVG document XML.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="document"/> parameter cannot be <c>null</c>.</exception>
        public static SvgDocument Open(XmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            Stream stream = new MemoryStream(UTF8Encoding.Default.GetBytes(document.InnerXml));
            return Open(stream, null);
        }

        public static Bitmap OpenAsBitmap(string path)
        {
            return null;
        }

        public static Bitmap OpenAsBitmap(XmlDocument document)
        {
            return null;
        }

        public RectangleF GetDimensions()
        {
            return new RectangleF(0, 0, Width.ToDeviceValue(), Height.ToDeviceValue());
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to render the document with.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="renderer"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(SvgRenderer renderer)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }

            this.Render(renderer);
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to be rendered to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="graphics"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }

            this.Render(SvgRenderer.FromGraphics(graphics));
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> and returns the image as a <see cref="Bitmap"/>.
        /// </summary>
        /// <returns>A <see cref="Bitmap"/> containing the rendered document.</returns>
        public virtual Bitmap Draw()
        {
            //Trace.TraceInformation("Begin Render");

            var size = GetDimensions();
            var bitmap = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));

            try
            {
                using (var renderer = SvgRenderer.FromImage(bitmap))
                {
                    renderer.TextRenderingHint = TextRenderingHint.AntiAlias;
                    renderer.TextContrast = 1;
                    renderer.PixelOffsetMode = PixelOffsetMode.Half;
                    this.Render(renderer);
                    renderer.Save();
                }
            }
            catch
            {
                bitmap.Dispose();
                throw;
            }

            //Trace.TraceInformation("End Render");
            return bitmap;
        }

        public void Write(Stream stream)
        {

        }

        public void Write(string path)
        {

        }
    }
}