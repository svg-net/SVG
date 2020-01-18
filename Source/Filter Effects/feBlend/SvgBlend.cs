namespace Svg.FilterEffects
{
    [SvgElement("feBlend")]
    public class SvgBlend : SvgFilterPrimitive
    {
        private SvgBlendMode _mode = SvgBlendMode.Normal;

        [SvgAttribute("mode")]
        public SvgBlendMode BlendMode
        {
            get { return _mode; }
            set { _mode = value; Attributes["mode"] = value; }
        }

        [SvgAttribute("in2")]
        public string Input2
        {
            get { return GetAttribute<string>("in2", false); }
            set { Attributes["in2"] = value; }
        }

        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feBlend filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGaussianBlur>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgBlend;

            newObj._mode = _mode;
            newObj.Input2 = Input2;

            return newObj;
        }
    }
}
