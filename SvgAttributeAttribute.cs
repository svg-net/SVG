using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    public class SvgAttributeAttribute : System.Attribute
    {
        private const string SVG_NAMESPACE = "http://www.w3.org/2000/svg";
        private string _name;
        private string _namespace;

        public override object TypeId
        {
            get
            {
                return base.TypeId;
            }
        }

        public override bool Match(object obj)
        {
            SvgAttributeAttribute indicator = obj as SvgAttributeAttribute;

            if (indicator == null)
                return false;

            // Always match if either value is String.Empty (wildcard)
            if (indicator.Name == String.Empty)
                return false;

            return String.Compare(indicator.Name, this.Name) == 0;
        }

        public string Name
        {
            get { return this._name; }
        }

        public string NameSpace
        {
            get { return this._namespace; }
        }

        internal SvgAttributeAttribute()
        {
            this._name = String.Empty;
        }

        internal SvgAttributeAttribute(string name)
        {
            this._name = name;
            this._namespace = SVG_NAMESPACE;
        }

        public SvgAttributeAttribute(string name, string nameSpace)
        {
            this._name = name;
            this._namespace = nameSpace;
        }
    }
}