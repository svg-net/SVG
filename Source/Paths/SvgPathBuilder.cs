using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Svg.Pathing;

namespace Svg
{
    public static class PointFExtensions
    {
        public static string ToSvgString(this PointF p)
        {
            return p.X.ToString(CultureInfo.InvariantCulture) + " " + p.Y.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class SvgPathBuilder : TypeConverter
    {
        /// <summary>
        /// Parses the specified string into a collection of path segments.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing path data.</param>
        public static SvgPathSegmentList Parse(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            var segments = new SvgPathSegmentList();

            try
            {
                foreach (var commandSet in SplitCommands(path.TrimEnd(null)))
                    CreatePathSegment(commandSet[0], segments, new CoordinateParser(commandSet.Trim()));
            }
            catch (Exception exc)
            {
                Trace.TraceError("Error parsing path \"{0}\": {1}", path, exc.Message);
            }

            return segments;
        }

        private static void CreatePathSegment(char command, SvgPathSegmentList segments, CoordinateParser parser)
        {
            var isRelative = char.IsLower(command);
            var coords = new float[6];
            // http://www.w3.org/TR/SVG11/paths.html#PathDataGeneralInformation

            switch (command)
            {
                case 'M': // moveto
                case 'm': // relative moveto
                    if (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                        segments.Add(new SvgMoveToSegment(ToAbsolute(coords[0], coords[1], segments, isRelative)));

                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'A': // elliptical arc
                case 'a': // relative elliptical arc
                    bool size;
                    bool sweep;

                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetBool(out size) &&
                           parser.TryGetBool(out sweep) && parser.TryGetFloat(out coords[3]) &&
                           parser.TryGetFloat(out coords[4]))
                    {
                        // A|a rx ry x-axis-rotation large-arc-flag sweep-flag x y
                        segments.Add(new SvgArcSegment(segments.Last.End, coords[0], coords[1], coords[2],
                            size ? SvgArcSize.Large : SvgArcSize.Small,
                            sweep ? SvgArcSweep.Positive : SvgArcSweep.Negative,
                            ToAbsolute(coords[3], coords[4], segments, isRelative)));
                    }
                    break;
                case 'L': // lineto
                case 'l': // relative lineto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'H': // horizontal lineto
                case 'h': // relative horizontal lineto
                    while (parser.TryGetFloat(out coords[0]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], segments.Last.End.Y, segments, isRelative, false)));
                    }
                    break;
                case 'V': // vertical lineto
                case 'v': // relative vertical lineto
                    while (parser.TryGetFloat(out coords[0]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(segments.Last.End.X, coords[0], segments, false, isRelative)));
                    }
                    break;
                case 'Q': // quadratic bézier curveto
                case 'q': // relative quadratic bézier curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]))
                    {
                        segments.Add(new SvgQuadraticCurveSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative)));
                    }
                    break;
                case 'T': // shorthand/smooth quadratic bézier curveto
                case 't': // relative shorthand/smooth quadratic bézier curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        var lastQuadCurve = segments.Last as SvgQuadraticCurveSegment;

                        var controlPoint = lastQuadCurve != null
                            ? Reflect(lastQuadCurve.ControlPoint, segments.Last.End)
                            : segments.Last.End;

                        segments.Add(new SvgQuadraticCurveSegment(segments.Last.End, controlPoint,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'C': // curveto
                case 'c': // relative curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]) &&
                           parser.TryGetFloat(out coords[4]) && parser.TryGetFloat(out coords[5]))
                    {
                        segments.Add(new SvgCubicCurveSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative),
                            ToAbsolute(coords[4], coords[5], segments, isRelative)));
                    }
                    break;
                case 'S': // shorthand/smooth curveto
                case 's': // relative shorthand/smooth curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]))
                    {
                        var lastCubicCurve = segments.Last as SvgCubicCurveSegment;

                        var controlPoint = lastCubicCurve != null
                            ? Reflect(lastCubicCurve.SecondControlPoint, segments.Last.End)
                            : segments.Last.End;

                        segments.Add(new SvgCubicCurveSegment(segments.Last.End, controlPoint,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative)));
                    }
                    break;
                case 'Z': // closepath
                case 'z': // relative closepath
                    segments.Add(new SvgClosePathSegment());
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
                if (lastSegment is SvgClosePathSegment)
                    lastSegment = segments.Reverse().OfType<SvgMoveToSegment>().First();

                if (isRelativeX)
                    point.X += lastSegment.End.X;

                if (isRelativeY)
                    point.Y += lastSegment.End.Y;
            }

            return point;
        }

        private static IEnumerable<string> SplitCommands(string path)
        {
            var commandStart = 0;

            for (var i = 0; i < path.Length; ++i)
            {
                if (char.IsLetter(path[i]) && path[i] != 'e' && path[i] != 'E') // e is used in scientific notiation. but not svg path
                {
                    var command = path.Substring(commandStart, i - commandStart).Trim();
                    commandStart = i;

                    if (!string.IsNullOrEmpty(command))
                        yield return command;

                    if (path.Length == i + 1)
                        yield return path[i].ToString();
                }
                else if (path.Length == i + 1)
                {
                    var command = path.Substring(commandStart, i - commandStart + 1).Trim();

                    if (!string.IsNullOrEmpty(command))
                        yield return command;
                }
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return Parse((string)value);

            return base.ConvertFrom(context, culture, value);
        }
    }
}
