using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Holds a dictionary of the default values of the SVG specification 
    /// </summary>
    public static class SvgDefaults
    {
        // defaults that are specific to some elements
        private static Dictionary<string, Dictionary<string, string>> _propDefaults = 
            new Dictionary<string, Dictionary<string, string>>
            {
                { "SvgRadialGradientServer", new Dictionary<string, string>
                { { "cx", "50%" }, { "cy", "50%" }, { "r", "50%" } } },
                { "SvgLinearGradientServer", new Dictionary<string, string>
                { { "x1", "0%" }, { "x2", "100%" }, { "y1", "0%" }, { "y2", "100%" }  } },
            };

        // common defaults
        private static readonly Dictionary<string, string> _defaults =
            new Dictionary<string, string>()
            {
                { "d", "" },
                { "viewBox", "0, 0, 0, 0" },
                { "visibility", "visible" },
                { "display", "inline" },
                { "enable-background", "accumulate" },
                { "opacity", "1" },
                { "clip", "auto" },
                { "clip-rule", "nonzero" },
                { "clipPathUnits", "userSpaceOnUse" },
                { "transform", "" },

                // line
                { "x1", "0" },
                { "x2", "0" },
                { "y1", "0" },
                { "y2", "0" },

                // circle, ellipse
                { "cx", "0" },
                { "cy", "0" },

                { "fill", "" },
                { "fill-opacity", "1" },
                { "fill-rule", "nonzero" },

                { "stop-color", "black" },
                { "stop-opacity", "1" },

                { "stroke", "none" },
                { "stroke-opacity", "1" },
                { "stroke-width", "1" },
                { "stroke-miterlimit", "4" },
                { "stroke-linecap", "butt" },
                { "stroke-linejoin", "miter" },
                { "stroke-dasharray", "none" },
                { "stroke-dashoffset", "0" },

                // marker
                { "markerUnits", "strokeWidth" },
                { "refX", "0" },
                { "refY", "0" },
                { "markerWidth", "3" },
                { "markerHeight", "3" },
                { "orient", "0" }
            };


        static SvgDefaults()
        {
        }

        /// <summary>
        /// Checks whether the property value is the default value of the svg definition.
        /// </summary>
        /// <param name="attributeName">Name of the svg attribute</param>
        /// <param name="componentType">Class name of the svg element</param>
        /// <param name="value">.NET value of the attribute</param>
        public static bool IsDefault(string attributeName, string componentType, string value)
        {
            if (_propDefaults.ContainsKey(componentType))
            {
                if (_propDefaults[componentType].ContainsKey(attributeName))
                {
                    return _propDefaults[componentType][attributeName] == value;
                }
            }

            if (_defaults.ContainsKey(attributeName))
            {
                return _defaults[attributeName] == value;
            }
            return false;
        }
    }
}
