namespace Svg.FilterEffects
{
    [SvgElement("feComposite")]
    public class SvgComposite : SvgFilterPrimitive
    {
        [SvgAttribute("operator")]
        public SvgCompositeOperator Operator
        {
            get { return GetAttribute("operator", false, SvgCompositeOperator.Over); }
            set { Attributes["operator"] = value; }
        }

        [SvgAttribute("k1")]
        public float K1
        {
            get { return GetAttribute("k1", false, 0f); }
            set { Attributes["k1"] = value; }
        }

        [SvgAttribute("k2")]
        public float K2
        {
            get { return GetAttribute("k2", false, 0f); }
            set { Attributes["k2"] = value; }
        }

        [SvgAttribute("k3")]
        public float K3
        {
            get { return GetAttribute("k3", false, 0f); }
            set { Attributes["k3"] = value; }
        }

        [SvgAttribute("k4")]
        public float K4
        {
            get { return GetAttribute("k4", false, 0f); }
            set { Attributes["k4"] = value; }
        }

        [SvgAttribute("in2")]
        public string Input2
        {
            get { return GetAttribute<string>("in2", false); }
            set { Attributes["in2"] = value; }
        }

        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feComposite filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgComposite>();
        }
    }
}
