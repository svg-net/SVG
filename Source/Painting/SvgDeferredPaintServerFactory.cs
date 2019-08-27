using System.ComponentModel;
using System.Globalization;

namespace Svg
{
    internal class SvgDeferredPaintServerFactory : SvgPaintServerFactory
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var s = ((string)value).Trim();
                if (!string.IsNullOrEmpty(s))
                    return new SvgDeferredPaintServer(s);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
