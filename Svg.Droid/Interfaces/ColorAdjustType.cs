namespace System.Drawing.Imaging
{
    // Summary:
    //     Specifies which GDI+ objects use color adjustment information.
    public enum ColorAdjustType
    {
        // Summary:
        //     Color adjustment information that is used by all GDI+ objects that do not
        //     have their own color adjustment information.
        Default = 0,
        //
        // Summary:
        //     Color adjustment information for System.Drawing.Bitmap objects.
        Bitmap = 1,
        //
        // Summary:
        //     Color adjustment information for System.Drawing.Brush objects.
        Brush = 2,
        //
        // Summary:
        //     Color adjustment information for System.Drawing.Pen objects.
        Pen = 3,
        //
        // Summary:
        //     Color adjustment information for text.
        Text = 4,
        //
        // Summary:
        //     The number of types specified.
        Count = 5,
        //
        // Summary:
        //     The number of types specified.
        Any = 6,
    }
}