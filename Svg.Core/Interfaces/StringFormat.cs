
using System.Drawing;

namespace Svg
{
    public interface StringFormat
    {
        StringFormatFlags FormatFlags { get; set; }
        void SetMeasurableCharacterRanges(CharacterRange[] characterRanges);
    }
}