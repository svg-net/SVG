using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Svg.Transforms;

namespace Svg
{
    public interface ISvgTransformable
    {
        SvgTransformCollection Transforms { get; set; }
        void PushTransforms(Graphics graphics);
        void PopTransforms(Graphics graphics);
    }
}