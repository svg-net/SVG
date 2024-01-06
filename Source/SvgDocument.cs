using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using Svg.Css;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using ExCSS;

namespace Svg
{
    /// <summary>
    /// The class used to create and load SVG documents.
    /// </summary>
    public partial class SvgDocument : SvgFragment, ITypeDescriptorContext
    {
        /// <summary>
        /// Skip the Dtd Processing for faster loading of svgs that have a DTD specified.
        /// For Example Adobe Illustrator svgs.
        /// </summary>
        public static bool DisableDtdProcessing { get; set; }

        /// <summary>
        /// Which types of XML external entities are allowed to be resolved. Defaults to <see cref="ExternalType.None"/> to prevent XXE.
        /// </summary>
        /// <see ref="https://owasp.org/www-community/vulnerabilities/XML_External_Entity_(XXE)_Processing"/>
        public static ExternalType ResolveExternalXmlEntites { get; set; } = ExternalType.None;

        /// <summary>
        /// Which types of external images are allowed to be resolved. Defaults to <see cref="ExternalType.Local"/> and <see cref="ExternalType.Remote"/>.
        /// </summary>
        public static ExternalType ResolveExternalImages { get; set; } = ExternalType.Local | ExternalType.Remote;

        /// <summary>
        /// Which types of external elements, for example text definitions, are allowed to be resolved. Defaults to <see cref="ExternalType.Local"/> and <see cref="ExternalType.Remote"/>.
        /// </summary>
        public static ExternalType ResolveExternalElements { get; set; } = ExternalType.Local | ExternalType.Remote;

        private static int? pointsPerInch;

        public static int PointsPerInch
        {
            get { return pointsPerInch ?? (int)(pointsPerInch = GetSystemDpi()); }
            set { pointsPerInch = value; }
        }

        private SvgElementIdManager _idManager;

        private Dictionary<string, IEnumerable<SvgFontFace>> _fontDefns = null;

        public override SvgUnit X
        {
            get { return 0f; }
        }

        public override SvgUnit Y
        {
            get { return 0f; }
        }

        public override SvgOverflow Overflow
        {
            get { return GetAttribute("overflow", false, SvgOverflow.Visible); }
        }

        private static int GetSystemDpi()
        {
            bool isWindows;

#if NETCOREAPP
            isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
            var platform = Environment.OSVersion.Platform;
            isWindows = platform == PlatformID.Win32NT;
#endif

            if (isWindows)
            {
                return GetWin32SystemDpi();
            }
            else
            {
                // hack for macOS and Linux
                return 96;
            }
        }

        internal Dictionary<string, IEnumerable<SvgFontFace>> FontDefns()
        {
            if (_fontDefns == null)
            {
                _fontDefns = (from f in Descendants().OfType<SvgFontFace>()
                              group f by f.FontFamily into family
                              select family).ToDictionary(f => f.Key, f => (IEnumerable<SvgFontFace>)f);
            }
            return _fontDefns;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgDocument"/> class.
        /// </summary>
        public SvgDocument()
        {
            Ppi = PointsPerInch;

            Namespaces.Add(string.Empty, SvgNamespaces.SvgNamespace);
            Namespaces.Add(SvgNamespaces.XLinkPrefix, SvgNamespaces.XLinkNamespace);
            Namespaces.Add(SvgNamespaces.XmlPrefix, SvgNamespaces.XmlNamespace);
        }

        private Uri baseUri;

        public Uri BaseUri
        {
            get { return baseUri; }
            set
            {
                if (value != null && !value.IsAbsoluteUri)
                    throw new ArgumentException("BaseUri is not absolute.");
                baseUri = value;
            }
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
        /// Overwrites the current IdManager with a custom implementation.
        /// Be careful with this: If elements have been inserted into the document before,
        /// you have to take care that the new IdManager also knows of them.
        /// </summary>
        /// <param name="manager"></param>
        public void OverwriteIdManager(SvgElementIdManager manager)
        {
            _idManager = manager;
        }

        /// <summary>
        /// Gets or sets the Pixels Per Inch of the rendered image.
        /// </summary>
        public int Ppi { get; set; }

        /// <summary>
        /// Gets or sets an external Cascading Style Sheet (CSS)
        /// </summary>
        public string ExternalCSSHref { get; set; }

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
            return Open<SvgDocument>(path, new SvgOptions());
        }

        /// <summary>
        /// Opens the document at the specified path and loads the SVG contents.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the path of the file to open.</param>
        /// <returns>An <see cref="SvgDocument"/> with the contents loaded.</returns>
        /// <exception cref="FileNotFoundException">The document at the specified <paramref name="path"/> cannot be found.</exception>
        public static T Open<T>(string path) where T : SvgDocument, new()
        {
            return Open<T>(path, new SvgOptions());
        }

        /// <summary>
        /// Opens the document at the specified path and loads the SVG contents.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the path of the file to open.</param>
        /// <param name="entities">A dictionary of custom entity definitions to be used when resolving XML entities within the document.</param>
        /// <returns>An <see cref="SvgDocument"/> with the contents loaded.</returns>
        /// <exception cref="FileNotFoundException">The document at the specified <paramref name="path"/> cannot be found.</exception>
        public static T Open<T>(string path, Dictionary<string, string> entities) where T : SvgDocument, new()
        {
            return Open<T>(path, new SvgOptions(entities));
        }

        /// <summary>
        /// Opens the document at the specified path and loads the SVG contents.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the path of the file to open.</param>
        /// <param name="svgOptions">A dictionary of custom entity definitions to be used when resolving XML entities within the document.</param>
        /// <returns>A <see cref="SvgDocument"/> with the contents loaded.</returns>
        /// <exception cref="FileNotFoundException">The document at the specified <paramref name="path"/> cannot be found.</exception>
        public static T Open<T>(string path, SvgOptions svgOptions) where T : SvgDocument, new()
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified document cannot be found.", path);
            }

