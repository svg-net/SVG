using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Text anchor is used to align (start-, middle- or end-alignment) a string of text relative to a given point.
    /// </summary>
    public enum SvgTextAnchor
    {
        /// <summary>
        /// The rendered characters are aligned such that the start of the text string is at the initial current text position.
        /// </summary>
        Start,
        /// <summary>
        /// The rendered characters are aligned such that the middle of the text string is at the current text position.
        /// </summary>
        Middle,
        /// <summary>
        /// The rendered characters are aligned such that the end of the text string is at the initial current text position.
        /// </summary>
        End
    }
}