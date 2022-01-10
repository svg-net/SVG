namespace Svg
{
    [SvgElement("radialGradient")]
    public partial class SvgRadialGradientServer : SvgGradientServer
    {
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get { return GetAttribute("cx", false, new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["cx"] = value; }
        }

        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get { return GetAttribute("cy", false, new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["cy"] = value; }
        }

        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get { return GetAttribute("r", false, new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["r"] = value; }
        }

        [SvgAttribute("fx")]
        public SvgUnit FocalX
        {
            get
            {
                var value = GetAttribute("fx", false, SvgUnit.None);
                if (value.IsEmpty || value.IsNone)
                    value = CenterX;
                return value;
            }
            set { Attributes["fx"] = value; }
        }

        [SvgAttribute("fy")]
        public SvgUnit FocalY
        {
            get
            {
                var value = GetAttribute("fy", false, SvgUnit.None);
                if (value.IsEmpty || value.IsNone)
                    value = CenterY;
                return value;
            }
            set { Attributes["fy"] = value; }
        }

        [SvgAttribute("fr")]
        public SvgUnit FocalRadius
        {
            get { return GetAttribute("fr", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["fr"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgRadialGradientServer>();
        }
    }
}
