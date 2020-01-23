using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    public abstract class SvgFilterPrimitive : SvgElement
    {
        public const string SourceGraphic = "SourceGraphic";
        public const string SourceAlpha = "SourceAlpha";
        public const string BackgroundImage = "BackgroundImage";
        public const string BackgroundAlpha = "BackgroundAlpha";
        public const string FillPaint = "FillPaint";
        public const string StrokePaint = "StrokePaint";

        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return GetAttribute("x", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return GetAttribute("y", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["y"] = value; }
        }

        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return GetAttribute("width", false, new SvgUnit(SvgUnitType.Percentage, 100f)); }
            set { Attributes["width"] = value; }
        }

        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return GetAttribute("height", false, new SvgUnit(SvgUnitType.Percentage, 100f)); }
            set { Attributes["height"] = value; }
        }

        [SvgAttribute("in")]
        public string Input
        {
            get { return GetAttribute<string>("in", false); }
            set { Attributes["in"] = value; }
        }

        [SvgAttribute("result")]
        public string Result
        {
            get { return GetAttribute<string>("result", false); }
            set { Attributes["result"] = value; }
        }

        protected SvgFilter Owner
        {
            get { return (SvgFilter)this.Parent; }
        }

        public abstract void Process(ImageBuffer buffer);
    }
}
