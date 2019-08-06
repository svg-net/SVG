using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Svg
{
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    [SvgElement("textPath")]
    public class SvgTextPath : SvgTextBase
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
            get { return GetAttribute("method", Inherited, SvgTextPathMethod.Align); }
            set { Attributes["method"] = value; }
        }

        [SvgAttribute("spacing")]
        public virtual SvgTextPathSpacing Spacing
        {
            get { return GetAttribute("spacing", Inherited, SvgTextPathSpacing.Exact); }
            set { Attributes["spacing"] = value; }
        }

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedPath
        {
            get { return GetAttribute<Uri>("href", false); }
            set { Attributes["href"] = value; }
        }

        protected override GraphicsPath GetBaselinePath(ISvgRenderer renderer)
        {
            var path = this.OwnerDocument.IdManager.GetElementById(this.ReferencedPath) as SvgVisualElement;
            if (path == null) return null;
            var pathData = (GraphicsPath)path.Path(renderer).Clone();
            if (path.Transforms != null && path.Transforms.Count > 0)
                pathData.Transform(path.Transforms.GetMatrix());
            return pathData;
        }
        protected override float GetAuthorPathLength()
        {
            var path = this.OwnerDocument.IdManager.GetElementById(this.ReferencedPath) as SvgPath;
            if (path == null) return 0;
            return path.PathLength;
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgTextPath>();
        }
    }
}
