using System;

namespace Svg
{
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    [SvgElement("textPath")]
    public partial class SvgTextPath : SvgTextBase
    {
        public override SvgUnitCollection Dx
        {
            get { return null; }
            set { /* do nothing */ }
        }

        [SvgAttribute("startOffset")]
        public virtual SvgUnit StartOffset
        {
            get { return base.Dx.Count < 1 ? SvgUnit.None : base.Dx[0]; }
            set
            {
                if (base.Dx.Count < 1)
                    base.Dx.Add(value);
                else
                    base.Dx[0] = value;
                Attributes["startOffset"] = value;
            }
        }

        [SvgAttribute("method")]
        public virtual SvgTextPathMethod Method
        {
            get { return GetAttribute("method", true, SvgTextPathMethod.Align); }
            set { Attributes["method"] = value; }
        }

        [SvgAttribute("spacing")]
        public virtual SvgTextPathSpacing Spacing
        {
            get { return GetAttribute("spacing", true, SvgTextPathSpacing.Exact); }
            set { Attributes["spacing"] = value; }
        }

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedPath
        {
            get { return GetAttribute<Uri>("href", false); }
            set { Attributes["href"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgTextPath>();
        }
    }
}
