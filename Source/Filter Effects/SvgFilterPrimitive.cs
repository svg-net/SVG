using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    public abstract class SvgFilterPrimitive : SvgElement
    {
        public static readonly string SourceGraphic = "SourceGraphic";
        public static readonly string SourceAlpha = "SourceAlpha";
        public static readonly string BackgroundImage = "BackgroundImage";
        public static readonly string BackgroundAlpha = "BackgroundAlpha";
        public static readonly string FillPaint = "FillPaint";
        public static readonly string StrokePaint = "StrokePaint";

        [SvgAttribute("in")]
        public string Input
        {
            get { return this.Attributes.GetAttribute<string>("in"); }
            set { this.Attributes["in"] = value; }
        }

        [SvgAttribute("result")]
        public string Result
        {
            get { return this.Attributes.GetAttribute<string>("result"); }
            set { this.Attributes["result"] = value; }
        }

        protected SvgFilter Owner
        {
            get { return (SvgFilter)this.Parent; }
        }

        public abstract Bitmap Process();
    }
}