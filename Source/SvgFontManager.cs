using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Svg
{
    public static class SvgFontManager
    {
        private static readonly Dictionary<string, FontFamily> SystemFonts;
        public static Func<string, FontFamily> FontLoaderCallback;
        static SvgFontManager()
        {
            SystemFonts = FontFamily.Families.ToDictionary(ff => ff.Name.ToLower());
        }

        public static FontFamily AddFontFile(string path)
        {
            var pfc = new PrivateFontCollection();
            var fp = Path.GetFullPath(path);
            pfc.AddFontFile(fp);
            return pfc.Families.Length == 0 ? null : pfc.Families[0];
        }

        public static FontFamily FindFont(string name)
        {
            if (name == null) return null;
            if (SystemFonts.TryGetValue(name.ToLower(), out var ff)) return ff;
            if (FontLoaderCallback == null) return null;
            var ff2 = FontLoaderCallback(name);
            SystemFonts.Add(name.ToLower(), ff2);
            return ff2;
        }
    }
}