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
