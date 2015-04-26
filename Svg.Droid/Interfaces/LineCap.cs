
namespace Svg
{

    // Summary:
    //     Specifies the available cap styles with which a System.Drawing.Pen object
    //     can end a line.
    public enum LineCap
    {
        // Summary:
        //     Specifies a flat line cap.
        Flat = 0,
        //
        // Summary:
        //     Specifies a square line cap.
        Square = 1,
        //
        // Summary:
        //     Specifies a round line cap.
        Round = 2,
        //
        // Summary:
        //     Specifies a triangular line cap.
        Triangle = 3,
        //
        // Summary:
        //     Specifies no anchor.
        NoAnchor = 16,
        //
        // Summary:
        //     Specifies a square anchor line cap.
        SquareAnchor = 17,
        //
        // Summary:
        //     Specifies a round anchor cap.
        RoundAnchor = 18,
        //
        // Summary:
        //     Specifies a diamond anchor cap.
        DiamondAnchor = 19,
        //
        // Summary:
        //     Specifies an arrow-shaped anchor cap.
        ArrowAnchor = 20,
        //
        // Summary:
        //     Specifies a mask used to check whether a line cap is an anchor cap.
        AnchorMask = 240,
        //
        // Summary:
        //     Specifies a custom line cap.
        Custom = 255,
    }
}