using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Svg
{
    
    // Summary:
    //     Specifies the display and layout information for text strings.
    [Flags]
    public enum StringFormatFlags
    {
        // Summary:
        //     Text is displayed from right to left.
        DirectionRightToLeft = 1,
        //
        // Summary:
        //     Text is vertically aligned.
        DirectionVertical = 2,
        //
        // Summary:
        //     Parts of characters are allowed to overhang the string's layout rectangle.
        //     By default, characters are repositioned to avoid any overhang.
        FitBlackBox = 4,
        //
        // Summary:
        //     Control characters such as the left-to-right mark are shown in the output
        //     with a representative glyph.
        DisplayFormatControl = 32,
        //
        // Summary:
        //     Fallback to alternate fonts for characters not supported in the requested
        //     font is disabled. Any missing characters are displayed with the fonts missing
        //     glyph, usually an open square.
        NoFontFallback = 1024,
        //
        // Summary:
        //     Includes the trailing space at the end of each line. By default the boundary
        //     rectangle returned by the Overload:System.Drawing.Graphics.MeasureString
        //     method excludes the space at the end of each line. Set this flag to include
        //     that space in measurement.
        MeasureTrailingSpaces = 2048,
        //
        // Summary:
        //     Text wrapping between lines when formatting within a rectangle is disabled.
        //     This flag is implied when a point is passed instead of a rectangle, or when
        //     the specified rectangle has a zero line length.
        NoWrap = 4096,
        //
        // Summary:
        //     Only entire lines are laid out in the formatting rectangle. By default layout
        //     continues until the end of the text, or until no more lines are visible as
        //     a result of clipping, whichever comes first. Note that the default settings
        //     allow the last line to be partially obscured by a formatting rectangle that
        //     is not a whole multiple of the line height. To ensure that only whole lines
        //     are seen, specify this value and be careful to provide a formatting rectangle
        //     at least as tall as the height of one line.
        LineLimit = 8192,
        //
        // Summary:
        //     Overhanging parts of glyphs, and unwrapped text reaching outside the formatting
        //     rectangle are allowed to show. By default all text and glyph parts reaching
        //     outside the formatting rectangle are clipped.
        NoClip = 16384,
    }
}