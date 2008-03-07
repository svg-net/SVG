using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public interface ISvgClipable
    {
        SvgClipPath ClipPath { get; set; }
        void SetClip(Graphics graphics);
        void ResetClip(Graphics graphics);
    }
}