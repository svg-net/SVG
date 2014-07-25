using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Svg.DataTypes;
using System.Linq;

namespace Svg
{
    public enum XmlSpaceHandling
    {
        @default,
        preserve
    }
    
    public abstract class SvgTextBase : SvgVisualElement
    {
        private SvgUnit _x;
        private SvgUnit _y;
        private SvgUnit _dy;
        private SvgUnit _dx;
        private SvgUnit _letterSpacing;
        private SvgUnit _wordSpacing;
        private SvgUnit _fontSize;
		private SvgFontWeight _fontWeight;
        private string _font;
        protected string _fontFamily;
        private SvgTextAnchor _textAnchor = SvgTextAnchor.Start;
        private static readonly SvgRenderer _stringMeasure;
        private const string DefaultFontFamily = "Times New Roman";
        
        private XmlSpaceHandling _space = XmlSpaceHandling.@default;

        /// <summary>
        /// Initializes the <see cref="SvgTextBase"/> class.
        /// </summary>
        static SvgTextBase()
        {
            Bitmap bitmap = new Bitmap(1, 1);
            _stringMeasure = SvgRenderer.FromImage(bitmap);
            _stringMeasure.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgTextBase"/> class.
        /// </summary>
        public SvgTextBase()
        {
            this._fontSize = new SvgUnit(0.0f);
            this._dy = new SvgUnit(0.0f);
            this._dx = new SvgUnit(0.0f);
        }
        
        /// <summary>
        /// Gets or sets the text to be rendered.
        /// </summary>
        public virtual string Text
        {
            get { return base.Content; }
            set { base.Content = value; this.IsPathDirty = true; this.Content = value; }
        }

        /// <summary>
        /// Gets or sets the text anchor.
        /// </summary>
        /// <value>The text anchor.</value>
        [SvgAttribute("text-anchor")]
        public virtual SvgTextAnchor TextAnchor
        {
            get { return this._textAnchor; }
            set { this._textAnchor = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
        	get { return this._x; }
        	set
        	{
        		if(_x != value)
        		{
        			this._x = value;
        			this.IsPathDirty = true;
        			OnAttributeChanged(new AttributeEventArgs{ Attribute = "x", Value = value });
        		}
        	}
        }

        /// <summary>
        /// Gets or sets the dX.
        /// </summary>
        /// <value>The dX.</value>
        [SvgAttribute("dx")]
        public virtual SvgUnit Dx
        {
            get { return this._dx; }
            set
            {
                if (_dx != value)
                {
                    this._dx = value;
                    this.IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "dx", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
        	get { return this._y; }
        	set
        	{
        		if(_y != value)
        		{
        			this._y = value;
        			this.IsPathDirty = true;
        			OnAttributeChanged(new AttributeEventArgs{ Attribute = "y", Value = value });
        		}
        	}
        }

        /// <summary>
        /// Gets or sets the dY.
        /// </summary>
        /// <value>The dY.</value>
        [SvgAttribute("dy")]
        public virtual SvgUnit Dy
        {
            get { return this._dy; }
            set
            {
                if (_dy != value)
                {
                    this._dy = value;
                    this.IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "dy", Value = value });
                }
            }
        }

        /// <summary>
        /// Specifies spacing behavior between text characters.
        /// </summary>
        [SvgAttribute("letter-spacing")]
        public virtual SvgUnit LetterSpacing
        {
            get { return this._letterSpacing; }
            set { this._letterSpacing = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Specifies spacing behavior between words.
        /// </summary>
        [SvgAttribute("word-spacing")]
        public virtual SvgUnit WordSpacing
        {
            get { return this._wordSpacing; }
            set { this._wordSpacing = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public virtual string FontFamily
        {
            get { return this._fontFamily ?? DefaultFontFamily; }
            set
            {
                this._fontFamily = ValidateFontFamily(value);
                this.IsPathDirty = true;
            }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public virtual SvgUnit FontSize
        {
            get { return this._fontSize; }
            set { this._fontSize = value; this.IsPathDirty = true; }
        }


		/// <summary>
		/// Refers to the boldness of the font.
		/// </summary>
		[SvgAttribute("font-weight")]
		public virtual SvgFontWeight FontWeight
		{
			get { return this._fontWeight; }
			set { this._fontWeight = value; this.IsPathDirty = true; }
		}


        /// <summary>
        /// Set all font information.
        /// </summary>
        [SvgAttribute("font")]
        public virtual string Font
        {
            get { return this._font; }
            set
            {
                var parts = value.Split(',');
                foreach (var part in parts)
                {
                    //This deals with setting font size. Looks for either <number>px or <number>pt style="font: bold 16px/normal 'trebuchet ms', verdana, sans-serif;"
                    Regex rx = new Regex(@"(\d+)+(?=pt|px)");
                    var res = rx.Match(part);
                    if (res.Success)
                    {
                        int fontSize = 10;
                        int.TryParse(res.Value, out fontSize);
                        this.FontSize = new SvgUnit((float)fontSize);
                    }

                    //this assumes "bold" has spaces around it. e.g.: style="font: bold 16px/normal 
                    rx = new Regex(@"\sbold\s");
                    res = rx.Match(part);
                    if (res.Success)
                    {
                        this.FontWeight = SvgFontWeight.bold;
                    }
                }
                var font = ValidateFontFamily(value);
                this._fontFamily = font;
                this._font = font; //not sure this is used?

                this.IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <remarks>
        /// <para>Unlike other <see cref="SvgGraphicsElement"/>s, <see cref="SvgText"/> has a default fill of black rather than transparent.</para>
        /// </remarks>
        /// <value>The fill.</value>
        public override SvgPaintServer Fill
        {
            get { return (this.Attributes["fill"] == null) ? new SvgColourServer(Color.Black) : (SvgPaintServer)this.Attributes["fill"]; }
            set { this.Attributes["fill"] = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Text;
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        /// <value></value>
        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        static private RectangleF MeasureString(SvgRenderer renderer, string text, Font font)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddString(text, font.FontFamily, 0, font.Size, new PointF(0.0f, 0.0f), StringFormat.GenericTypographic);
            
            p.Transform(renderer.Transform);
            return p.GetBounds();
        }

        private static string ValidateFontFamily(string fontFamilyList)
        {
            // Split font family list on "," and then trim start and end spaces and quotes.
            var fontParts = fontFamilyList.Split(new[] { ',' }).Select(fontName => fontName.Trim(new[] { '"', ' ','\''  }));

            var families = System.Drawing.FontFamily.Families;

            // Find a the first font that exists in the list of installed font families.
            //styles from IE get sent through as lowercase.
            foreach (var f in fontParts.Where(f => families.Any(family => family.Name.ToLower() == f.ToLower())))
            {
                return f;
            }
            // No valid font family found from the list requested.
            return null;
        }


        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> object to render to.</param>
        /// <remarks>Necessary to make sure that any internal tspan elements get rendered as well</remarks>
        protected override void Render(SvgRenderer renderer)
        {
            if ((this.Path != null) && this.Visible && this.Displayable)
            {
                this.PushTransforms(renderer);
                this.SetClip(renderer);

                // If this element needs smoothing enabled turn anti-aliasing on
                if (this.RequiresSmoothRendering)
                {
                    renderer.SmoothingMode = SmoothingMode.AntiAlias;
                }

                this.RenderFill(renderer);
                this.RenderStroke(renderer);
                this.RenderChildren(renderer);

                // Reset the smoothing mode
                if (this.RequiresSmoothRendering && renderer.SmoothingMode == SmoothingMode.AntiAlias)
                {
                    renderer.SmoothingMode = SmoothingMode.Default;
                }

                this.ResetClip(renderer);
                this.PopTransforms(renderer);
            }
        }

        private GraphicsPath _path;

        protected class NodeBounds
        {
            public float xOffset { get; set; }
            public SizeF Bounds { get; set; }
            public ISvgNode Node { get; set; }
        }
        protected class BoundsData
        {
            private List<NodeBounds> _nodes = new List<NodeBounds>();
            public IList<NodeBounds> Nodes 
            {
                get { return _nodes; }
            }
            public SizeF Bounds { get; set; }
        }
        protected BoundsData GetTextBounds()
        {
            var font = GetFont();
            SvgTextBase innerText;
            SizeF stringBounds;
            float totalHeight = 0;
            float totalWidth = 0;

            var result = new BoundsData();
            var nodes = (from n in this.Nodes 
                         where (n is SvgContentNode || n is SvgTextBase) && !string.IsNullOrEmpty(n.Content)
                         select n).ToList();
            ISvgNode node;
            for (var i = 0; i < nodes.Count; i++)
            {
                node = nodes[i];
                lock (_stringMeasure)
                {
                    innerText = node as SvgTextBase;
                    if (innerText == null)
                    {
                        stringBounds = _stringMeasure.MeasureString(PrepareText(node.Content,
                                                                                i > 0 && nodes[i - 1] is SvgTextBase,
                                                                                i < nodes.Count - 1 && nodes[i + 1] is SvgTextBase), font);
                    }
                    else
                    {
                        stringBounds = innerText.GetTextBounds().Bounds;
                    }
                    result.Nodes.Add(new NodeBounds() { Bounds = stringBounds, Node = node, xOffset = totalWidth });
                    totalHeight = Math.Max(totalHeight, stringBounds.Height);
                    totalWidth += stringBounds.Width;
                }
            }
            result.Bounds = new SizeF(totalWidth, totalHeight);
            return result;
        }

        protected float _calcX = 0;
        protected float _calcY = 0;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get
            {
                // Make sure the path is always null if there is no text
                //if there is a TSpan inside of this text element then path should not be null (even if this text is empty!)
                if ((string.IsNullOrEmpty(this.Text) || this.Text.Trim().Length < 1) && this.Children.Where(x => x is SvgTextSpan).Select(x => x as SvgTextSpan).Count() == 0)
                    return _path = null;
                //NOT SURE WHAT THIS IS ABOUT - Path gets created again anyway - WTF?
                // When an empty string is passed to GraphicsPath, it rises an InvalidArgumentException...

                if (_path == null || this.IsPathDirty)
                {
                    var font = GetFont();
                    SvgTextBase innerText;
                    //RectangleF bounds;
                    float x = (_x == SvgUnit.Empty || _x == SvgUnit.None ? _calcX : _x.ToDeviceValue(this)) + _dx.ToDeviceValue(this);
                    float y = (_y == SvgUnit.Empty || _y == SvgUnit.None ? _calcY : _y.ToDeviceValue(this, true)) + _dy.ToDeviceValue(this, true);

                    _path = new GraphicsPath();
                    _path.StartFigure();

                    // Measure the overall bounds of all the text
                    var boundsData = GetTextBounds();

                    // Determine the location of the start point
                    switch (this.TextAnchor)
                    {
                        case SvgTextAnchor.Middle:
                            x -= (boundsData.Bounds.Width / 2);
                            break;
                        case SvgTextAnchor.End:
                            x -= boundsData.Bounds.Width;
                            break;
                    }

                    NodeBounds data;
                    for (var i = 0; i < boundsData.Nodes.Count; i++)
                    {
                        data = boundsData.Nodes[i];
                        innerText = data.Node as SvgTextBase;
                        if (innerText == null)
                        {
                            // Minus FontSize because the x/y coords mark the bottom left, not bottom top.
                            DrawString(_path, x + data.xOffset, y - boundsData.Bounds.Height, font,
                                       PrepareText(data.Node.Content, i > 0 && boundsData.Nodes[i-1].Node is SvgTextBase,
                                                                      i < boundsData.Nodes.Count - 1 && boundsData.Nodes[i + 1].Node is SvgTextBase));
                        }
                        else
                        {
                            innerText._calcX = x + data.xOffset;
                            innerText._calcY = y;
                        }
                    }

                    _path.CloseFigure();
                    this.IsPathDirty = false;
                }
                return _path;
            }
            protected set
            {
                _path = value;
            }
        }

        /// <summary>
        /// Prepare the text according to the whitespace handling rules.  <see href="http://www.w3.org/TR/SVG/text.html">SVG Spec</see>.
        /// </summary>
        /// <param name="value">Text to be prepared</param>
        /// <returns>Prepared text</returns>
        protected string PrepareText(string value, bool leadingSpace, bool trailingSpace)
        {
            if (_space == XmlSpaceHandling.preserve)
            {
                return (leadingSpace ? " " : "") + value.Replace('\t', ' ').Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' ') + (trailingSpace ? " " : "");
            }
            else
            {
                return (leadingSpace ? " " : "") + value.Replace("\r", "").Replace("\n", "").Replace('\t', ' ').Trim().Replace("  ", " ") + (trailingSpace ? " " : "");
            }
        }
        /// <summary>
        /// Get the font information based on data stored with the text object or inherited from the parent.
        /// </summary>
        /// <returns></returns>
        internal Font GetFont()
        {
            var parent = this.Parent as SvgTextBase;
            Font parentFont = null;
            if (parent != null) parentFont = parent.GetFont();

            float fontSize = this.FontSize.ToDeviceValue(this);
            if (fontSize == 0.0f)
            {
                fontSize = (parentFont == null ? 1.0f : parentFont.Size);
                fontSize = (fontSize == 0.0f ? 1.0f : fontSize);
            }
            var fontWeight = ((_fontWeight == SvgFontWeight.inherit && parentFont != null && parentFont.Bold) || _fontWeight == SvgFontWeight.bold ?
                                FontStyle.Bold : FontStyle.Regular);
            var family = _fontFamily ?? (parentFont == null ? DefaultFontFamily : parentFont.FontFamily.Name);
            return new Font(family, fontSize, fontWeight, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Draws a string on a path at a specified location and with a specified font.
        /// </summary>
        internal void DrawString(GraphicsPath path, float x, float y, Font font, string text)
		{
			PointF location = new PointF(x, y);
			
			// No way to do letter-spacing or word-spacing, so do manually
			if (this.LetterSpacing.Value > 0.0f || this.WordSpacing.Value > 0.0f)
			{
				// Cut up into words, or just leave as required
				string[] words = (this.WordSpacing.Value > 0.0f) ? text.Split(' ') : new string[] { text };
				float wordSpacing = this.WordSpacing.ToDeviceValue(this);
				float letterSpacing = this.LetterSpacing.ToDeviceValue(this);
				float start = this.X.ToDeviceValue(this);

				foreach (string word in words)
				{
					// Only do if there is line spacing, just write the word otherwise
					if (this.LetterSpacing.Value > 0.0f)
					{
						char[] characters = word.ToCharArray();
						foreach (char currentCharacter in characters)
						{
                            path.AddString(currentCharacter.ToString(), font.FontFamily, (int)font.Style, font.Size, location, StringFormat.GenericTypographic);
							location = new PointF(path.GetBounds().Width + start + letterSpacing, location.Y);
						}
					}
					else
					{
                        path.AddString(word, font.FontFamily, (int)font.Style, font.Size, location, StringFormat.GenericTypographic);
					}

					// Move the location of the word to be written along
					location = new PointF(path.GetBounds().Width + start + wordSpacing, location.Y);
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(text))
				{
					path.AddString(text, font.FontFamily, (int)font.Style, font.Size, location, StringFormat.GenericTypographic);
				}
			}
		}
		
		[SvgAttribute("onchange")]
        public event EventHandler<StringArg> Change;
		
		//change
        protected void OnChange(string newString, string sessionID)
        {
        	RaiseChange(this, new StringArg {s = newString, SessionID = sessionID});
        }
        
        protected void RaiseChange(object sender, StringArg s)
        {
        	var handler = Change;
            if (handler != null)
            {
                handler(sender, s);
            }
        }

#if Net4
		public override void RegisterEvents(ISvgEventCaller caller)
		{
			//register basic events
			base.RegisterEvents(caller); 
			
			//add change event for text
            caller.RegisterAction<string, string>(this.ID + "/onchange", OnChange);
		}
		
		public override void UnregisterEvents(ISvgEventCaller caller)
		{
			//unregister base events
			base.UnregisterEvents(caller);
			
			//unregister change event
        	caller.UnregisterAction(this.ID + "/onchange");
			
		}
#endif
    }
}
