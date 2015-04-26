
using System.Drawing;

namespace Svg
{
    public interface ColorBlend
    {

        // Summary:
        //     Gets or sets an array of colors that represents the colors to use at corresponding
        //     positions along a gradient.
        //
        // Returns:
        //     An array of System.Drawing.Color structures that represents the colors to
        //     use at corresponding positions along a gradient.
        Color[] Colors { get; set; }
        //
        // Summary:
        //     Gets or sets the positions along a gradient line.
        //
        // Returns:
        //     An array of values that specify percentages of distance along the gradient
        //     line.
        float[] Positions { get; set; }
    }
}