using System.Collections.Generic;

#nullable enable

namespace Svg;

public class SvgOptions
{
    public SvgOptions()
    {
    }
    
    public SvgOptions(Dictionary<string, string> entities)
    {
        Entities = entities;
    }

    public SvgOptions(Dictionary<string, string> entities, string css)
    {
        Entities = entities;
        Css = css;
    }

    public SvgOptions(string css)
    {
        Css = css;
    }

    public Dictionary<string, string>? Entities { get; set; }
    public string? Css { get; set; }
}
