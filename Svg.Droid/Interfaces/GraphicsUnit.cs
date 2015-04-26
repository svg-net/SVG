
namespace Svg
{

    // Summary:
    //     Specifies the unit of measure for the given data.
    public enum GraphicsUnit
    {
        // Summary:
        //     Specifies the world coordinate system unit as the unit of measure.
        World = 0,
        //
        // Summary:
        //     Specifies the unit of measure of the display device. Typically pixels for
        //     video displays, and 1/100 inch for printers.
        Display = 1,
        //
        // Summary:
        //     Specifies a device pixel as the unit of measure.
        Pixel = 2,
        //
        // Summary:
        //     Specifies a printer's point (1/72 inch) as the unit of measure.
        Point = 3,
        //
        // Summary:
        //     Specifies the inch as the unit of measure.
        Inch = 4,
        //
        // Summary:
        //     Specifies the document unit (1/300 inch) as the unit of measure.
        Document = 5,
        //
        // Summary:
        //     Specifies the millimeter as the unit of measure.
        Millimeter = 6,
    }
}