using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Specifies the dominant baseline, which is used to align the baseline of the text content
    /// relative to the dominant baseline of the parent element.
    /// </summary>
    /// <remarks>
    /// The dominant baseline is used to determine or re-determine a scaled-baseline-table.
    /// A scaled-baseline-table is a compound value with three components:
    /// a baseline-identifier for the dominant-baseline, a baseline-table and a baseline-table font-size.
    /// </remarks>
    [TypeConverter(typeof(SvgDominantBaselineConverter))]
    public enum SvgDominantBaseline
    {
        /// <summary>
        /// If this property occurs on a text element, then the computed value depends on the value of the writing-mode attribute.
        /// If the writing-mode is horizontal, then the value of the dominant-baseline component is alphabetic,
        /// otherwise if the writing-mode is vertical, then the value of the dominant-baseline component is central.
        /// </summary>
        Auto,

        /// <summary>
        /// The dominant-baseline and the baseline-table components are set by determining the predominant script of the character data content.
        /// The writing-mode, whether horizontal or vertical, is used to select the appropriate set of baseline-tables
        /// and the dominant baseline is used to select the baseline-table that corresponds to that baseline.
        /// </summary>
        /// <remarks>This value is deprecated in SVG 2.</remarks>
        [Obsolete("This value is deprecated in SVG 2.")]
        UseScript,

        /// <summary>
        /// The dominant-baseline, the baseline-table, and the baseline-table font-size remain the same as that of the parent text content element.
        /// </summary>
        /// <remarks>This value is deprecated in SVG 2.</remarks>
        [Obsolete("This value is deprecated in SVG 2.")]
        NoChange,

        /// <summary>
        /// The dominant-baseline and the baseline-table remain the same,
        /// but the baseline-table font-size is changed to the value of the font-size attribute on this element.
        /// </summary>
        /// <remarks>This value is deprecated in SVG 2.</remarks>
        [Obsolete("This value is deprecated in SVG 2.")]
        ResetSize,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be ideographic,
        /// the derived baseline-table is constructed using the ideographic baseline-table in the font,
        /// and the baseline-table font-size is changed to the value of the font-size attribute on this element.
        /// </summary>
        Ideographic,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be alphabetic,
        /// the derived baseline-table is constructed using the alphabetic baseline-table in the font,
        /// and the baseline-table font-size is changed to the value of the font-size attribute on this element.
        /// </summary>
        Alphabetic,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be hanging,
        /// the derived baseline-table is constructed using the hanging baseline-table in the font,
        /// and the baseline-table font-size is changed to the value of the font-size attribute on this element.
        /// </summary>
        Hanging,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be mathematical,
        /// the derived baseline-table is constructed using the mathematical baseline-table in the font,
        /// and the baseline-table font-size is changed to the value of the font-size attribute on this element.
        /// </summary>
        Mathematical,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be central,
        /// and the derived baseline-table is constructed from the defined baselines in a baseline-table in the font.
        /// That font baseline-table is chosen using the following priority order of baseline-table names:
        /// ideographic, alphabetic, hanging, mathematical.
        /// </summary>
        Central,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be middle,
        /// and the derived baseline-table is constructed from the defined baselines in a baseline-table in the font.
        /// That font baseline-table is chosen using the following priority order of baseline-table names:
        /// ideographic, alphabetic, hanging, mathematical.
        /// </summary>
        Middle,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be text-after-edge.
        /// The derived baseline-table is constructed from the defined baselines in a baseline-table in the font.
        /// The choice of which font baseline-table to use from the baseline-tables in the font is browser dependent.
        /// </summary>
        /// <remarks>This value is from SVG 1.1 specification.</remarks>
        TextAfterEdge,

        /// <summary>
        /// The baseline-identifier for the dominant-baseline is set to be text-before-edge.
        /// The derived baseline-table is constructed from the defined baselines in a baseline-table in the font.
        /// The choice of which font baseline-table to use from the baseline-tables in the font is browser dependent.
        /// </summary>
        /// <remarks>This value is from SVG 1.1 specification.</remarks>
        TextBeforeEdge,

        /// <summary>
        /// Uses the bottom of the em box as the baseline. This aligns the bottom of the text with the specified position.
        /// </summary>
        /// <remarks>See <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Reference/Attribute/dominant-baseline">MDN documentation</see> for details.</remarks>
        TextBottom,

        /// <summary>
        /// Uses the top of the em box as the baseline. This aligns the top of the text with the specified position.
        /// </summary>
        /// <remarks>See <see href="https://developer.mozilla.org/en-US/docs/Web/SVG/Reference/Attribute/dominant-baseline">MDN documentation</see> for details.</remarks>
        TextTop,

        /// <summary>
        /// The value is inherited from the parent element.
        /// </summary>
        Inherit,
    }
}