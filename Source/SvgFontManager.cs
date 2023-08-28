#if !NO_SDC
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Svg
{
    /// <summary>
    /// Manages access to <see cref="SystemFonts"/> and any privately loaded fonts.
    /// When a font is requested in the render process, if the font is not found as an embedded SvgFont, the render
    /// process will SvgFontManager.FindFont method.
    /// </summary>
    public class SvgFontManager : IDisposable
    {
        private static readonly string[][] defaultLocalizedFamilyNames = new string[][]
        {
            // Japanese
            new string[]{ "Meiryo", "メイリオ", },
            new string[]{ "MS Gothic", "ＭＳ ゴシック", },
            new string[]{ "MS Mincho", "ＭＳ 明朝", },
        };

        public static List<string[]> LocalizedFamilyNames { get; private set; } = new List<string[]>();

        public static List<string> PrivateFontPathList { get; private set; } = new List<string>();

        public static List<byte[]> PrivateFontDataList { get; private set; } = new List<byte[]>();

        private readonly List<FontFamily> families = new List<FontFamily>();

        private readonly List<string[]> localizedFamilyNames = new List<string[]>();

        internal SvgFontManager()
        {
            families.AddRange(FontFamily.Families);

#if !NETSTANDARD
            using (var privateFontCollection = new PrivateFontCollection())
            {
                foreach (var path in PrivateFontPathList)
                    privateFontCollection.AddFontFile(path);

                foreach (var data in PrivateFontDataList)
                {
                    var memory = IntPtr.Zero;
                    try
                    {
                        memory = Marshal.AllocCoTaskMem(data.Length);
                        Marshal.Copy(data, 0, memory, data.Length);
                        privateFontCollection.AddMemoryFont(memory, data.Length);
                    }
                    finally
                    {
                        if (memory != IntPtr.Zero)
                            Marshal.FreeCoTaskMem(memory);
                    }
                }

                families.AddRange(privateFontCollection.Families);
            }
#endif

            localizedFamilyNames.AddRange(LocalizedFamilyNames);
            localizedFamilyNames.AddRange(defaultLocalizedFamilyNames);
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
        public FontFamily FindFont(string name)
        {
            if (name == null)
                return null;

            var familyNames = localizedFamilyNames.Find(f => f.Contains(name, StringComparer.CurrentCultureIgnoreCase))
                              ?? Enumerable.Repeat(name, 1);
            foreach (var familyName in familyNames)
            {
                var family = families.Find(f => f.Name.Equals(familyName, StringComparison.CurrentCultureIgnoreCase));
                if (family != null)
                    return family;
            }

            switch (name.ToLower())
            {
                case "serif":
                    return FontFamily.GenericSerif;
                case "sans-serif":
                    return FontFamily.GenericSansSerif;
                case "monospace":
                    return FontFamily.GenericMonospace;
            }

            return null;
        }

        public void Dispose()
        {
            families.ForEach(f => f.Dispose());
        }
    }
}
#endif
