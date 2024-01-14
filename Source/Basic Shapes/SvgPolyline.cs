namespace Svg
{
    /// <summary>
    /// SvgPolyline defines a set of connected straight line segments. Typically, <see cref="SvgPolyline"/> defines open shapes.
    /// </summary>
    [SvgElement("polyline")]
    public partial class SvgPolyline : SvgPolygon
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgPolyline>();
        }
    }
}
