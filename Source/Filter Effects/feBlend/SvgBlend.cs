using System;
using System.Collections.Generic;
using System.Text;
using Svg;
using Svg.FilterEffects;

namespace Svg.FilterEffects
{
    [SvgElement("feBlend")]
    public class SvgBlend : SvgFilterPrimitive
    {
        [SvgAttribute("in2", SvgAttributeAttribute.SvgNamespace)]
        public string Input2 { get; set; }

        [SvgAttribute("mode", SvgAttributeAttribute.SvgNamespace)]
        public SvgBlendMode Mode { get; set; }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgBlend>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgBlend;
            newObj.Input = this.Input;
            newObj.Input2 = this.Input2;
            newObj.Mode = this.Mode;
            return newObj;
        }

        public override void Process(ImageBuffer buffer)
        {
            //TODO: Implement image processing for all modes
        }
    }
}
