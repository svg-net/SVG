namespace Svg
{
    [SvgElement("radialGradient")]
    public partial class SvgRadialGradientServer : SvgGradientServer
    {
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get { return GetAttribute("cx", false, SvgDeferredPaintServer.TryGet<SvgRadialGradientServer>(InheritGradient, null)?.CenterX ?? new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["cx"] = value; }
        }

        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get { return GetAttribute("cy", false, SvgDeferredPaintServer.TryGet<SvgRadialGradientServer>(InheritGradient, null)?.CenterY ?? new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["cy"] = value; }
        }

        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get { return GetAttribute("r", false, SvgDeferredPaintServer.TryGet<SvgRadialGradientServer>(InheritGradient, null)?.Radius ?? new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["r"] = value; }
        }

        [SvgAttribute("fx")]
        public SvgUnit FocalX
        {
            get
            {
                var value = GetAttribute("fx", false, SvgDeferredPaintServer.TryGet<SvgRadialGradientServer>(InheritGradient, null)?.FocalX ?? SvgUnit.None);
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
                var value = GetAttribute("fy", false, SvgDeferredPaintServer.TryGet<SvgRadialGradientServer>(InheritGradient, null)?.FocalY ?? SvgUnit.None);
                if (value.IsEmpty || value.IsNone)
                    value = CenterY;
                return value;
            }
            set { Attributes["fy"] = value; }
        }

        [SvgAttribute("fr")]
        public SvgUnit FocalRadius
        {
            get { return GetAttribute("fr", false, SvgDeferredPaintServer.TryGet<SvgRadialGradientServer>(InheritGradient, null)?.FocalRadius ?? new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["fr"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgRadialGradientServer>();
        }
    }
}
