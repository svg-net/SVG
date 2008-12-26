using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

using Svg.Pathing;

namespace Svg
{
    internal class SvgPathBuilder : TypeConverter
    {
        /// <summary>
        /// Parses the specified string into a collection of path segments.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing path data.</param>
        public static SvgPathSegmentList Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            var segments = new SvgPathSegmentList();

            try
            {
                List<float> coords;
                char command;
                bool isRelative;

                foreach (var commandSet in SplitCommands(path.TrimEnd(null)))
                {
                    coords = new List<float>(ParseCoordinates(commandSet.Trim()));
                    command = commandSet[0];
                    isRelative = char.IsLower(command);
                    // http://www.w3.org/TR/SVG11/paths.html#PathDataGeneralInformation

                    switch (command)
                    {
                        case 'm': // relative moveto
                        case 'M': // moveto
                            segments.Add(
                                new SvgMoveToSegment(ToAbsolute(coords[0], coords[1], segments, isRelative)));

                            for (var i = 2; i < coords.Count; i += 2)
                            {
                                segments.Add(new SvgLineSegment(segments.Last.End,
                                    ToAbsolute(coords[i], coords[i + 1], segments, isRelative)));
                            }
                            break;
                        case 'a':
                        case 'A':
                            SvgArcSize size;
                            SvgArcSweep sweep;

                            for (var i = 0; i < coords.Count; i += 7)
                            {
                                size = (coords[i + 3] != 0.0f) ? SvgArcSize.Large : SvgArcSize.Small;
                                sweep = (coords[i + 4] != 0.0f) ? SvgArcSweep.Positive : SvgArcSweep.Negative;

                                // A|a rx ry x-axis-rotation large-arc-flag sweep-flag x y
                                segments.Add(new SvgArcSegment(segments.Last.End, coords[i], coords[i + 1], coords[i + 2],
                                    size, sweep, ToAbsolute(coords[i + 5], coords[i + 6], segments, isRelative)));
                            }
                            break;
                        case 'l': // relative lineto
                        case 'L': // lineto
                            for (var i = 0; i < coords.Count; i += 2)
                            {
                                segments.Add(new SvgLineSegment(segments.Last.End,
                                    ToAbsolute(coords[i], coords[i + 1], segments, isRelative)));
                            }
                            break;
                        case 'H': // horizontal lineto
                        case 'h': // relative horizontal lineto
                            foreach (var value in coords)
                                segments.Add(new SvgLineSegment(segments.Last.End,
                                    ToAbsolute(value, segments.Last.End.Y, segments, isRelative, false)));
                            break;
                        case 'V': // vertical lineto
                        case 'v': // relative vertical lineto
                            foreach (var value in coords)
                                segments.Add(new SvgLineSegment(segments.Last.End,
                                    ToAbsolute(segments.Last.End.X, value, segments, false, isRelative)));
                            break;
                        case 'Q': // curveto
                        case 'q': // relative curveto
                            for (var i = 0; i < coords.Count; i += 4)
                            {
                                segments.Add(new SvgQuadraticCurveSegment(segments.Last.End,
                                    ToAbsolute(coords[i], coords[i + 1], segments, isRelative),
                                    ToAbsolute(coords[i + 2], coords[i + 3], segments, isRelative)));
                            }
                            break;
                        case 'T': // shorthand/smooth curveto
                        case 't': // relative shorthand/smooth curveto
                            for (var i = 0; i < coords.Count; i += 2)
                            {
                                var lastQuadCurve = segments.Last as SvgQuadraticCurveSegment;

                                var controlPoint = lastQuadCurve != null
                                    ? Reflect(lastQuadCurve.ControlPoint, segments.Last.End)
                                    : segments.Last.End;

                                segments.Add(new SvgQuadraticCurveSegment(segments.Last.End, controlPoint,
                                    ToAbsolute(coords[i], coords[i + 1], segments, isRelative)));
                            }
                            break;
                        case 'C': // curveto
                        case 'c': // relative curveto
                            for (var i = 0; i < coords.Count; i += 6)
                            {
                                segments.Add(new SvgCubicCurveSegment(segments.Last.End,
                                    ToAbsolute(coords[i], coords[i + 1], segments, isRelative),
                                    ToAbsolute(coords[i + 2], coords[i + 3], segments, isRelative),
                                    ToAbsolute(coords[i + 4], coords[i + 5], segments, isRelative)));
                            }
                            break;
                        case 'S': // shorthand/smooth curveto
                        case 's': // relative shorthand/smooth curveto

                            for (var i = 0; i < coords.Count; i += 4)
                            {
                                var lastCubicCurve = segments.Last as SvgCubicCurveSegment;

                                var controlPoint = lastCubicCurve != null
                                    ? Reflect(lastCubicCurve.SecondControlPoint, segments.Last.End)
                                    : segments.Last.End;

                                segments.Add(new SvgCubicCurveSegment(segments.Last.End, controlPoint,
                                    ToAbsolute(coords[i], coords[i + 1], segments, isRelative),
                                    ToAbsolute(coords[i + 2], coords[i + 3], segments, isRelative)));
                            }
                            break;
                        case 'Z': // closepath
                        case 'z': // relative closepath
                            segments.Add(new SvgClosePathSegment());
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError("Error parsing path \"{0}\": {1}", path, exc.Message);
            }

            return segments;
        }

        private static PointF Reflect(PointF point, PointF mirror)
        {
            // TODO: Only works left to right???
            var x = mirror.X + (mirror.X - point.X);
            var y = mirror.Y + (mirror.Y - point.Y);

            return new PointF(Math.Abs(x), Math.Abs(y));
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

                if (isRelativeX)
                {
                    point.X += lastSegment.End.X;
                }

                if (isRelativeY)
                {
                    point.Y += lastSegment.End.Y;
                }
            }

            return point;
        }

        private static IEnumerable<string> SplitCommands(string path)
        {
            var commandStart = 0;

            for (var i = 0; i < path.Length; i++)
            {
                string command;
                if (char.IsLetter(path[i]))
                {
                    command = path.Substring(commandStart, i - commandStart).Trim();
                    commandStart = i;

                    if (!string.IsNullOrEmpty(command))
                    {
                        yield return command;
                    }

                    if (path.Length == i + 1)
                    {
                        yield return path[i].ToString();
                    }
                }
                else if (path.Length == i + 1)
                {
                    command = path.Substring(commandStart, i - commandStart + 1).Trim();

                    if (!string.IsNullOrEmpty(command))
                    {
                        yield return command;
                    }
                }
            }
        }

        private static IEnumerable<float> ParseCoordinates(string coords)
        {
            // TODO: Handle "1-1" (new PointF(1, -1);
            var parts = coords.Remove(0, 1).Replace("-", " -").Split(new[] { ',', ' ', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < parts.Length; i++)
            {
                yield return float.Parse(parts[i].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Parse((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}