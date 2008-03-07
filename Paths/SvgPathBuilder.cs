using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Text.RegularExpressions;
using System.Diagnostics;

using Svg.Pathing;

namespace Svg
{
    internal class SvgPathBuilder : TypeConverter
    {
        public static SvgPathSegmentList Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            SvgPathSegmentList segments = new SvgPathSegmentList();

            try
            {
                IEnumerable<PointF> coords;
                char command;
                PointF controlPoint;
                SvgQuadraticCurveSegment lastQuadCurve;
                SvgCubicCurveSegment lastCubicCurve;
                List<PointF> pointCache = new List<PointF>();

                foreach (string commandSet in SvgPathBuilder.SplitCommands(path.TrimEnd(null)))
                {
                    coords = SvgPathBuilder.ParseCoordinates(commandSet, segments);
                    command = commandSet[0];
                    // http://www.w3.org/TR/SVG11/paths.html#PathDataGeneralInformation

                    switch (command)
                    {
                        case 'm': // relative moveto
                        case 'M': // moveto
                            foreach (PointF point in coords)
                            {
                                segments.Add(new SvgMoveToSegment(point));
                            }
                            break;
                        case 'a':
                        case 'A':
                            throw new NotImplementedException("Arc segments are not yet implemented");
                        case 'l': // relative lineto
                        case 'L': // lineto
                            foreach (PointF point in coords)
                            {
                                segments.Add(new SvgLineSegment(segments.Last.End, point));
                            }
                            break;
                        case 'H': // horizontal lineto
                        case 'h': // relative horizontal lineto
                            foreach (PointF point in coords)
                            {
                                segments.Add(new SvgLineSegment(segments.Last.End, new PointF(segments.Last.End.X, point.Y)));
                            }
                            break;
                        case 'V': // vertical lineto
                        case 'v': // relative vertical lineto
                            foreach (PointF point in coords)
                            {
                                segments.Add(new SvgLineSegment(segments.Last.End, new PointF(point.X, segments.Last.End.Y)));
                            }
                            break;
                        case 'Q': // curveto
                        case 'q': // relative curveto
                            pointCache.Clear();
                            foreach (PointF point in coords) { pointCache.Add(point); }

                            for (int i = 0; i < pointCache.Count; i += 2)
                            {
                                segments.Add(new SvgQuadraticCurveSegment(segments.Last.End, pointCache[i], pointCache[i + 1]));
                            }
                            break;
                        case 'T': // shorthand/smooth curveto
                        case 't': // relative shorthand/smooth curveto
                            foreach (PointF point in coords)
                            {
                                lastQuadCurve = segments.Last as SvgQuadraticCurveSegment;

                                if (lastQuadCurve != null)
                                    controlPoint = Reflect(lastQuadCurve.ControlPoint, segments.Last.End);
                                else
                                    controlPoint = segments.Last.End;

                                segments.Add(new SvgQuadraticCurveSegment(segments.Last.End, controlPoint, point));
                            }
                            break;
                        case 'C': // curveto
                        case 'c': // relative curveto
                            pointCache.Clear();
                            foreach (PointF point in coords) { pointCache.Add(point); }

                            for (int i = 0; i < pointCache.Count; i += 3)
                            {
                                segments.Add(new SvgCubicCurveSegment(segments.Last.End, pointCache[i], pointCache[i + 1], pointCache[i + 2]));
                            }
                            break;
                        case 'S': // shorthand/smooth curveto
                        case 's': // relative shorthand/smooth curveto
                            pointCache.Clear();
                            foreach (PointF point in coords) { pointCache.Add(point); }

                            for (int i = 0; i < pointCache.Count; i += 2)
                            {
                                lastCubicCurve = segments.Last as SvgCubicCurveSegment;

                                if (lastCubicCurve != null)
                                {
                                    controlPoint = Reflect(lastCubicCurve.SecondControlPoint, segments.Last.End);
                                }
                                else
                                {
                                    controlPoint = segments.Last.End;
                                }

                                segments.Add(new SvgCubicCurveSegment(segments.Last.End, controlPoint, pointCache[i], pointCache[i + 1]));
                            }
                            break;
                        case 'Z': // closepath
                        case 'z': // relative closepath
                            segments.Add(new SvgClosePathSegment());
                            break;
                    }
                }
            }
            catch
            {
                Trace.TraceError("Error parsing path \"{0}\".", path);
            }

            return segments;
        }

        private static PointF Reflect(PointF point, PointF mirror)
        {
            // TODO: Only works left to right???
            float x = mirror.X + (mirror.X - point.X);
            float y = mirror.Y + (mirror.Y - point.Y);

            return new PointF(Math.Abs(x), Math.Abs(y));
        }

        private static PointF ToAbsolute(PointF point, SvgPathSegmentList segments)
        {
            PointF lastPoint = segments.Last.End;
            return new PointF(lastPoint.X + point.X, lastPoint.Y + point.Y);
        }

        private static IEnumerable<string> SplitCommands(string path)
        {
            int commandStart = 0;
            string command = null;

            for (int i = 0; i < path.Length; i++)
            {
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

        private static IEnumerable<PointF> ParseCoordinates(string coords, SvgPathSegmentList segments)
        {
            // TODO: Handle "1-1" (new PointF(1, -1);
            string[] parts = coords.Remove(0, 1).Replace("-", " -").Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            float x;
            float y;
            PointF point;
            bool relative = char.IsLower(coords[0]);

            for (int i = 0; i < parts.Length; i += 2)
            {
                x = float.Parse(parts[i]);
                y = float.Parse(parts[i + 1]);
                point = new PointF(x, y);

                if (relative)
                {
                    point = SvgPathBuilder.ToAbsolute(point, segments);
                }

                yield return point;
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return SvgPathBuilder.Parse((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}