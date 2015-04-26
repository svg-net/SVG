
using System;

namespace Svg
{ 
    public interface Font : IDisposable
    {
        float Size{ get; set; }
        float SizeInPoints { get; set; }
        FontStyle Style { get; set; }
        FontFamily FontFamily { get; set; }
    }
}