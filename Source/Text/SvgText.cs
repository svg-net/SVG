using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

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
        private Font _font;
        private GraphicsPath _path;
        private SvgTextAnchor _textAnchor = SvgTextAnchor.Start;
        private static readonly SvgRenderer _stringMeasure;

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
            this._font = new Font(new FontFamily("Times New Roman"), 1.0f);
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
            set { base.Content = value; this.IsPathDirty = true; }
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
            set { this._x = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return this._y; }
            set { this._y = value; this.IsPathDirty = true; }
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
        public virtual Font FontFamily
        {
            get { return this._font; }
            set { this._font = value; this.IsPathDirty = true; }
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
        /// Set all font information.
        /// </summary>
        [SvgAttribute("font")]
        public virtual Font Font
        {
            get { return this._font; }
            set { this._font = value; this.IsPathDirty = true; }
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
            get { return (this.Attributes["Fill"] == null) ? new SvgColourServer(Color.Black) : (SvgPaintServer)this.Attributes["Fill"]; }
            set { this.Attributes["Fill"] = value; }
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

        static private int MeasureString(SvgRenderer renderer, string text, Font font)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddString(text, font.FontFamily, 0, font.Size, new PointF(0.0f, 0.0f), StringFormat.GenericTypographic);
            p.Transform(renderer.Transform);
            return (int)(p.GetBounds().Width + 1.0f);
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
                if (_path == null || this.IsPathDirty && !string.IsNullOrEmpty(this.Text))
                {
                    float fontSize = this.FontSize.ToDeviceValue(this);
                    if (fontSize == 0.0f)
                    {
                        fontSize = 1.0f;
                    }
                    int stringWidth;
                    PointF location = PointF.Empty;

                    // Minus FontSize because the x/y coords mark the bottom left, not bottom top.
                    switch (this.TextAnchor)
                    {
                        case SvgTextAnchor.Start:
                            location = new PointF(this.X.ToDeviceValue(this), this.Y.ToDeviceValue(this, true) - fontSize);
                            break;
                        case SvgTextAnchor.Middle:
                            stringWidth = SvgText.MeasureString(_stringMeasure, this.Text, new Font(this._font.FontFamily, fontSize));
                            location = new PointF(this.X.ToDeviceValue(this) - (stringWidth / 2), this.Y.ToDeviceValue(this, true) - fontSize);
                            break;
                        case SvgTextAnchor.End:
                            stringWidth = SvgText.MeasureString(_stringMeasure, this.Text, new Font(this._font.FontFamily, fontSize));
                            location = new PointF(this.X.ToDeviceValue(this) - stringWidth, this.Y.ToDeviceValue(this, true) - fontSize);
                            break;
                    }

                    _path = new GraphicsPath();
                    _path.StartFigure();

                    // No way to do letter-spacing or word-spacing, so do manually
                    if (this.LetterSpacing.Value > 0.0f || this.WordSpacing.Value > 0.0f)
                    {
                        // Cut up into words, or just leave as required
                        string[] words = (this.WordSpacing.Value > 0.0f) ? this.Text.Split(' ') : new string[] { this.Text };
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
                                    _path.AddString(currentCharacter.ToString(), this._font.FontFamily, 0, fontSize, location, StringFormat.GenericTypographic);
                                    location = new PointF(_path.GetBounds().Width + start + letterSpacing, location.Y);
                                }
                            }
                            else
                            {
                                _path.AddString(word, this._font.FontFamily, 0, fontSize, location, StringFormat.GenericTypographic);
                            }

                            // Move the location of the word to be written along
                            location = new PointF(_path.GetBounds().Width + start + wordSpacing, location.Y);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.Text))
                        {
                            _path.AddString(this.Text, this._font.FontFamily, 0, fontSize, location, StringFormat.GenericTypographic);
                        }
                    }

                    _path.CloseFigure();
                    this.IsPathDirty = false;
                }
                return _path;
            }
        }
    }
}