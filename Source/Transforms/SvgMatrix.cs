using System.Collections.Generic;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies custom transform to this Matrix (Required for projects created by the Inkscape).
    /// </summary>
    public sealed partial class SvgMatrix : SvgTransform
    {
        public List<float> Points { get; set; }

        public override string WriteToString()
        {
            return $"matrix({Points[0].ToSvgString()}, {Points[1].ToSvgString()}, {Points[2].ToSvgString()}, {Points[3].ToSvgString()}, {Points[4].ToSvgString()}, {Points[5].ToSvgString()})";
        }

        public SvgMatrix(List<float> m)
        {
            Points = m;
        }

        public override object Clone()
        {
            return new SvgMatrix(Points);
        }
    }
}
