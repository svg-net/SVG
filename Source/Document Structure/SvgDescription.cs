using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    [DefaultProperty("Text")]
    [SvgElement("desc")]
    public class SvgDescription : SvgElement
    {
        private string _text;

        public string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }

        public override string ToString()
        {
            return this.Text;
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDescription>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgDescription;
            newObj.Text = this.Text;
            return newObj;
        }

    }
}