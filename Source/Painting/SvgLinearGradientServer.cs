using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Svg
{
    [SvgElement("linearGradient")]
    public partial class SvgLinearGradientServer : SvgGradientServer
    {
        [SvgAttribute("x1")]
        public SvgUnit X1
        {
            get { return GetAttribute("x1", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["x1"] = value; }
        }

        [SvgAttribute("y1")]
        public SvgUnit Y1
        {
            get { return GetAttribute("y1", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["y1"] = value; }
        }

        [SvgAttribute("x2")]
        public SvgUnit X2
        {
            get { return GetAttribute("x2", false, new SvgUnit(SvgUnitType.Percentage, 100f)); }
            set { Attributes["x2"] = value; }
        }

        [SvgAttribute("y2")]
        public SvgUnit Y2
        {
            get { return GetAttribute("y2", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["y2"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgLinearGradientServer>();
        }
    }
}
