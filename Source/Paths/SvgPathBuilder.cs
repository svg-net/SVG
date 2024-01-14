using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using Svg.Pathing;

namespace Svg
{
    public static class PointFExtensions
    {
        public static string ToSvgString(this float value)
        {
            // Use G7 format specifier to be compatible across all target frameworks.
            return value.ToString("G7", CultureInfo.InvariantCulture);
        }

        public static string ToSvgString(this PointF p)
        {
            return $"{p.X.ToSvgString()} {p.Y.ToSvgString()}";
        }
    }

    public class SvgPathBuilder : TypeConverter
    {
        /// <summary>
        /// Parses the specified string into a collection of path segments.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing path data.</param>
        public static SvgPathSegmentList Parse(ReadOnlySpan<char> path)
        {
            var segments = new SvgPathSegmentList();

            try
            {
                var pathTrimmed = path.TrimEnd();
                var commandStart = 0;
                var pathLength = pathTrimmed.Length;

                for (var i = 0; i < pathLength; ++i)
                {
                    var currentChar = pathTrimmed[i];
                    if (char.IsLetter(currentChar) && currentChar != 'e' && currentChar != 'E') // e is used in scientific notiation. but not svg path
                    {
                        var start = commandStart;
                        var length = i - commandStart;
                        var command = pathTrimmed.Slice(start, length).Trim();
                        commandStart = i;

                        if (command.Length > 0)
                        {
                            var commandSetTrimmed = pathTrimmed.Slice(start, length).Trim();
                            var state = new CoordinateParserState(ref commandSetTrimmed);
                            CreatePathSegment(commandSetTrimmed[0], segments, ref state, ref commandSetTrimmed);
                        }

                        if (pathLength == i + 1)
                        {
                            var commandSetTrimmed = pathTrimmed.Slice(i, 1).Trim();
                            var state = new CoordinateParserState(ref commandSetTrimmed);
                            CreatePathSegment(commandSetTrimmed[0], segments, ref state, ref commandSetTrimmed);
                        }
                    }
                    else if (pathLength == i + 1)
                    {
                        var start = commandStart;
                        var length = i - commandStart + 1;
                        var command = pathTrimmed.Slice(start, length).Trim();

                        if (command.Length > 0)
                        {
                            var commandSetTrimmed = pathTrimmed.Slice(start, length).Trim();
                            var state = new CoordinateParserState(ref commandSetTrimmed);
                            CreatePathSegment(commandSetTrimmed[0], segments, ref state, ref commandSetTrimmed);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError("Error parsing path \"{0}\": {1}", path.ToString(), exc.Message);
            }

            return segments;
        }

        private static void CreatePathSegment(char command, SvgPathSegmentList segments, ref CoordinateParserState state, ref ReadOnlySpan<char> chars)
        {
            var isRelative = char.IsLower(command);
            // http://www.w3.org/TR/SVG11/paths.html#PathDataGeneralInformation

            switch (command)
            {
                case 'M': // moveto
                case 'm': // relative moveto
                    {
                        if (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                         && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgMoveToSegment(
                                    isRelative, new PointF(coords0, coords1)));
                        }
                        while (CoordinateParser.TryGetFloat(out coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out coords1, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgLineSegment(
                                    isRelative, new PointF(coords0, coords1)));
                        }
                    }
                    break;
                case 'A': // elliptical arc
                case 'a': // relative elliptical arc
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords2, ref chars, ref state)
                            && CoordinateParser.TryGetBool(out var size, ref chars, ref state)
                            && CoordinateParser.TryGetBool(out var sweep, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords3, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords4, ref chars, ref state))
                        {
                            // A|a rx ry x-axis-rotation large-arc-flag sweep-flag x y
                            segments.Add(
                                new SvgArcSegment(
                                    coords0,
                                    coords1,
                                    coords2,
                                    size ? SvgArcSize.Large : SvgArcSize.Small,
                                    sweep ? SvgArcSweep.Positive : SvgArcSweep.Negative,
                                    isRelative, new PointF(coords3, coords4)));
                        }
                    }
                    break;
                case 'L': // lineto
                case 'l': // relative lineto
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgLineSegment(
                                    isRelative, new PointF(coords0, coords1)));
                        }
                    }
                    break;
                case 'H': // horizontal lineto
                case 'h': // relative horizontal lineto
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgLineSegment(
                                    isRelative, new PointF(coords0, float.NaN)));
                        }
                    }
                    break;
                case 'V': // vertical lineto
                case 'v': // relative vertical lineto
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgLineSegment(
                                    isRelative, new PointF(float.NaN, coords0)));
                        }
                    }
                    break;
                case 'Q': // quadratic bézier curveto
                case 'q': // relative quadratic bézier curveto
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords2, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords3, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgQuadraticCurveSegment(
                                    isRelative,
                                    new PointF(coords0, coords1),
                                    new PointF(coords2, coords3)));
                        }
                    }
                    break;
                case 'T': // shorthand/smooth quadratic bézier curveto
                case 't': // relative shorthand/smooth quadratic bézier curveto
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgQuadraticCurveSegment(
                                    isRelative, new PointF(coords0, coords1)));
                        }
                    }
                    break;
                case 'C': // curveto
                case 'c': // relative curveto
                    {
                    while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                        && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state)
                        && CoordinateParser.TryGetFloat(out var coords2, ref chars, ref state)
                        && CoordinateParser.TryGetFloat(out var coords3, ref chars, ref state)
                        && CoordinateParser.TryGetFloat(out var coords4, ref chars, ref state)
                        && CoordinateParser.TryGetFloat(out var coords5, ref chars, ref state))
                    {
                        segments.Add(
                            new SvgCubicCurveSegment(
                                isRelative,
                                new PointF(coords0, coords1),
                                new PointF(coords2, coords3),
                                new PointF(coords4, coords5)));
                    }
                    }
                    break;
                case 'S': // shorthand/smooth curveto
                case 's': // relative shorthand/smooth curveto
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords2, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords3, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgCubicCurveSegment(
                                    isRelative,
                                    new PointF(coords0, coords1),
                                    new PointF(coords2, coords3)));
                        }
                    }
                    break;
                case 'Z': // closepath
                case 'z': // relative closepath
                    {
                        segments.Add(new SvgClosePathSegment(isRelative));
                    }
                    break;
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
                return Parse(s.AsSpan());

            return base.ConvertFrom(context, culture, value);
        }
    }
}
