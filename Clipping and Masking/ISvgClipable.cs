using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public interface ISvgClipable
    {
        /// <summary>
        /// Gets or sets the ID of the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        Uri ClipPath { get; set; }
        void SetClip(SvgRenderer renderer);
        void ResetClip(SvgRenderer renderer);
    }
}