using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NETFULL
using System.Drawing.Drawing2D;
using System.Drawing;
#else
using System.DrawingCore.Drawing2D;
using System.DrawingCore;
#endif

namespace Svg
{
    public interface IGraphicsProvider
    {
        Graphics GetGraphics();
    }
}
