using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("missing-glyph")]
    public class SvgMissingGlyph : SvgGlyph
    {
        [SvgAttribute("glyph-name")]
        public override string GlyphName
        {
            get { return GetAttribute("glyph-name", Inherited, "__MISSING_GLYPH__"); }
            set { Attributes["glyph-name"] = value; }
        }
    }
}
