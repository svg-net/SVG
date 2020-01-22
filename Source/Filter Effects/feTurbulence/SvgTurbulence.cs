namespace Svg.FilterEffects
{
    [SvgElement("feTurbulence")]
    public class SvgTurbulence : SvgFilterPrimitive
    {
        [SvgAttribute("baseFrequency")]
        public SvgNumberCollection BaseFrequency
        {
            get { return GetAttribute("baseFrequency", false, new SvgNumberCollection() { 0f, 0f }); }
            set { Attributes["baseFrequency"] = value; }
        }

        [SvgAttribute("numOctaves")]
        public int NumOctaves
        {
            get { return GetAttribute("numOctaves", false, 1); }
            set { Attributes["numOctaves"] = value; }
        }

        [SvgAttribute("seed")]
        public float Seed
        {
            get { return GetAttribute("seed", false, 0f); }
            set { Attributes["seed"] = value; }
        }

        [SvgAttribute("stitchTiles")]
        public SvgStitchType StitchTiles
        {
            get { return GetAttribute("stitchTiles", false, SvgStitchType.NoStitch); }
            set { Attributes["stitchTiles"] = value; }
        }

        [SvgAttribute("type")]
        public SvgTurbulenceType Type
        {
            get { return GetAttribute("type", false, SvgTurbulenceType.Turbulence); }
            set { Attributes["type"] = value; }
        }

        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feTurbulence filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTurbulence>();
        }
    }
}
