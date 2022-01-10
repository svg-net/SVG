using System.Linq;
using Svg.Pathing;

namespace Svg
{
    [SvgElement("glyph")]
    public partial class SvgGlyph : SvgPathBasedElement, ISvgPathElement
    {
        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d")]
        public SvgPathSegmentList PathData
        {
            get { return GetAttribute<SvgPathSegmentList>("d", false); }
            set
            {
                var old = PathData;
                if (old != null)
                    old.Owner = null;
                Attributes["d"] = value;
                value.Owner = this;
            }
        }

        [SvgAttribute("glyph-name")]
        public virtual string GlyphName
        {
            get { return GetAttribute<string>("glyph-name", true); }
            set { Attributes["glyph-name"] = value; }
        }

        [SvgAttribute("horiz-adv-x")]
        public float HorizAdvX
        {
            get { return GetAttribute("horiz-adv-x", true, Parents.OfType<SvgFont>().First().HorizAdvX); }
            set { Attributes["horiz-adv-x"] = value; }
        }

        [SvgAttribute("unicode")]
        public string Unicode
        {
            get { return GetAttribute<string>("unicode", true); }
            set { Attributes["unicode"] = value; }
        }

        [SvgAttribute("vert-adv-y")]
        public float VertAdvY
        {
            get { return GetAttribute("vert-adv-y", true, Parents.OfType<SvgFont>().First().VertAdvY); }
            set { Attributes["vert-adv-y"] = value; }
        }

        [SvgAttribute("vert-origin-x")]
        public float VertOriginX
        {
            get { return GetAttribute("vert-origin-x", true, Parents.OfType<SvgFont>().First().VertOriginX); }
            set { Attributes["vert-origin-x"] = value; }
        }

        [SvgAttribute("vert-origin-y")]
        public float VertOriginY
        {
            get { return GetAttribute("vert-origin-y", true, Parents.OfType<SvgFont>().First().VertOriginY); }
            set { Attributes["vert-origin-y"] = value; }
        }

        public void OnPathUpdated()
        {
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGlyph>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGlyph;

            if (newObj.PathData != null)
                newObj.PathData.Owner = newObj;
            return newObj;
        }
    }
}
