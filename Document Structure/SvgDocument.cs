using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Xml.Schema;

namespace Svg
{
    /// <summary>
    /// The class used to create and load all SVG documents.
    /// </summary>
    public class SvgDocument : SvgFragment, ITypeDescriptorContext
    {
        private SvgElementIdManager _idManager;
        private int _ppi;
        public static readonly int PPI = 96;
        /// <summary>
        /// Gets a <see cref="string"/> containing the XLink namespace (http://www.w3.org/1999/xlink).
        /// </summary>
        public static readonly string XLinkNamespace = "http://www.w3.org/1999/xlink";

        /// <summary>
        /// Retrieves the <see cref="SvgElement"/> with the specified ID.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID of the element to find.</param>
        /// <returns>An <see cref="SvgElement"/> of one exists with the specified ID; otherwise false.</returns>
        public virtual SvgElement GetElementById(string id)
        {
            return this.IdManager.GetElementById(id);
        }

        /// <summary>
        /// Gets an <see cref="SvgElementIdManager"/> for this document.
        /// </summary>
        protected internal virtual SvgElementIdManager IdManager
        {
            get
            {
                if (this._idManager == null)
                {
                    this._idManager = new SvgElementIdManager(this);
                }

                return this._idManager;
            }
        }

        public int Ppi
        {
            get { return this._ppi; }
            set { this._ppi = value; }
        }

        public SvgDocument()
        {
            this._ppi = 96;
        }

        /// <summary>
        /// Opens the document at the specified path and loads the contents.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the path of the file to open.</param>
        /// <returns>An <see cref="SvgDocument"/> with the contents loaded.</returns>
        /// <exception cref="FileNotFoundException">The document at the specified <paramref name="path"/> cannot be found.</exception>
        public static SvgDocument Open(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified document cannot be found.", path);
            }

            return Open(File.OpenRead(path));
        }

        public static SvgDocument Open(Stream stream)
        {
            return Open(stream, null);
        }

        public static SvgDocument OpenRender(Stream stream, Graphics graphics)
        {
            return null;
        }

        public static SvgDocument Open(Stream stream, ValidationEventHandler validationEventHandler)
        {
            Trace.TraceInformation("Begin Read");

            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                Stack<SvgElement> elementStack = new Stack<SvgElement>();
                StringBuilder value = new StringBuilder();
                SvgElement element = null;
                SvgElement parent = null;
                SvgDocument svgDocument = null;
                reader.XmlResolver = new SvgDtdResolver();
                reader.EntityHandling = EntityHandling.ExpandEntities;
                bool isEmpty;

                // Don't need it
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
                                isEmpty = reader.IsEmptyElement;
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
                                    continue;

                                // Add to the parents children
                                if (elementStack.Count > 0)
                                {
                                    parent = elementStack.Peek();
                                    parent.Children.Add(element);
                                }

                                // Push element into stack
                                elementStack.Push(element);

                                // Need to process if the element is empty
                                if (isEmpty)
                                    goto case XmlNodeType.EndElement;

                                break;
                            case XmlNodeType.EndElement:
                                // Skip if no element was created
                                if (element == null)
                                    continue;
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

                Trace.TraceInformation("End Read");
                return svgDocument;
            }
        }

        public static SvgDocument Open(System.Xml.XmlDocument document)
        {
            return null;
        }

        public static Bitmap OpenAsBitmap(string path)
        {
            return null;
        }

        public static Bitmap OpenAsBitmap(System.Xml.XmlDocument document)
        {
            return null;
        }

        public RectangleF GetDimensions()
        {
            return new RectangleF(0, 0, this.Width.ToDeviceValue(), this.Height.ToDeviceValue());
            return new RectangleF();
        }

        public void Draw(Graphics graphics)
        {
            this.Render(graphics);
        }

        public virtual Bitmap Draw()
        {
            Trace.TraceInformation("Begin Render");

            RectangleF size = this.GetDimensions();
            Bitmap bitmap = new Bitmap((int)this.Width.ToDeviceValue(), (int)this.Height.ToDeviceValue());

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.TextContrast = 0;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                this.Render(g);
                g.Save();
            }

            Trace.TraceInformation("End Render");
            return bitmap;
        }

        public void Write(Stream stream)
        {
            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {
            }
        }

        public void Write(string path)
        {
            this.Write(File.Create(path));
        }

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
    }
}