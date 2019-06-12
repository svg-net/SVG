using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Svg.FilterEffects
{
    [SvgElement("feMergeNode")]
    public class SvgMergeNode : SvgElement
    {
        [SvgAttribute("in")]
        public string Input
        {
            get { return GetAttribute<string>("in", false); }
            set { Attributes["in"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            throw new NotImplementedException();
        }

    }
}
