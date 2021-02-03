using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Svg
{
    [TypeConverter(typeof(XmlSpaceHandlingConverter))]
    public enum XmlSpaceHandling
    {
        Default,
        Inherit,
        Preserve
    }
}
