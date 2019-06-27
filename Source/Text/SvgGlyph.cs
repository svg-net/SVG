using System.Drawing.Drawing2D;
using System.Linq;
using Svg.Pathing;

namespace Svg
{
    [SvgElement("glyph")]
    public class SvgGlyph : SvgPathBasedElement, ISvgPathElement
    {
        private GraphicsPath _path;

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
            get { return GetAttribute<string>("glyph-name", Inherited); }
            set { Attributes["glyph-name"] = value; }
        }

        [SvgAttribute("horiz-adv-x")]
        public float HorizAdvX
        {
            get { return GetAttribute("horiz-adv-x", Inherited, Parents.OfType<SvgFont>().First().HorizAdvX); }
            set { Attributes["horiz-adv-x"] = value; }
        }

        [SvgAttribute("unicode")]
        public string Unicode
        {
            get { return GetAttribute<string>("unicode", Inherited); }
            set { Attributes["unicode"] = value; }
        }

        [SvgAttribute("vert-adv-y")]
        public float VertAdvY
        {
            get { return GetAttribute("vert-adv-y", Inherited, Parents.OfType<SvgFont>().First().VertAdvY); }
            set { Attributes["vert-adv-y"] = value; }
        }

        [SvgAttribute("vert-origin-x")]
        public float VertOriginX
        {
            get { return GetAttribute("vert-origin-x", Inherited, Parents.OfType<SvgFont>().First().VertOriginX); }
            set { Attributes["vert-origin-x"] = value; }
        }

        [SvgAttribute("vert-origin-y")]
        public float VertOriginY
        {
            get { return GetAttribute("vert-origin-y", Inherited, Parents.OfType<SvgFont>().First().VertOriginY); }
            set { Attributes["vert-origin-y"] = value; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                _path = new GraphicsPath();

                if (PathData != null)
                    foreach (var segment in PathData)
                        segment.AddToPath(_path);

                IsPathDirty = false;
            }
            return _path;
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
            if (PathData != null)
            {
                var pathData = new SvgPathSegmentList();
                foreach (var segment in PathData)
                    pathData.Add(segment.Clone());
                newObj.PathData = pathData;
            }
            return newObj;
        }
    }
}
