using System.ComponentModel;
using System.Globalization;

namespace Svg
{
    /// <summary>
    /// Factory class for &lt;IRI&gt;.
    /// </summary>
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
