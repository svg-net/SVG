using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Svg
{
    public abstract partial class SvgTextBase : SvgVisualElement
    {
        private SvgUnitCollection _x = new SvgUnitCollection();
        private SvgUnitCollection _y = new SvgUnitCollection();
        private SvgUnitCollection _dy = new SvgUnitCollection();
        private SvgUnitCollection _dx = new SvgUnitCollection();
        private string _rotate;
        private List<float> _rotations = new List<float>();

        public SvgTextBase()
        {
            _x.CollectionChanged += OnXChanged;
            _dx.CollectionChanged += OnDxChanged;
            _y.CollectionChanged += OnYChanged;
            _dy.CollectionChanged += OnDyChanged;
        }

        /// <summary>
        /// Gets or sets the text to be rendered.
        /// </summary>
        public virtual string Text
        {
            get { return Content; }
            set
            {
                Nodes.Clear();
                Children.Clear();
                if (value != null)
                {
                    Nodes.Add(new SvgContentNode { Content = value });
                }
                Content = value;
                IsPathDirty = true;
            }
        }

        public override XmlSpaceHandling SpaceHandling
        {
            set { base.SpaceHandling = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        [SvgAttribute("x")]
        public virtual SvgUnitCollection X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    if (_x != null) _x.CollectionChanged -= OnXChanged;
                    _x = value;
                    if (_x != null) _x.CollectionChanged += OnXChanged;

                    IsPathDirty = true;
                }
                Attributes["x"] = value;
            }
        }

        private void OnXChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Attributes["x"] = X.FirstOrDefault();
            this.IsPathDirty = true;
        }

        /// <summary>
        /// Gets or sets the dX.
        /// </summary>
        /// <value>The dX.</value>
        [SvgAttribute("dx")]
        public virtual SvgUnitCollection Dx
        {
            get { return _dx; }
            set
            {
                if (_dx != value)
                {
                    if (_dx != null) _dx.CollectionChanged -= OnDxChanged;
                    _dx = value;
                    if (_dx != null) _dx.CollectionChanged += OnDxChanged;

                    IsPathDirty = true;
                }
                Attributes["dx"] = value;
            }
        }

        private void OnDxChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Attributes["dx"] = X.FirstOrDefault();
            this.IsPathDirty = true;
        }


        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        [SvgAttribute("y")]
        public virtual SvgUnitCollection Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    if (_y != null) _y.CollectionChanged -= OnYChanged;
                    _y = value;
                    if (_y != null) _y.CollectionChanged += OnYChanged;

                    IsPathDirty = true;
                }
                Attributes["y"] = value;
            }
        }

        private void OnYChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Attributes["y"] = Y.FirstOrDefault();
            this.IsPathDirty = true;
        }

        /// <summary>
        /// Gets or sets the dY.
        /// </summary>
        /// <value>The dY.</value>
        [SvgAttribute("dy")]
        public virtual SvgUnitCollection Dy
        {
            get { return _dy; }
            set
            {
                if (_dy != value)
                {
                    if (_dy != null) _dy.CollectionChanged -= OnDyChanged;
                    _dy = value;
                    if (_dy != null) _dy.CollectionChanged += OnDyChanged;

                    IsPathDirty = true;
                }
                Attributes["dy"] = value;
            }
        }

        private void OnDyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Attributes["dy"] = Y.FirstOrDefault();
            this.IsPathDirty = true;
        }

        /// <summary>
        /// Gets or sets the rotate.
        /// </summary>
        /// <value>The rotate.</value>
        [SvgAttribute("rotate")]
        public virtual string Rotate
        {
            get { return _rotate; }
            set
            {
                if (_rotate != value)
                {
                    _rotate = value;
                    _rotations.Clear();
                    _rotations.AddRange(from r in _rotate.Split(new char[] { ',', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                        select float.Parse(r, NumberStyles.Any, CultureInfo.InvariantCulture));
                    IsPathDirty = true;
                }
                Attributes["rotate"] = value;
            }
        }

        /// <summary>
        /// The pre-calculated length of the text
        /// </summary>
        [SvgAttribute("textLength")]
        public virtual SvgUnit TextLength
        {
            get { return GetAttribute("textLength", true, SvgUnit.None); }
            set { Attributes["textLength"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the text anchor.
        /// </summary>
        /// <value>The text anchor.</value>
        [SvgAttribute("lengthAdjust")]
        public virtual SvgTextLengthAdjust LengthAdjust
        {
            get { return GetAttribute("lengthAdjust", true, SvgTextLengthAdjust.Spacing); }
            set { Attributes["lengthAdjust"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Specifies spacing behavior between text characters.
        /// </summary>
        [SvgAttribute("letter-spacing")]
        public virtual SvgUnit LetterSpacing
        {
            get { return GetAttribute("letter-spacing", true, SvgUnit.None); }
            set { Attributes["letter-spacing"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Specifies spacing behavior between words.
        /// </summary>
        [SvgAttribute("word-spacing")]
        public virtual SvgUnit WordSpacing
        {
            get { return GetAttribute("word-spacing", true, SvgUnit.None); }
            set { Attributes["word-spacing"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <remarks>
        /// <para>Unlike other <see cref="SvgVisualElement"/>s, <see cref="SvgText"/> has a default fill of black rather than transparent.</para>
        /// </remarks>
        /// <value>The fill.</value>
        public override SvgPaintServer Fill
        {
            get { return GetAttribute<SvgPaintServer>("fill", true, new SvgColourServer(System.Drawing.Color.Black)); }
            set { Attributes["fill"] = value; }
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

        private static readonly Regex MultipleSpaces = new Regex(@" {2,}", RegexOptions.Compiled);

        /// <summary>
        /// Prepare the text according to the whitespace handling rules and text transformations.  <see href="http://www.w3.org/TR/SVG/text.html">SVG Spec</see>.
        /// </summary>
        /// <param name="value">Text to be prepared</param>
        /// <returns>Prepared text</returns>
        protected string PrepareText(string value)
        {
            value = ApplyTransformation(value);
            value = new StringBuilder(value).Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ').ToString();
            return this.SpaceHandling == XmlSpaceHandling.Preserve ? value : MultipleSpaces.Replace(value.Trim(), " ");
        }

        private string ApplyTransformation(string value)
        {
            switch (this.TextTransformation)
            {
                case SvgTextTransformation.Capitalize:
                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);

                case SvgTextTransformation.Uppercase:
                    return value.ToUpper();

                case SvgTextTransformation.Lowercase:
                    return value.ToLower();
            }

            return value;
        }

        [SvgAttribute("onchange")]
        public event EventHandler<StringArg> Change;

        //change
        protected void OnChange(string newString, string sessionID)
        {
            RaiseChange(this, new StringArg { s = newString, SessionID = sessionID });
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

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgTextBase;

            if (_x == null)
                newObj._x = null;
            else
            {
                newObj._x = (SvgUnitCollection)_x.Clone();
                newObj._x.CollectionChanged += newObj.OnXChanged;
            }
            if (_y == null)
                newObj._y = null;
            else
            {
                newObj._y = (SvgUnitCollection)_y.Clone();
                newObj._y.CollectionChanged += newObj.OnYChanged;
            }
            if (_dx == null)
                newObj._dx = null;
            else
            {
                newObj._dx = (SvgUnitCollection)_dx.Clone();
                newObj._dx.CollectionChanged += newObj.OnDxChanged;
            }
            if (_dy == null)
                newObj._dy = null;
            else
            {
                newObj._dy = (SvgUnitCollection)_dy.Clone();
                newObj._dy.CollectionChanged += newObj.OnDyChanged;
            }
            newObj._rotate = _rotate;
            foreach (var rotation in _rotations)
                newObj._rotations.Add(rotation);
            return newObj;
        }

        /// <summary>Empty text elements are not legal - only write this element if it has children.</summary>
        public override bool ShouldWriteElement()
        {
            return (this.HasChildren() || this.Nodes.Count > 0);
        }
    }
}
