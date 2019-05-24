using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    public class GdiFontDefn : IFontDefn
    {
        private readonly Font _font;

        public float Size
        {
            get { return _font.Size; }
        }
        public float SizeInPoints
        {
            get { return _font.SizeInPoints; }
        }

        public GdiFontDefn(Font font)
        {
            _font = font;
        }

        public void AddStringToPath(ISvgRenderer renderer, GraphicsPath path, string text, PointF location)
        {
            path.AddString(text, _font.FontFamily, (int)_font.Style, _font.Size, location, StringFormat.GenericTypographic);
        }

        //Baseline calculation to match http://bobpowell.net/formattingtext.aspx
        public float Ascent(ISvgRenderer renderer)
        {
            var ff = _font.FontFamily;
            var ascent = ff.GetCellAscent(_font.Style);
            var baselineOffset = _font.SizeInPoints / ff.GetEmHeight(_font.Style) * ascent;
            return SvgDocument.PointsPerInch / 72f * baselineOffset;
        }

        public IList<RectangleF> MeasureCharacters(ISvgRenderer renderer, string text)
        {
            var g = GetGraphics(renderer);
            var regions = new List<RectangleF>();
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                for (var s = 0; s <= (text.Length - 1) / 32; s++)
                {
                    var numberOfChar = Math.Min(32, text.Length - 32 * s);
                    format.SetMeasurableCharacterRanges((from r in Enumerable.Range(32 * s, numberOfChar)
                                                         select new CharacterRange(r, 1)).ToArray());
                    regions.AddRange(from r in g.MeasureCharacterRanges(text, _font, new Rectangle(0, 0, numberOfChar * _font.Height, 1000), format)
                                     select r.GetBounds(g));
                }
            }
            return regions;
        }

        public SizeF MeasureString(ISvgRenderer renderer, string text)
        {
            var g = GetGraphics(renderer);
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.SetMeasurableCharacterRanges(new CharacterRange[] { new CharacterRange(0, text.Length) });
                format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                var r = g.MeasureCharacterRanges(text, _font, new Rectangle(0, 0, 1000, 1000), format);
                var rect = r[0].GetBounds(g);

                return new SizeF(rect.Width, Ascent(renderer));
            }
        }

        private Graphics GetGraphics(ISvgRenderer renderer)
        {
            var provider = renderer as IGraphicsProvider;
            if (provider == null)
            {
                throw new NotImplementedException("renderer is not IGraphicsProvider");
            }
            return provider.GetGraphics();
        }

        public void Dispose()
        {
            _font.Dispose();
        }
    }
}