            using (var stream = File.OpenRead(path))
            {
                var doc = Open<T>(stream, svgOptions);
                doc.BaseUri = new Uri(System.IO.Path.GetFullPath(path));
                return doc;
            }
        }

        /// <summary>
        /// Attempts to open an SVG document from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the SVG document to open.</param>
        public static T Open<T>(Stream stream) where T : SvgDocument, new()
        {
            return Open<T>(stream, new SvgOptions());
        }

        /// <summary>
        /// Opens an SVG document from the specified <see cref="Stream"/> and adds the specified entities.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the SVG document to open.</param>
        /// <param name="entities">Custom entity definitions.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> parameter cannot be <c>null</c>.</exception>
        public static T Open<T>(Stream stream, Dictionary<string, string> entities)
            where T : SvgDocument, new()
        {
            return Open<T>(stream, new SvgOptions(entities));
        }

        /// <summary>
        /// Opens an SVG document from the specified <see cref="Stream"/> and adds the specified entities.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the SVG document to open.</param>
        /// <param name="svgOptions">Css Style that will be applied to the Svg Document</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> parameter cannot be <c>null</c>.</exception>
        public static T Open<T>(Stream stream, SvgOptions svgOptions) where T : SvgDocument, new()
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            // Don't close the stream via a dispose: that is the client's job.
            var reader = new SvgTextReader(stream, svgOptions.Entities)
            {
                XmlResolver = new SvgDtdResolver(),
                WhitespaceHandling = WhitespaceHandling.Significant,
                DtdProcessing = DisableDtdProcessing ? DtdProcessing.Ignore : DtdProcessing.Parse,
            };
            return Create<T>(reader, svgOptions.Css);
        }

        /// <summary>
        /// Attempts to create an SVG document from the specified string data.
        /// </summary>
        /// <param name="svg">The SVG data.</param>
        public static T FromSvg<T>(string svg) where T : SvgDocument, new()
        {
            if (string.IsNullOrEmpty(svg))
            {
                throw new ArgumentNullException("svg");
            }

            using (var strReader = new StringReader(svg))
            {
                var reader = new SvgTextReader(strReader, null)
                {
                    XmlResolver = new SvgDtdResolver(),
                    WhitespaceHandling = WhitespaceHandling.Significant,
                    DtdProcessing = DisableDtdProcessing ? DtdProcessing.Ignore : DtdProcessing.Parse,
                };
                return Create<T>(reader);
            }
        }

        /// <summary>
        /// Attempts to open an SVG document from the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> containing the SVG document to open.</param>
        public static T Open<T>(XmlReader reader) where T : SvgDocument, new()
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            using (var svgReader = XmlReader.Create(reader, new XmlReaderSettings()
            {
                XmlResolver = new SvgDtdResolver(),
                DtdProcessing = DtdProcessing.Parse,
            }))
            {
                return Create<T>(svgReader);
            }
        }

        private static T Create<T>(XmlReader reader, string css = null) where T : SvgDocument, new()
        {
            var styles = new List<ISvgNode>();
            var elementFactory = new SvgElementFactory();

            var svgDocument = Create<T>(reader, elementFactory, styles);
            
            if (css != null) {
                styles.Add(new SvgUnknownElement() { Content = css });
            }

            if (styles.Any())
            {
                var cssTotal = string.Join(Environment.NewLine, styles.Select(s => s.Content).ToArray());
                var stylesheetParser = new StylesheetParser(true, true, tolerateInvalidValues: true);
                var stylesheet = stylesheetParser.Parse(cssTotal);

                foreach (var rule in stylesheet.StyleRules)
                    try
                    {
                        var rootNode = new NonSvgElement();
                        rootNode.Children.Add(svgDocument);

                        var elemsToStyle = rootNode.QuerySelectorAll(rule.Selector, elementFactory);
                        foreach (var elem in elemsToStyle)
                            foreach (var declaration in rule.Style)
                            {
                                elem.AddStyle(declaration.Name, declaration.Original, rule.Selector.GetSpecificity());
                            }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceWarning(ex.Message);
                    }
            }

            svgDocument?.FlushStyles(true);
            return svgDocument;
        }

        /// <summary> Open Svg Document without applying Stylesheets. </summary>
        /// <typeparam name="T">SvgDocument to create</typeparam>
        /// <param name="reader">reader</param>
        /// <param name="elementFactory">elementFactory</param>
        /// <param name="styles">read svg StyleSheets</param>
        /// <returns>Created Svg Document</returns>
        internal static T Create<T>(XmlReader reader, SvgElementFactory elementFactory, List<ISvgNode> styles)
            where T : SvgDocument, new()
        {
#if !NO_SDC
            if (!SkipGdiPlusCapabilityCheck)
            {
                EnsureSystemIsGdiPlusCapable(); // Validate whether the GDI+ can be loaded, this will yield an exception if not
            }
#endif
            var elementStack = new Stack<SvgElement>();
            bool elementEmpty;
            SvgElement element = null;
            SvgElement parent;
            T svgDocument = null;

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
                                element = elementFactory.CreateElement(reader, svgDocument);
                            }
                            else
                            {
                                svgDocument = elementFactory.CreateDocument<T>(reader);
                                element = svgDocument;
                            }

                            // Add to the parents children
                            if (elementStack.Count > 0)
                            {
                                parent = elementStack.Peek();
                                if (parent != null && element != null)
                                {
                                    parent.Children.Add(element);
                                    parent.Nodes.Add(element);
                                }
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

                            // Pop the element out of the stack
                            element = elementStack.Pop();

                            if (element.Nodes.OfType<SvgContentNode>().Any())
                            {
                                element.Content = string.Concat((from n in element.Nodes select n.Content).ToArray());
                            }
                            else
                            {
                                element.Nodes.Clear(); // No sense wasting the space where it isn't needed
                            }

                            var unknown = element as SvgUnknownElement;
                            if (unknown != null && unknown.ElementName == "style")
                            {
                                styles.Add(unknown);
                            }
                            break;
                        case XmlNodeType.CDATA:
                        case XmlNodeType.Text:
                        case XmlNodeType.SignificantWhitespace:
                            element = elementStack.Peek();
                            element.Nodes.Add(new SvgContentNode() { Content = reader.Value });
                            break;
                        case XmlNodeType.EntityReference:
                            reader.ResolveEntity();
                            element = elementStack.Peek();
                            element.Nodes.Add(new SvgContentNode() { Content = reader.Value });
                            break;
                    }
                }
                catch (Exception exc)
                {
                    Trace.TraceError(exc.Message);
                }
            }

            return svgDocument;
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

            var reader = new SvgNodeReader(document.DocumentElement, null);
            return Create<SvgDocument>(reader);
        }

        /// <summary>
        /// If both or one of raster height and width is not given (0), calculate that missing value from original SVG size
        /// while keeping original SVG size ratio
        /// </summary>
        /// <param name="size"></param>
        /// <param name="rasterWidth"></param>
        /// <param name="rasterHeight"></param>
        public virtual void RasterizeDimensions(ref SizeF size, int rasterWidth, int rasterHeight)
        {
            if (size.Width == 0)
                return;

            // Ratio of height/width of the original SVG size, to be used for scaling transformation
            float ratio = size.Height / size.Width;

            size.Width = rasterWidth > 0 ? rasterWidth : size.Width;
            size.Height = rasterHeight > 0 ? rasterHeight : size.Height;

            if (rasterHeight == 0 && rasterWidth > 0)
            {
                size.Height = (int)(rasterWidth * ratio);
            }
            else if (rasterHeight > 0 && rasterWidth == 0)
            {
                size.Width = (int)(rasterHeight / ratio);
            }
        }

        public override void Write(XmlWriter writer)
        {
            // Save previous culture and switch to invariant for writing
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                base.Write(writer);
            }
            finally
            {
                // Make sure to set back the old culture even an error occurred.
                // Switch culture back
                Thread.CurrentThread.CurrentCulture = previousCulture;
            }
        }

        public void Write(Stream stream, bool useBom = true)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = useBom ? Encoding.UTF8 : new UTF8Encoding(false),
                Indent = true
            };

            using var xmlWriter = XmlWriter.Create(stream, settings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);

            if (!string.IsNullOrEmpty(this.ExternalCSSHref))
                xmlWriter.WriteProcessingInstruction("xml-stylesheet", string.Format("type=\"text/css\" href=\"{0}\"", this.ExternalCSSHref));

            this.Write(xmlWriter);

            xmlWriter.Flush();
        }

        public void Write(string path, bool useBom = true)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                this.Write(fs, useBom);
            }
        }

        protected override void WriteAttributes(XmlWriter writer)
        {
            writer.WriteAttributeString("version", "1.1");
            base.WriteAttributes(writer);
        }
    }
}
