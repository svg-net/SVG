
namespace Svg
{

    // Summary:
    //     Specifies whether smoothing (antialiasing) is applied to lines and curves
    //     and the edges of filled areas.
    public enum SmoothingMode
    {
        // Summary:
        //     Specifies an invalid mode.
        Invalid = -1,
        //
        // Summary:
        //     Specifies no antialiasing.
        Default = 0,
        //
        // Summary:
        //     Specifies no antialiasing.
        HighSpeed = 1,
        //
        // Summary:
        //     Specifies antialiased rendering.
        HighQuality = 2,
        //
        // Summary:
        //     Specifies no antialiasing.
        None = 3,
        //
        // Summary:
        //     Specifies antialiased rendering.
        AntiAlias = 4,
    }
}