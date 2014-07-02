using System.ComponentModel;

namespace Svg.DataTypes
{
	[TypeConverter(typeof(SvgMarkerUnitsConverter))]
	public enum SvgMarkerUnits
	{
		strokeWidth,
		userSpaceOnUse
	}
}
