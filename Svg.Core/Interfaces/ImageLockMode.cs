namespace Svg
{
    // Summary:
    //     Specifies flags that are passed to the flags parameter of the Overload:System.Drawing.Bitmap.LockBits
    //     method. The Overload:System.Drawing.Bitmap.LockBits method locks a portion
    //     of an image so that you can read or write the pixel data.
    public enum ImageLockMode
    {
        // Summary:
        //     Specifies that a portion of the image is locked for reading.
        ReadOnly = 1,
        //
        // Summary:
        //     Specifies that a portion of the image is locked for writing.
        WriteOnly = 2,
        //
        // Summary:
        //     Specifies that a portion of the image is locked for reading or writing.
        ReadWrite = 3,
        //
        // Summary:
        //     Specifies that the buffer used for reading or writing pixel data is allocated
        //     by the user. If this flag is set, the flags parameter of the Overload:System.Drawing.Bitmap.LockBits
        //     method serves as an input parameter (and possibly as an output parameter).
        //     If this flag is cleared, then the flags parameter serves only as an output
        //     parameter.
        UserInputBuffer = 4,
    }
}