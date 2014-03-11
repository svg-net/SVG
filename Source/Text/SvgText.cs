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
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    [SvgElement("text")]
    public class SvgText : SvgVisualElement
    {
        private SvgUnit _x;
        private SvgUnit _y;
        private SvgUnit _letterSpacing;
        private SvgUnit _wordSpacing;
        private SvgUnit _fontSize;
		private SvgFontWeight _fontWeight;
        private string _font;
        private string _fontFamily;
        private GraphicsPath _path;
        private SvgTextAnchor _textAnchor = SvgTextAnchor.Start;
        private static readonly SvgRenderer _stringMeasure;
        private const string DefaultFontFamily = "Times New Roman";

        /// <summary>
        /// Initializes the <see cref="SvgText"/> class.
        /// </summary>
        static SvgText()
        {
            Bitmap bitmap = new Bitmap(1, 1);
            _stringMeasure = SvgRenderer.FromImage(bitmap);
            _stringMeasure.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgText"/> class.
        /// </summary>
        public SvgText()
        {
            this._fontFamily = DefaultFontFamily;
            this._fontSize = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgText"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public SvgText(string text) : this()
        {
            this.Text = text;
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
            	this._x = value; 
            	this.IsPathDirty = true;
            	OnAttributeChanged(new AttributeEventArgs{ Attribute = "x", Value = value });
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
            	this._y = value; 
            	this.IsPathDirty = true; 
            	OnAttributeChanged(new AttributeEventArgs{ Attribute = "y", Value = value });
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
            get { return this._fontFamily; }
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
            get { return (this.Attributes["fill"] == null) ? new SvgColourServer(Color.Black) : (SvgPaintServer)this.Attributes["Fill"]; }
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

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get
            {
                // Make sure the path is always null if there is no text
				//if (string.IsNullOrEmpty(this.Text))
				//    _path = null;
				//NOT SURE WHAT THIS IS ABOUT - Path gets created again anyway - WTF?

				if (_path == null || this.IsPathDirty)
                {
                    float fontSize = this.FontSize.ToDeviceValue(this);
                    if (fontSize == 0.0f)
                    {
                        fontSize = 1.0f;
                    }

                	FontStyle fontWeight = (this.FontWeight == SvgFontWeight.bold ? FontStyle.Bold : FontStyle.Regular);
					Font font = new Font(this._fontFamily, fontSize, fontWeight, GraphicsUnit.Pixel);

                    _path = new GraphicsPath();
                    _path.StartFigure();

					if (!string.IsNullOrEmpty(this.Text))
	                	DrawString(_path, this.X, this.Y, SvgUnit.Empty, SvgUnit.Empty, font, fontSize, this.Text);

					foreach (var tspan in this.Children.Where(x => x is SvgTextSpan).Select(x => x as SvgTextSpan))
					{
						if (!string.IsNullOrEmpty(tspan.Text))
							DrawString(
							_path, 
							tspan.X == SvgUnit.Empty ? this.X: tspan.X,
							tspan.Y == SvgUnit.Empty ? this.Y : tspan.Y, 
							tspan.DX,
							tspan.DY,
							font, 
							fontSize,
							tspan.Text);
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

        private static string ValidateFontFamily(string fontFamilyList)
        {
            // Split font family list on "," and then trim start and end spaces and quotes.
            var fontParts = fontFamilyList.Split(new[] { ',' }).Select(fontName => fontName.Trim(new[] { '"', ' ' }));

            var families = System.Drawing.FontFamily.Families;

            // Find a the first font that exists in the list of installed font families.
            //styles from IE get sent through as lowercase.
            foreach (var f in fontParts.Where(f => families.Any(family => family.Name.ToLower() == f.ToLower())))
            {
                return f;
            }
            // No valid font family found from the list requested.
            return DefaultFontFamily;
        }

		private void DrawString(GraphicsPath path, SvgUnit x, SvgUnit y, SvgUnit dx, SvgUnit dy, Font font, float fontSize, string text)
		{
			PointF location = PointF.Empty;
			SizeF stringBounds;

			lock (_stringMeasure)
			{
				stringBounds = _stringMeasure.MeasureString(text, font);
			}

			float xToDevice = x.ToDeviceValue(this) + dx.ToDeviceValue(this);
			float yToDevice = y.ToDeviceValue(this, true) + dy.ToDeviceValue(this, true);

			// Minus FontSize because the x/y coords mark the bottom left, not bottom top.
			switch (this.TextAnchor)
			{
				case SvgTextAnchor.Start:
					location = new PointF(xToDevice, yToDevice - stringBounds.Height);
					break;
				case SvgTextAnchor.Middle:
					location = new PointF(xToDevice - (stringBounds.Width / 2), yToDevice - stringBounds.Height);
					break;
				case SvgTextAnchor.End:
					location = new PointF(xToDevice - stringBounds.Width, yToDevice - stringBounds.Height);
					break;
			}

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
							path.AddString(currentCharacter.ToString(), new FontFamily(this._fontFamily), (int)font.Style, fontSize, location, StringFormat.GenericTypographic);
							location = new PointF(path.GetBounds().Width + start + letterSpacing, location.Y);
						}
					}
					else
					{
						path.AddString(word, new FontFamily(this._fontFamily), (int)font.Style, fontSize, location, StringFormat.GenericTypographic);
					}

					// Move the location of the word to be written along
					location = new PointF(path.GetBounds().Width + start + wordSpacing, location.Y);
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(text))
				{
					path.AddString(text, new FontFamily(this._fontFamily), (int)font.Style, fontSize, location, StringFormat.GenericTypographic);
				}
			}

		}


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgText>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgText;
			newObj.TextAnchor = this.TextAnchor;
			newObj.WordSpacing = this.WordSpacing;
			newObj.LetterSpacing = this.LetterSpacing;
			newObj.Font = this.Font;
			newObj.FontFamily = this.FontFamily;
			newObj.FontSize = this.FontSize;
			newObj.FontWeight = this.FontWeight;
			newObj.X = this.X;
			newObj.Y = this.Y;
			return newObj;
		}
    }
}
