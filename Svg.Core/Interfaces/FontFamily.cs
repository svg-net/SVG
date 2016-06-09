
using System.Collections;
using System.Collections.Generic;

namespace Svg
{
    public interface FontFamily
    {
        float GetCellAscent(FontStyle style);
        float GetEmHeight(FontStyle style);
        bool IsStyleAvailable(FontStyle fontStyle);
        string Name { get;  }
    }

    public interface FontFamilyProvider
    {

        IEnumerable<FontFamily> Families { get; }
        FontFamily GenericSerif { get;  }
        FontFamily GenericSansSerif { get; }
        FontFamily GenericMonospace { get; }
        StringFormat GenericTypographic { get; }
    }
}