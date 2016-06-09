

namespace Svg
{

    // Summary:
    //     Specifies the quality of text rendering.
    public enum TextRenderingHint
    {
        // Summary:
        //     Each character is drawn using its glyph bitmap, with the system default rendering
        //     hint. The text will be drawn using whatever font-smoothing settings the user
        //     has selected for the system.
        SystemDefault = 0,
        //
        // Summary:
        //     Each character is drawn using its glyph bitmap. Hinting is used to improve
        //     character appearance on stems and curvature.
        SingleBitPerPixelGridFit = 1,
        //
        // Summary:
        //     Each character is drawn using its glyph bitmap. Hinting is not used.
        SingleBitPerPixel = 2,
        //
        // Summary:
        //     Each character is drawn using its antialiased glyph bitmap with hinting.
        //     Much better quality due to antialiasing, but at a higher performance cost.
        AntiAliasGridFit = 3,
        //
        // Summary:
        //     Each character is drawn using its antialiased glyph bitmap without hinting.
        //     Better quality due to antialiasing. Stem width differences may be noticeable
        //     because hinting is turned off.
        AntiAlias = 4,
        //
        // Summary:
        //     Each character is drawn using its glyph ClearType bitmap with hinting. The
        //     highest quality setting. Used to take advantage of ClearType font features.
        ClearTypeGridFit = 5,
    }
}