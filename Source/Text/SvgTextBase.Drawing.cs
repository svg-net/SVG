#if !NO_SDC
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    public abstract partial class SvgTextBase : SvgVisualElement
    {
        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get
            {
                var path = this.Path(null);
                foreach (var elem in this.Children.OfType<SvgVisualElement>())
                {
                    // If string is blank, GraphicsPath points will be empty.
                    var span = elem as SvgTextSpan;
                    if (span != null && string.IsNullOrWhiteSpace(span.Text))
                        continue;

                    path.AddPath(elem.Path(null), false);
                }
                if (Transforms == null || Transforms.Count == 0)
                    return path.GetBounds();

                using (path = (GraphicsPath)path.Clone())
                using (var matrix = Transforms.GetMatrix())
                {
                    path.Transform(matrix);
                    return path.GetBounds();
                }
            }
        }

        //private static GraphicsPath GetPath(string text, Font font)
        //{
        //    var fontMetrics = (from c in text.Distinct()
        //                       select new { Char = c, Metrics = Metrics(c, font) }).
        //                       ToDictionary(c => c.Char, c=> c.Metrics);
        //    // Measure each character and check the metrics against the overall metrics of rendering
        //    // an entire word with kerning.
        //}
        //private static RectangleF Metrics(char c, Font font)
        //{
        //    var path = new GraphicsPath();
        //    path.AddString(c.ToString(), font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), StringFormat.GenericTypographic);
        //    return path.GetBounds();
        //}

        protected internal override void RenderFillAndStroke(ISvgRenderer renderer)
        {
            base.RenderFillAndStroke(renderer);
            RenderChildren(renderer);
        }

        internal virtual IEnumerable<ISvgNode> GetContentNodes()
        {
            return (this.Nodes == null || this.Nodes.Count < 1 ? this.Children.OfType<ISvgNode>().Where(o => !(o is ISvgDescriptiveElement)) : this.Nodes);
        }
        protected virtual GraphicsPath GetBaselinePath(ISvgRenderer renderer)
        {
            return null;
        }
        protected virtual float GetAuthorPathLength()
        {
            return 0;
        }

        private GraphicsPath _path;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            //if there is a TSpan inside of this text element then path should not be null (even if this text is empty!)

            var nodeCount = GetContentNodes().Count(x => x is SvgContentNode &&
                                                         string.IsNullOrEmpty(x.Content.Trim(new[]
                                                             {'\r', '\n', '\t'})));

            if (_path == null || IsPathDirty || nodeCount == 1)
            {
                if (renderer != null && renderer is IGraphicsProvider)
                    SetPath(new TextDrawingState(renderer, this));
                else
                    using (var r = SvgRenderer.FromNull())
                        SetPath(new TextDrawingState(r, this));
            }
            return _path;
        }

        private void SetPath(TextDrawingState state)
        {
            SetPath(state, true);
        }

        /// <summary>
        /// Sets the path on this element and all child elements.  Uses the state
        /// object to track the state of the drawing
        /// </summary>
        /// <param name="state">State of the drawing operation</param>
        /// <param name="doMeasurements">If true, calculate and apply text length adjustments.</param>
        private void SetPath(TextDrawingState state, bool doMeasurements)
        {
            TextDrawingState origState = null;
            bool alignOnBaseline = state.BaselinePath != null && (this.TextAnchor == SvgTextAnchor.Middle || this.TextAnchor == SvgTextAnchor.End);

            if (doMeasurements)
            {
                if (this.TextLength != SvgUnit.None)
                {
                    origState = state.Clone();
                }
                else if (alignOnBaseline)
                {
                    origState = state.Clone();
                    state.BaselinePath = null;
                }
            }

            foreach (var node in GetContentNodes())
            {
                SvgTextBase textNode = node as SvgTextBase;

                if (textNode == null)
                {
                    if (!string.IsNullOrEmpty(node.Content)) state.DrawString(PrepareText(node.Content));
                }
                else
                {
                    TextDrawingState newState = new TextDrawingState(state, textNode);

                    textNode.SetPath(newState);
                    state.NumChars += newState.NumChars;
                    state.Current = newState.Current;
                }
            }

            var path = state.GetPath() ?? new GraphicsPath();

            // Apply any text length adjustments
            if (doMeasurements)
            {
                if (this.TextLength != SvgUnit.None)
                {
                    var specLength = this.TextLength.ToDeviceValue(state.Renderer, UnitRenderingType.Horizontal, this);
                    var actLength = state.TextBounds.Width;
                    var diff = (actLength - specLength);
                    if (Math.Abs(diff) > 1.5)
                    {
                        if (this.LengthAdjust == SvgTextLengthAdjust.Spacing)
                        {
                            if (this.X.Count < 2)
                            {
                                var numCharDiff = state.NumChars - origState.NumChars - 1;
                                if ( numCharDiff != 0)
                                {
                                    origState.LetterSpacingAdjust = -1 * diff / numCharDiff;
                                    SetPath(origState, false);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            using (var matrix = new Matrix())
                            {
                                matrix.Translate(-1 * state.TextBounds.X, 0, MatrixOrder.Append);
                                matrix.Scale(specLength / actLength, 1, MatrixOrder.Append);
                                matrix.Translate(state.TextBounds.X, 0, MatrixOrder.Append);
                                path.Transform(matrix);
                            }
                        }
                    }
                }
                else if (alignOnBaseline)
                {
                    var bounds = path.GetBounds();
                    if (this.TextAnchor == SvgTextAnchor.Middle)
                    {
                        origState.StartOffsetAdjust = -1 * bounds.Width / 2;
                    }
                    else
                    {
                        origState.StartOffsetAdjust = -1 * bounds.Width;
                    }
                    SetPath(origState, false);
                    return;
                }
            }


            _path = path;
            this.IsPathDirty = false;
        }

        private class FontBoundable : ISvgBoundable
        {
            private IFontDefn _font;
            private float _width = 1;

            public FontBoundable(IFontDefn font)
            {
                _font = font;
            }
            public FontBoundable(IFontDefn font, float width)
            {
                _font = font;
                _width = width;
            }

            public PointF Location
            {
                get { return PointF.Empty; }
            }

            public SizeF Size
            {
                get { return new SizeF(_width, _font.Size); }
            }

            public RectangleF Bounds
            {
                get { return new RectangleF(this.Location, this.Size); }
            }
        }

        private class TextDrawingState
        {
            private float _xAnchor = float.MinValue;
            private IList<GraphicsPath> _anchoredPaths = new List<GraphicsPath>();
            private GraphicsPath _currPath = null;
            private GraphicsPath _finalPath = null;
            private float _authorPathLength = 0;

            public GraphicsPath BaselinePath { get; set; }
            public PointF Current { get; set; }
            public RectangleF TextBounds { get; set; }
            public SvgTextBase Element { get; set; }
            public float LetterSpacingAdjust { get; set; }
            public int NumChars { get; set; }
            public TextDrawingState Parent { get; set; }
            public ISvgRenderer Renderer { get; set; }
            public float StartOffsetAdjust { get; set; }

            private TextDrawingState() { }
            public TextDrawingState(ISvgRenderer renderer, SvgTextBase element)
            {
                this.Element = element;
                this.Renderer = renderer;
                this.Current = PointF.Empty;
                this.TextBounds = RectangleF.Empty;
                _xAnchor = 0;
                this.BaselinePath = element.GetBaselinePath(renderer);
                _authorPathLength = element.GetAuthorPathLength();
            }

            public TextDrawingState(TextDrawingState parent, SvgTextBase element)
                : this(parent.Renderer, element)
            {
                this.Parent = parent;
                this.Current = parent.Current;
                this.TextBounds = parent.TextBounds;
                this.BaselinePath = this.BaselinePath ?? parent.BaselinePath;
                if (_authorPathLength == 0)
                    _authorPathLength = parent._authorPathLength;
            }

            public GraphicsPath GetPath()
            {
                FlushPath();
                return _finalPath;
            }

            public TextDrawingState Clone()
            {
                var result = new TextDrawingState();
                result._anchoredPaths = this._anchoredPaths.ToList();
                result.BaselinePath = this.BaselinePath;
                result._xAnchor = this._xAnchor;
                result.Current = this.Current;
                result.TextBounds = this.TextBounds;
                result.Element = this.Element;
                result.NumChars = this.NumChars;
                result.Parent = this.Parent;
                result.Renderer = this.Renderer;
                return result;
            }

            public void DrawString(string value)
            {
                // Get any defined anchors
                var xAnchors = GetValues(value.Length, e => e._x, UnitRenderingType.HorizontalOffset);
                var yAnchors = GetValues(value.Length, e => e._y, UnitRenderingType.VerticalOffset);
                using (var fontManager = this.Element.OwnerDocument?.FontManager == null ? new SvgFontManager() : null)
                using (var font = this.Element.GetFont(this.Renderer, fontManager))
                {
                    var fontBaselineHeight = font.Ascent(this.Renderer);
                    PathStatistics pathStats = null;
                    var pathScale = 1.0;
                    if (BaselinePath != null)
                    {
                        pathStats = new PathStatistics(BaselinePath.PathData);
                        if (_authorPathLength > 0) pathScale = _authorPathLength / pathStats.TotalLength;
                    }

                    // Get all of the offsets (explicit and defined by spacing)
                    IList<float> xOffsets;
                    IList<float> yOffsets;
                    IList<float> rotations;
                    float baselineShift = 0.0f;

                    try
                    {
                        this.Renderer.SetBoundable(new FontBoundable(font, (float)(pathStats == null ? 1 : pathStats.TotalLength)));
                        xOffsets = GetValues(value.Length, e => e._dx, UnitRenderingType.Horizontal);
                        yOffsets = GetValues(value.Length, e => e._dy, UnitRenderingType.Vertical);
                        if (StartOffsetAdjust != 0.0f)
                        {
                            if (xOffsets.Count < 1)
                            {
                                xOffsets.Add(StartOffsetAdjust);
                            }
                            else
                            {
                                xOffsets[0] += StartOffsetAdjust;
                            }
                        }

                        if (this.Element.LetterSpacing.Value != 0.0f || this.Element.WordSpacing.Value != 0.0f || this.LetterSpacingAdjust != 0.0f)
                        {
                            var spacing = this.Element.LetterSpacing.ToDeviceValue(this.Renderer, UnitRenderingType.Horizontal, this.Element) + this.LetterSpacingAdjust;
                            var wordSpacing = this.Element.WordSpacing.ToDeviceValue(this.Renderer, UnitRenderingType.Horizontal, this.Element);
                            if (this.Parent == null && this.NumChars == 0 && xOffsets.Count < 1) xOffsets.Add(0);
                            for (int i = (this.Parent == null && this.NumChars == 0 ? 1 : 0); i < value.Length; i++)
                            {
                                if (i >= xOffsets.Count)
                                {
                                    xOffsets.Add(spacing + (char.IsWhiteSpace(value[i]) ? wordSpacing : 0));
                                }
                                else
                                {
                                    xOffsets[i] += spacing + (char.IsWhiteSpace(value[i]) ? wordSpacing : 0);
                                }
                            }
                        }

                        rotations = GetValues(value.Length, e => e._rotations);

                        // Calculate Y-offset due to baseline shift. Don't inherit the value so that it is not accumulated multiple times.
                        var baselineShiftText = Element.BaselineShift.Trim().ToLower();
                        if (string.IsNullOrEmpty(baselineShiftText))
                            baselineShiftText = "baseline";

                        switch (baselineShiftText)
                        {
                            case "baseline":
                                // do nothing
                                break;
                            case "sub":
                                baselineShift = new SvgUnit(SvgUnitType.Ex, 1).ToDeviceValue(this.Renderer, UnitRenderingType.Vertical, this.Element);
                                break;
                            case "super":
                                baselineShift = -1f * new SvgUnit(SvgUnitType.Ex, 1).ToDeviceValue(this.Renderer, UnitRenderingType.Vertical, this.Element);
                                break;
                            default:
                                var convert = new SvgUnitConverter();
                                var shiftUnit = (SvgUnit)convert.ConvertFromInvariantString(baselineShiftText);
                                baselineShift = -1f * shiftUnit.ToDeviceValue(this.Renderer, UnitRenderingType.Vertical, this.Element);
                                break;
                        }

                        if (baselineShift != 0.0f)
                        {
                            if (yOffsets.Any())
                            {
                                yOffsets[0] += baselineShift;
                            }
                            else
                            {
                                yOffsets.Add(baselineShift);
                            }
                        }
                    }
                    finally
                    {
                        this.Renderer.PopBoundable();
                    }

                    var xTextStart = Current.X;
                    // NOTE: Assuming a horizontal left-to-right font
                    // Render absolutely positioned items in the horizontal direction
                    var yPos = Current.Y;
                    for (int i = 0; i < xAnchors.Count - 1; i++)
                    {
                        FlushPath();
                        _xAnchor = xAnchors[i] + (xOffsets.Count > i ? xOffsets[i] : 0);
                        EnsurePath();
                        yPos = (yAnchors.Count > i ? yAnchors[i] : yPos) + (yOffsets.Count > i ? yOffsets[i] : 0);

                        xTextStart = xTextStart.Equals(Current.X) ? _xAnchor : xTextStart;
                        DrawStringOnCurrPath(value[i].ToString(), font, new PointF(_xAnchor, yPos),
                            fontBaselineHeight, (rotations.Count > i ? rotations[i] : rotations.LastOrDefault()));
                    }

                    // Render any remaining characters
                    var renderChar = 0;
                    var xPos = this.Current.X;
                    if (xAnchors.Any())
                    {
                        FlushPath();
                        renderChar = xAnchors.Count - 1;
                        xPos = xAnchors.Last();
                        _xAnchor = xPos;
                    }
                    EnsurePath();


                    // Render individual characters as necessary
                    var lastIndividualChar = renderChar + Math.Max(Math.Max(Math.Max(Math.Max(xOffsets.Count, yOffsets.Count), yAnchors.Count), rotations.Count) - renderChar - 1, 0);
                    if (rotations.LastOrDefault() != 0.0f || pathStats != null) lastIndividualChar = value.Length;
                    if (lastIndividualChar > renderChar)
                    {
                        var charBounds = font.MeasureCharacters(this.Renderer, value.Substring(renderChar, Math.Min(lastIndividualChar + 1, value.Length) - renderChar));
                        PointF pathPoint;
                        float rotation;
                        float halfWidth;
                        for (int i = renderChar; i < lastIndividualChar; i++)
                        {
                            xPos += (float)pathScale * (xOffsets.Count > i ? xOffsets[i] : 0) + (charBounds[i - renderChar].X - (i == renderChar ? 0 : charBounds[i - renderChar - 1].X));
                            yPos = (yAnchors.Count > i ? yAnchors[i] : yPos) + (yOffsets.Count > i ? yOffsets[i] : 0);
                            if (pathStats == null)
                            {
                                xTextStart = xTextStart.Equals(Current.X) ? xPos : xTextStart;
                                DrawStringOnCurrPath(value[i].ToString(), font, new PointF(xPos, yPos),
                                    fontBaselineHeight, (rotations.Count > i ? rotations[i] : rotations.LastOrDefault()));
                            }
                            else
                            {
                                xPos = Math.Max(xPos, 0);
                                halfWidth = charBounds[i - renderChar].Width / 2;
                                if (pathStats.OffsetOnPath(xPos + halfWidth))
                                {
                                    pathStats.LocationAngleAtOffset(xPos + halfWidth, out pathPoint, out rotation);
                                    pathPoint = new PointF((float)(pathPoint.X - halfWidth * Math.Cos(rotation * Math.PI / 180) - (float)pathScale * yPos * Math.Sin(rotation * Math.PI / 180)),
                                        (float)(pathPoint.Y - halfWidth * Math.Sin(rotation * Math.PI / 180) + (float)pathScale * yPos * Math.Cos(rotation * Math.PI / 180)));
                                    xTextStart = xTextStart.Equals(Current.X) ? pathPoint.X : xTextStart;
                                    DrawStringOnCurrPath(value[i].ToString(), font, pathPoint, fontBaselineHeight, rotation);
                                }
                            }
                        }

                        // Add the kerning to the next character
                        if (lastIndividualChar < value.Length)
                        {
                            xPos += charBounds[charBounds.Count - 1].X - charBounds[charBounds.Count - 2].X;
                        }
                        else
                        {
                            xPos += charBounds.Last().Width;
                        }
                    }

                    // Render the string normally
                    if (lastIndividualChar < value.Length)
                    {
                        xPos += (xOffsets.Count > lastIndividualChar ? xOffsets[lastIndividualChar] : 0);
                        yPos = (yAnchors.Count > lastIndividualChar ? yAnchors[lastIndividualChar] : yPos) +
                               (yOffsets.Count > lastIndividualChar ? yOffsets[lastIndividualChar] : 0);
                        xTextStart = xTextStart.Equals(Current.X) ? xPos : xTextStart;
                        DrawStringOnCurrPath(value.Substring(lastIndividualChar), font, new PointF(xPos, yPos),
                            fontBaselineHeight, rotations.LastOrDefault());
                        var bounds = font.MeasureString(this.Renderer, value.Substring(lastIndividualChar));
                        xPos += bounds.Width;
                    }


                    NumChars += value.Length;
                    // Undo any baseline shift.  This is not persisted, unlike normal vertical offsets.
                    this.Current = new PointF(xPos, yPos - baselineShift);
                    this.TextBounds = new RectangleF(xTextStart, 0, this.Current.X - xTextStart, 0);
                }
            }

            private void DrawStringOnCurrPath(string value, IFontDefn font, PointF location, float fontBaselineHeight, float rotation)
            {
                var drawPath = _currPath;
                if (rotation != 0.0f) drawPath = new GraphicsPath();
                font.AddStringToPath(this.Renderer, drawPath, value, new PointF(location.X, location.Y - fontBaselineHeight));
                if (rotation != 0.0f && drawPath.PointCount > 0)
                {
                    using (var matrix = new Matrix())
                    {
                        matrix.Translate(-1 * location.X, -1 * location.Y, MatrixOrder.Append);
                        matrix.Rotate(rotation, MatrixOrder.Append);
                        matrix.Translate(location.X, location.Y, MatrixOrder.Append);
                        drawPath.Transform(matrix);
                        _currPath.AddPath(drawPath, false);
                    }
                }

            }

            private void EnsurePath()
            {
                if (_currPath == null)
                {
                    _currPath = new GraphicsPath();
                    _currPath.StartFigure();

                    var currState = this;
                    while (currState != null && currState._xAnchor <= float.MinValue)
                    {
                        currState = currState.Parent;
                    }
                    currState._anchoredPaths.Add(_currPath);
                }
            }

            private void FlushPath()
            {
                if (_currPath != null)
                {
                    _currPath.CloseFigure();

                    // Abort on empty paths (e.g. rendering a space)
                    if (_currPath.PointCount < 1)
                    {
                        _anchoredPaths.Clear();
                        _xAnchor = float.MinValue;
                        _currPath = null;
                        return;
                    }

                    if (_xAnchor > float.MinValue)
                    {
                        float minX = float.MaxValue;
                        float maxX = float.MinValue;
                        RectangleF bounds;
                        foreach (var path in _anchoredPaths)
                        {
                            bounds = path.GetBounds();
                            if (bounds.Left < minX) minX = bounds.Left;
                            if (bounds.Right > maxX) maxX = bounds.Right;
                        }

                        var xOffset = 0f; //_xAnchor - minX;
                        switch (Element.TextAnchor)
                        {
                            case SvgTextAnchor.Middle:
                                if (_anchoredPaths.Count == 1) xOffset -= this.TextBounds.Width / 2;
                                else xOffset -= (maxX - minX) / 2;
                                break;
                            case SvgTextAnchor.End:
                                if (_anchoredPaths.Count == 1) xOffset -= this.TextBounds.Width;
                                else xOffset -= (maxX - minX);
                                break;
                        }

                        if (xOffset != 0)
                        {
                            using (var matrix = new Matrix())
                            {
                                matrix.Translate(xOffset, 0);
                                foreach (var path in _anchoredPaths)
                                {
                                    path.Transform(matrix);
                                }
                            }
                        }

                        _anchoredPaths.Clear();
                        _xAnchor = float.MinValue;

                    }

                    if (_finalPath == null)
                    {
                        _finalPath = _currPath;
                    }
                    else
                    {
                        _finalPath.AddPath(_currPath, false);
                    }

                    _currPath = null;
                }
            }

            private IList<float> GetValues(int maxCount, Func<SvgTextBase, IEnumerable<float>> listGetter)
            {
                var currState = this;
                int charCount = 0;
                var results = new List<float>();
                int resultCount = 0;

                while (currState != null)
                {
                    charCount += currState.NumChars;
                    results.AddRange(listGetter.Invoke(currState.Element).Skip(charCount).Take(maxCount));
                    if (results.Count > resultCount)
                    {
                        maxCount -= results.Count - resultCount;
                        charCount += results.Count - resultCount;
                        resultCount = results.Count;
                    }

                    if (maxCount < 1) return results;

                    currState = currState.Parent;
                }

                return results;
            }
            private IList<float> GetValues(int maxCount, Func<SvgTextBase, IEnumerable<SvgUnit>> listGetter, UnitRenderingType renderingType)
            {
                var currState = this;
                int charCount = 0;
                var results = new List<float>();
                int resultCount = 0;

                while (currState != null)
                {
                    charCount += currState.NumChars;
                    results.AddRange(listGetter.Invoke(currState.Element).Skip(charCount).Take(maxCount).Select(p => p.ToDeviceValue(currState.Renderer, renderingType, currState.Element)));
                    if (results.Count > resultCount)
                    {
                        maxCount -= results.Count - resultCount;
                        charCount += results.Count - resultCount;
                        resultCount = results.Count;
                    }

                    if (maxCount < 1) return results;

                    currState = currState.Parent;
                }

                return results;
            }
        }
    }
}
#endif
