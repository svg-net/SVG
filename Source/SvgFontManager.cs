using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace Svg
{
    /// <summary>
    /// Manages access to <see cref="SystemFonts"/> and any privately loaded fonts.
    /// When a font is requested in the render process, if the font is not found as an embedded SvgFont, the render
    /// process will SvgFontManager.FindFont method.
    /// </summary>

    public static class SvgFontManager
    {
        private static readonly Dictionary<string, FontFamily> SystemFonts;
        public static Func<string, FontFamily> FontLoaderCallback;
        static SvgFontManager()
        {
            SystemFonts = FontFamily.Families.ToDictionary(ff => ff.Name.ToLower());
        }

        /// <summary>
        /// Loads a font from the given path.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the full path to the font file.</param>
        /// <returns>An <see cref="FontFamily"/> of the loaded font.</returns>
        public static FontFamily LoadFontFamily(string path)
        {
            var pfc = new PrivateFontCollection();
            var fp = Path.GetFullPath(path);
            pfc.AddFontFile(fp);
            return pfc.Families.Length == 0 ? null : pfc.Families[0];
        }

        /// <summary>
        /// This method searches a dictionary of fonts (pre loaded with the system fonts). If a
        /// font can't be found and a callback has been provided - then the callback should perform
        /// any validation and return a font (or null if not found/error).
        /// Where a font can't be located it is the responsibility of the caller to perform any
        /// exception handling.
        /// </summary>
        /// <param name="name">A <see cref="string"/> containing the FamilyName of the font.</param>
        /// <returns>An <see cref="FontFamily"/> of the loaded font or null is not located.</returns>
        public static FontFamily FindFont(string name)
        {
            if (name == null) return null;
            FontFamily ff = null;
            if (SystemFonts.TryGetValue(name.ToLower(), out ff)) return ff;
            if (FontLoaderCallback == null) return null;
            var ff2 = FontLoaderCallback(name);
            SystemFonts.Add(name.ToLower(), ff2);
            return ff2;
        }
    }
}