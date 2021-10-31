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
            return p.X.ToSvgString() + " " + p.Y.ToSvgString();
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
                                    ToAbsolute(coords0, coords1, segments, isRelative)));
                        }
                        while (CoordinateParser.TryGetFloat(out coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out coords1, ref chars, ref state))
                        {
                            segments.Add(
                                new SvgLineSegment(
                                    segments.Last.End,
                                    ToAbsolute(coords0, coords1, segments, isRelative)));
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
                                    segments.Last.End,
                                    coords0,
                                    coords1,
                                    coords2,
                                    size ? SvgArcSize.Large : SvgArcSize.Small,
                                    sweep ? SvgArcSweep.Positive : SvgArcSweep.Negative,
                                    ToAbsolute(coords3, coords4, segments, isRelative)));
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
                                    segments.Last.End,
                                    ToAbsolute(coords0, coords1, segments, isRelative)));
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
                                    segments.Last.End,
                                    ToAbsolute(coords0, segments.Last.End.Y, segments, isRelative, false)));
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
                                    segments.Last.End,
                                    ToAbsolute(segments.Last.End.X, coords0, segments, false, isRelative)));
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
                                    segments.Last.End,
                                    ToAbsolute(coords0, coords1, segments, isRelative),
                                    ToAbsolute(coords2, coords3, segments, isRelative)));
                        }
                    }
                    break;
                case 'T': // shorthand/smooth quadratic bézier curveto
                case 't': // relative shorthand/smooth quadratic bézier curveto
                    {
                        while (CoordinateParser.TryGetFloat(out var coords0, ref chars, ref state)
                            && CoordinateParser.TryGetFloat(out var coords1, ref chars, ref state))
                        {
                            var lastQuadCurve = segments.Last as SvgQuadraticCurveSegment;
                            var controlPoint = lastQuadCurve != null ? Reflect(lastQuadCurve.ControlPoint, segments.Last.End) : segments.Last.End;
                            segments.Add(
                                new SvgQuadraticCurveSegment(
                                    segments.Last.End,
                                    controlPoint,
                                    ToAbsolute(coords0, coords1, segments, isRelative)));
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
                                segments.Last.End,
                                ToAbsolute(coords0, coords1, segments, isRelative),
                                ToAbsolute(coords2, coords3, segments, isRelative),
                                ToAbsolute(coords4, coords5, segments, isRelative)));
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
                            var lastCubicCurve = segments.Last as SvgCubicCurveSegment;
                            var controlPoint = lastCubicCurve != null ? Reflect(lastCubicCurve.SecondControlPoint, segments.Last.End) : segments.Last.End;
                            segments.Add(
                                new SvgCubicCurveSegment(
                                    segments.Last.End,
                                    controlPoint,
                                    ToAbsolute(coords0, coords1, segments, isRelative),
                                    ToAbsolute(coords2, coords3, segments, isRelative)));
                        }
                    }
                    break;
                case 'Z': // closepath
                case 'z': // relative closepath
                    {
                        segments.Add(new SvgClosePathSegment());
                    }
                    break;
            }
        }

        private static PointF Reflect(PointF point, PointF mirror)
        {
            var dx = Math.Abs(mirror.X - point.X);
            var dy = Math.Abs(mirror.Y - point.Y);

            var x = mirror.X + (mirror.X >= point.X ? dx : -dx);
            var y = mirror.Y + (mirror.Y >= point.Y ? dy : -dy);

            return new PointF(x, y);
        }

        /// <summary>
        /// Creates point with absolute coorindates.
        /// </summary>
        /// <param name="x">Raw X-coordinate value.</param>
        /// <param name="y">Raw Y-coordinate value.</param>
        /// <param name="segments">Current path segments.</param>
        /// <param name="isRelativeBoth"><b>true</b> if <paramref name="x"/> and <paramref name="y"/> contains relative coordinate values, otherwise <b>false</b>.</param>
        /// <returns><see cref="PointF"/> that contains absolute coordinates.</returns>
        private static PointF ToAbsolute(float x, float y, SvgPathSegmentList segments, bool isRelativeBoth)
        {
            return ToAbsolute(x, y, segments, isRelativeBoth, isRelativeBoth);
        }

        /// <summary>
        /// Creates point with absolute coorindates.
        /// </summary>
        /// <param name="x">Raw X-coordinate value.</param>
        /// <param name="y">Raw Y-coordinate value.</param>
        /// <param name="segments">Current path segments.</param>
        /// <param name="isRelativeX"><b>true</b> if <paramref name="x"/> contains relative coordinate value, otherwise <b>false</b>.</param>
        /// <param name="isRelativeY"><b>true</b> if <paramref name="y"/> contains relative coordinate value, otherwise <b>false</b>.</param>
        /// <returns><see cref="PointF"/> that contains absolute coordinates.</returns>
        private static PointF ToAbsolute(float x, float y, SvgPathSegmentList segments, bool isRelativeX, bool isRelativeY)
        {
            var point = new PointF(x, y);

            if ((isRelativeX || isRelativeY) && segments.Count > 0)
            {
                var lastSegment = segments.Last;

                // if the last element is a SvgClosePathSegment the position of the previous element should be used because the position of SvgClosePathSegment is 0,0
                if (lastSegment is SvgClosePathSegment && segments.Count > 0)
                {
                    for (int i = segments.Count - 1; i >= 0; i--)
                    {
                        if (segments[i] is SvgMoveToSegment moveToSegment)
                        {
                            lastSegment = moveToSegment;
                            break;
                        }
                    }
                }

                if (isRelativeX)
                    point.X += lastSegment.End.X;

                if (isRelativeY)
                    point.Y += lastSegment.End.Y;
            }

            return point;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
                return Parse(s.AsSpan());

            return base.ConvertFrom(context, culture, value);
        }
    }
}
