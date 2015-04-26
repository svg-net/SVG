

namespace Svg
{

    // Summary:
    //     Specifies how to join consecutive line or curve segments in a figure (subpath)
    //     contained in a System.Drawing.Drawing2D.GraphicsPath object.
    public enum LineJoin
    {
        // Summary:
        //     Specifies a mitered join. This produces a sharp corner or a clipped corner,
        //     depending on whether the length of the miter exceeds the miter limit.
        Miter = 0,
        //
        // Summary:
        //     Specifies a beveled join. This produces a diagonal corner.
        Bevel = 1,
        //
        // Summary:
        //     Specifies a circular join. This produces a smooth, circular arc between the
        //     lines.
        Round = 2,
        //
        // Summary:
        //     Specifies a mitered join. This produces a sharp corner or a beveled corner,
        //     depending on whether the length of the miter exceeds the miter limit.
        MiterClipped = 3,
    }
}