using System;
using System.Collections.Generic;
using Android.Graphics;

namespace Svg.Droid
{
    //http://stackoverflow.com/questions/3532397/how-to-retrieve-a-list-of-available-installed-fonts-in-android
    public class AndroidFontFamilyProvider : FontFamilyProvider
    {
        public IEnumerable<FontFamily> Families
        {
            get
            {
                return new List<FontFamily>()
                {
                    new AndroidFontFamily(Typeface.Default, "Default"), GenericSerif, GenericSansSerif, GenericMonospace,
                };
            }
        }

        public FontFamily GenericSerif { get { return new AndroidFontFamily(Typeface.Serif, "Serif"); } }
        public FontFamily GenericSansSerif { get { return new AndroidFontFamily(Typeface.Serif, "SansSerif"); } }
        public FontFamily GenericMonospace { get { return new AndroidFontFamily(Typeface.Serif, "Monospace"); } }
        public StringFormat GenericTypographic { get { throw new NotImplementedException();} }
    }
}