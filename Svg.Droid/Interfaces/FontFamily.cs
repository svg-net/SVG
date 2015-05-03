
using System.Collections;
using System.Collections.Generic;

namespace Svg
{
    public interface FontFamily
    {
        float GetCellAscent(FontStyle style);
        float GetEmHeight(FontStyle style);
        bool IsStyleAvailable(FontStyle fontStyle);
        string Name { get; set; }
    }

    public interface FontFamilyProvider
    {

        IEnumerable<FontFamily> Families { get; }
        FontFamily GenericSerif { get; set; }
        FontFamily GenericSansSerif { get; set; }
        FontFamily GenericMonospace { get; set; }
        StringFormat GenericTypographic { get; set; }
    }
}