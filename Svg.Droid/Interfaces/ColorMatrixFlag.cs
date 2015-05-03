
namespace System.Drawing.Imaging
{
    // Summary:
    //     Specifies the types of images and colors that will be affected by the color
    //     and grayscale adjustment settings of an System.Drawing.Imaging.ImageAttributes.
    public enum ColorMatrixFlag
    {
        // Summary:
        //     All color values, including gray shades, are adjusted by the same color-adjustment
        //     matrix.
        Default = 0,
        //
        // Summary:
        //     All colors are adjusted, but gray shades are not adjusted. A gray shade is
        //     any color that has the same value for its red, green, and blue components.
        SkipGrays = 1,
        //
        // Summary:
        //     Only gray shades are adjusted.
        AltGrays = 2,
    }
}