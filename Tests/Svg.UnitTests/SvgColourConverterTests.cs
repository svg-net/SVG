using System.Drawing;
using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgColourConverterTests
    {
        [Test]
        public void ParseReturnsValidColor_html_4()
        {
            var aqua = SvgColourConverter.Parse("aqua");
            Assert.AreEqual(Color.Aqua, aqua);

            var black = SvgColourConverter.Parse("black");
            Assert.AreEqual(Color.Black, black);

            var blue = SvgColourConverter.Parse("blue");
            Assert.AreEqual(Color.Blue, blue);

            var fuchsia = SvgColourConverter.Parse("fuchsia");
            Assert.AreEqual(Color.Fuchsia, fuchsia);

            var gray = SvgColourConverter.Parse("gray");
            Assert.AreEqual(Color.Gray, gray);

            var green = SvgColourConverter.Parse("green");
            Assert.AreEqual(Color.Green, green);

            var lime = SvgColourConverter.Parse("lime");
            Assert.AreEqual(Color.Lime, lime);

            var maroon = SvgColourConverter.Parse("maroon");
            Assert.AreEqual(Color.Maroon, maroon);

            var navy = SvgColourConverter.Parse("navy");
            Assert.AreEqual(Color.Navy, navy);

            var olive = SvgColourConverter.Parse("olive");
            Assert.AreEqual(Color.Olive, olive);

            var purple = SvgColourConverter.Parse("purple");
            Assert.AreEqual(Color.Purple, purple);

            var red = SvgColourConverter.Parse("red");
            Assert.AreEqual(Color.Red, red);

            var silver = SvgColourConverter.Parse("silver");
            Assert.AreEqual(Color.Silver, silver);

            var teal = SvgColourConverter.Parse("teal");
            Assert.AreEqual(Color.Teal, teal);

            var white = SvgColourConverter.Parse("white");
            Assert.AreEqual(Color.White, white);

            var yellow = SvgColourConverter.Parse("yellow");
            Assert.AreEqual(Color.Yellow, yellow);
        }

        [Test]
        public void ParseReturnsValidColor_system_colors()
        {
            var activeBorder = SvgColourConverter.Parse("ActiveBorder");
            Assert.AreEqual(SystemColors.ActiveBorder, activeBorder);

            var activeCaption = SvgColourConverter.Parse("ActiveCaption");
            Assert.AreEqual(SystemColors.ActiveCaption, activeCaption);

            var appWorkspace = SvgColourConverter.Parse("AppWorkspace");
            Assert.AreEqual(SystemColors.AppWorkspace, appWorkspace);

            // TODO: Check if SystemColors.Desktop is valid color for Background.
            var background = SvgColourConverter.Parse("Background");
            Assert.AreEqual(SystemColors.Desktop, background);

            var buttonFace = SvgColourConverter.Parse("ButtonFace");
            Assert.AreEqual(SystemColors.ButtonFace, buttonFace);

            var buttonHighlight = SvgColourConverter.Parse("ButtonHighlight");
            Assert.AreEqual(SystemColors.ControlLightLight, buttonHighlight);

            var buttonShadow = SvgColourConverter.Parse("ButtonShadow");
            Assert.AreEqual(SystemColors.ControlDark, buttonShadow);

            // TODO: Check if SystemColors.ControlText is valid color for ButtonText.
            var buttonText = SvgColourConverter.Parse("ButtonText");
            Assert.AreEqual(SystemColors.ControlText, buttonText);

            // TODO: Check if SystemColors.ActiveCaptionText is valid color for CaptionText.
            var captionText = SvgColourConverter.Parse("CaptionText");
            Assert.AreEqual(SystemColors.ActiveCaptionText, captionText);

            var grayText = SvgColourConverter.Parse("GrayText");
            Assert.AreEqual(SystemColors.GrayText, grayText);

            var highlight = SvgColourConverter.Parse("Highlight");
            Assert.AreEqual(SystemColors.Highlight, highlight);

            var highlightText = SvgColourConverter.Parse("HighlightText");
            Assert.AreEqual(SystemColors.HighlightText, highlightText);

            var inactiveBorder = SvgColourConverter.Parse("InactiveBorder");
            Assert.AreEqual(SystemColors.InactiveBorder, inactiveBorder);

            var inactiveCaption = SvgColourConverter.Parse("InactiveCaption");
            Assert.AreEqual(SystemColors.InactiveCaption, inactiveCaption);

            var inactiveCaptionText = SvgColourConverter.Parse("InactiveCaptionText");
            Assert.AreEqual(SystemColors.InactiveCaptionText, inactiveCaptionText);

            // TODO: Check if SystemColors.Info is valid color for InfoBackground.
            var infoBackground = SvgColourConverter.Parse("InfoBackground");
            Assert.AreEqual(SystemColors.Info, infoBackground);

            var infoText = SvgColourConverter.Parse("InfoText");
            Assert.AreEqual(SystemColors.InfoText, infoText);

            var menu = SvgColourConverter.Parse("Menu");
            Assert.AreEqual(SystemColors.Menu, menu);

            var menuText = SvgColourConverter.Parse("MenuText");
            Assert.AreEqual(SystemColors.MenuText, menuText);

            var scrollbar = SvgColourConverter.Parse("ScrollBar");
            Assert.AreEqual(SystemColors.ScrollBar, scrollbar);

            // TODO: Check if SystemColors.ControlDarkDark is valid color for ThreeDDarkShadow.
            var threeDDarkShadow = SvgColourConverter.Parse("ThreeDDarkShadow");
            Assert.AreEqual(SystemColors.ControlDarkDark, threeDDarkShadow);

            // TODO: Check if SystemColors.Control is valid color for ThreeDFace.
            var threeDFace = SvgColourConverter.Parse("ThreeDFace");
            Assert.AreEqual(SystemColors.Control, threeDFace);

            // TODO: Check if SystemColors.ControlLight is valid color for ThreeDHighlight.
            var threeDHighlight = SvgColourConverter.Parse("ThreeDHighlight");
            Assert.AreEqual(SystemColors.ControlLight, threeDHighlight);

            // TODO: Check if SystemColors.ControlLightLight is valid color for ThreeDLightShadow.
            var threeDLightShadow = SvgColourConverter.Parse("ThreeDLightShadow");
            Assert.AreEqual(SystemColors.ControlLightLight, threeDLightShadow);

            // TODO: Check if SystemColors.?? is valid color for ThreeDShadow.
            // var threeDShadow = SvgColourConverter.Parse("ThreeDDarkShadow");
            // Assert.AreEqual(SystemColors.??, threeDShadow);

            var window = SvgColourConverter.Parse("Window");
            Assert.AreEqual(SystemColors.Window, window);

            var windowFrame = SvgColourConverter.Parse("WindowFrame");
            Assert.AreEqual(SystemColors.WindowFrame, windowFrame);

            var windowText = SvgColourConverter.Parse("WindowText");
            Assert.AreEqual(SystemColors.WindowText, windowText);
        }

        [Test]
        public void ParseReturnsValidColor_hex_rgb()
        {
            var color1 = SvgColourConverter.Parse("#f00");
            Assert.AreEqual(Color.Red, color1);

            var color2 = SvgColourConverter.Parse("#fb0");
            Assert.AreEqual(Color.FromArgb(0xFF, 0xFF, 0xBB, 0x00), color2);

            var color3 = SvgColourConverter.Parse("#fff");
            Assert.AreEqual(Color.White, color3);
        }

        [Test]
        public void ParseReturnsValidColor_hex_rrggbb()
        {
            var color1 = SvgColourConverter.Parse("#ff0000");
            Assert.AreEqual(Color.Red, color1);

            var color2 = SvgColourConverter.Parse("#00ff00");
            Assert.AreEqual(Color.Lime, color2);

            var color3 = SvgColourConverter.Parse("#0000ff");
            Assert.AreEqual(Color.Blue, color3);

            var color4 = SvgColourConverter.Parse("#000000");
            Assert.AreEqual(Color.Black, color4);

            var color5 = SvgColourConverter.Parse("#ffffff");
            Assert.AreEqual(Color.White, color5);

            var color6 = SvgColourConverter.Parse("#ffbb00");
            Assert.AreEqual(Color.FromArgb(0xFF, 0xFF, 0xBB, 0x00), color6);
        }

        [Test]
        public void ParseReturnsValidColor_rgb_integer_range()
        {
            var color1 = SvgColourConverter.Parse("rgb(255,0,0)");
            Assert.AreEqual(Color.Red, color1);

            var color2 = SvgColourConverter.Parse("rgb(0,255,0)");
            Assert.AreEqual(Color.Lime, color2);

            var color3 = SvgColourConverter.Parse("rgb(0,0,255)");
            Assert.AreEqual(Color.Blue, color3);

            var color4 = SvgColourConverter.Parse("rgb(0,0,0)");
            Assert.AreEqual(Color.Black, color4);

            var color5 = SvgColourConverter.Parse("rgb(255,255,255)");
            Assert.AreEqual(Color.White, color5);

            var color6 = SvgColourConverter.Parse("rgb(300,0,0)");
            Assert.AreEqual(Color.Red, color6);

            var color7 = SvgColourConverter.Parse("rgb(255,-10,0)");
            Assert.AreEqual(Color.Red, color7);
        }

        [Test]
        public void ParseReturnsValidColor_rgb_float_range()
        {
            var color1 = SvgColourConverter.Parse("rgb(100%, 0%, 0%)");
            Assert.AreEqual(Color.Red, color1);
 
            var color2 = SvgColourConverter.Parse("rgb(0%, 100%, 0%)");
            Assert.AreEqual(Color.Lime, color2);

            var color3 = SvgColourConverter.Parse("rgb(0%, 0%, 100%)");
            Assert.AreEqual(Color.Blue, color3);

            var color4 = SvgColourConverter.Parse("rgb(100%, 100%, 100%)");
            Assert.AreEqual(Color.White, color4);

            var color5 = SvgColourConverter.Parse("rgb(0%, 0%, 0%)");
            Assert.AreEqual(Color.Black, color5);

            var color6 = SvgColourConverter.Parse("rgb(110%, 0%, 0%)");
            Assert.AreEqual(Color.Red, color6);

            var color7 = SvgColourConverter.Parse("rgb(110%, -10%, 0%)");
            Assert.AreEqual(Color.Red, color7);
        }

        [Test]
        public void ParseReturnsValidColor_rgb_hsl()
        {
             var color1 = SvgColourConverter.Parse("hsl(0, 100%, 50%)");
             Assert.AreEqual(Color.Red, color1);

             var color2 = SvgColourConverter.Parse("hsl(120, 100%, 50%)");
             Assert.AreEqual(Color.Lime, color2);

             var color3 = SvgColourConverter.Parse("hsl(240, 100%, 50%)");
             Assert.AreEqual(Color.Blue, color3);

             var color4 = SvgColourConverter.Parse("hsl(0, 0%, 100%)");
             Assert.AreEqual(Color.White, color4);

             var color5 = SvgColourConverter.Parse("hsl(0, 0%, 0%)");
             Assert.AreEqual(Color.Black, color5);

             var color6 = SvgColourConverter.Parse("hsl(359, 0%, 0%)");
             Assert.AreEqual(Color.Black, color6);

             var color7 = SvgColourConverter.Parse("hsl(180, 50%, 50%)");
             Assert.AreEqual(Color.FromArgb(255, 64, 191, 191), color7);

             var color8 = SvgColourConverter.Parse("hsl(400, 50%, 50%)");
             Assert.AreEqual(Color.FromArgb(255, 191, 149, 64), color8);

             var color9 = SvgColourConverter.Parse("hsl(180, -50%, 50%)");
             Assert.AreEqual(Color.Gray, color9);

             var color10 = SvgColourConverter.Parse("hsl(90, 50%, -50%)");
             Assert.AreEqual(Color.Black, color10);

             var color11 = SvgColourConverter.Parse("hsl(-30, 50%, 50%)");
             Assert.AreEqual(Color.FromArgb(255, 191, 64, 128), color11);
        }

        [Test]
        public void ParseReturnsValidColor_grey()
        {
            // gray

            var g1 = SvgColourConverter.Parse("grey");
            Assert.AreEqual(Color.Gray, g1);

            var g2 = SvgColourConverter.Parse("gray");
            Assert.AreEqual(Color.Gray, g2);

            var g3 = SvgColourConverter.Parse("Grey");
            Assert.AreEqual(Color.Gray, g3);

            var g4 = SvgColourConverter.Parse("Gray");
            Assert.AreEqual(Color.Gray, g4);

            // lightgray

            var lg1 = SvgColourConverter.Parse("lightgrey");
            Assert.AreEqual(Color.LightGray, lg1);

            var lg2 = SvgColourConverter.Parse("lightgray");
            Assert.AreEqual(Color.LightGray, lg2);

            var lg3 = SvgColourConverter.Parse("LightGrey");
            Assert.AreEqual(Color.LightGray, lg3);

            var lg4 = SvgColourConverter.Parse("LightGray");
            Assert.AreEqual(Color.LightGray, lg4);

            // darkslategray

            var dsg1 = SvgColourConverter.Parse("darkslategrey");
            Assert.AreEqual(Color.DarkSlateGray, dsg1);

            var dsg2 = SvgColourConverter.Parse("darkslategray");
            Assert.AreEqual(Color.DarkSlateGray, dsg2);

            var dsg3 = SvgColourConverter.Parse("DarkSlateGrey");
            Assert.AreEqual(Color.DarkSlateGray, dsg3);

            var dsg4 = SvgColourConverter.Parse("DarkSlateGray");
            Assert.AreEqual(Color.DarkSlateGray, dsg4);
        }
    }
}
