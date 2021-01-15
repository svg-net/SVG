using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;

namespace Svg.Benchmark
{
    public class SvgColourConverterBenchmarks
    {
        private readonly CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private readonly SvgColourConverter _converter = new SvgColourConverter();

        [Benchmark]
        public void SvgColourConverter_Parse_html_4()
        {
           _converter.Parse(null, _cultureInfo, "aqua");
           _converter.Parse(null, _cultureInfo, "black");
           _converter.Parse(null, _cultureInfo, "blue");
           _converter.Parse(null, _cultureInfo, "fuchsia");
           _converter.Parse(null, _cultureInfo, "gray");
           _converter.Parse(null, _cultureInfo, "green");
           _converter.Parse(null, _cultureInfo, "lime");
           _converter.Parse(null, _cultureInfo, "maroon");
           _converter.Parse(null, _cultureInfo, "navy");
           _converter.Parse(null, _cultureInfo, "olive");
           _converter.Parse(null, _cultureInfo, "purple");
           _converter.Parse(null, _cultureInfo, "red");
           _converter.Parse(null, _cultureInfo, "silver");
           _converter.Parse(null, _cultureInfo, "teal");
           _converter.Parse(null, _cultureInfo, "white");
           _converter.Parse(null, _cultureInfo, "yellow");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_html_4_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "aqua");
            _converter.Parse_OLD(null, _cultureInfo, "black");
            _converter.Parse_OLD(null, _cultureInfo, "blue");
            _converter.Parse_OLD(null, _cultureInfo, "fuchsia");
            _converter.Parse_OLD(null, _cultureInfo, "gray");
            _converter.Parse_OLD(null, _cultureInfo, "green");
            _converter.Parse_OLD(null, _cultureInfo, "lime");
            _converter.Parse_OLD(null, _cultureInfo, "maroon");
            _converter.Parse_OLD(null, _cultureInfo, "navy");
            _converter.Parse_OLD(null, _cultureInfo, "olive");
            _converter.Parse_OLD(null, _cultureInfo, "purple");
            _converter.Parse_OLD(null, _cultureInfo, "red");
            _converter.Parse_OLD(null, _cultureInfo, "silver");
            _converter.Parse_OLD(null, _cultureInfo, "teal");
            _converter.Parse_OLD(null, _cultureInfo, "white");
            _converter.Parse_OLD(null, _cultureInfo, "yellow");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_DIRECT_first()
        {
            var colour = "ActiveBorder".AsSpan().Trim();
            SvgColourConverter.TryToGetSystemColor(ref colour, out var color);
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_DIRECT_middle()
        {
            var colour = "InactiveCaption".AsSpan().Trim();
            SvgColourConverter.TryToGetSystemColor(ref colour, out var color);
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_DIRECT_last()
        {
            var colour = "WindowText".AsSpan().Trim();
            SvgColourConverter.TryToGetSystemColor(ref colour, out var color);
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_first()
        {
            _converter.Parse(null, _cultureInfo, "ActiveBorder");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_middle()
        {
            _converter.Parse(null, _cultureInfo, "InactiveCaption");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_last()
        {
            _converter.Parse(null, _cultureInfo, "WindowText");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors()
        {
            _converter.Parse(null, _cultureInfo, "ActiveBorder");
            _converter.Parse(null, _cultureInfo, "ActiveCaption");
            _converter.Parse(null, _cultureInfo, "AppWorkspace");
            _converter.Parse(null, _cultureInfo, "Background");
            _converter.Parse(null, _cultureInfo, "ButtonFace");
            _converter.Parse(null, _cultureInfo, "ButtonHighlight");
            _converter.Parse(null, _cultureInfo, "ButtonShadow");
            _converter.Parse(null, _cultureInfo, "ButtonText");
            _converter.Parse(null, _cultureInfo, "CaptionText");
            _converter.Parse(null, _cultureInfo, "GrayText");
            _converter.Parse(null, _cultureInfo, "Highlight");
            _converter.Parse(null, _cultureInfo, "HighlightText");
            _converter.Parse(null, _cultureInfo, "InactiveBorder");
            _converter.Parse(null, _cultureInfo, "InactiveCaption");
            _converter.Parse(null, _cultureInfo, "InactiveCaptionText");
            _converter.Parse(null, _cultureInfo, "InfoBackground");
            _converter.Parse(null, _cultureInfo, "InfoText");
            _converter.Parse(null, _cultureInfo, "Menu");
            _converter.Parse(null, _cultureInfo, "MenuText");
            _converter.Parse(null, _cultureInfo, "ScrollBar");
            _converter.Parse(null, _cultureInfo, "ThreeDDarkShadow");
            _converter.Parse(null, _cultureInfo, "ThreeDFace");
            _converter.Parse(null, _cultureInfo, "ThreeDHighlight");
            _converter.Parse(null, _cultureInfo, "ThreeDLightShadow");
            // TODO: _converter.Parse(null, _cultureInfo, "ThreeDShadow");
            _converter.Parse(null, _cultureInfo, "Window");
            _converter.Parse(null, _cultureInfo, "WindowFrame");
            _converter.Parse(null, _cultureInfo, "WindowText");
        }
        
        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_DIRECT_first_OLD()
        {
            var colour = "ActiveBorder".Trim();
            SvgColourConverter.TryToGetSystemColor_OLD(colour, out var color);
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_DIRECT_middle_OLD()
        {
            var colour = "InactiveCaption".Trim();
            SvgColourConverter.TryToGetSystemColor_OLD(colour, out var color);
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_DIRECT_last_OLD()
        {
            var colour = "WindowText".Trim();
            SvgColourConverter.TryToGetSystemColor_OLD(colour, out var color);
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_first_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "ActiveBorder");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_middle_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "InactiveCaption");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_single_last_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "WindowText");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_system_colors_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "ActiveBorder");
            _converter.Parse_OLD(null, _cultureInfo, "ActiveCaption");
            _converter.Parse_OLD(null, _cultureInfo, "AppWorkspace");
            _converter.Parse_OLD(null, _cultureInfo, "Background");
            _converter.Parse_OLD(null, _cultureInfo, "ButtonFace");
            _converter.Parse_OLD(null, _cultureInfo, "ButtonHighlight");
            _converter.Parse_OLD(null, _cultureInfo, "ButtonShadow");
            _converter.Parse_OLD(null, _cultureInfo, "ButtonText");
            _converter.Parse_OLD(null, _cultureInfo, "CaptionText");
            _converter.Parse_OLD(null, _cultureInfo, "GrayText");
            _converter.Parse_OLD(null, _cultureInfo, "Highlight");
            _converter.Parse_OLD(null, _cultureInfo, "HighlightText");
            _converter.Parse_OLD(null, _cultureInfo, "InactiveBorder");
            _converter.Parse_OLD(null, _cultureInfo, "InactiveCaption");
            _converter.Parse_OLD(null, _cultureInfo, "InactiveCaptionText");
            _converter.Parse_OLD(null, _cultureInfo, "InfoBackground");
            _converter.Parse_OLD(null, _cultureInfo, "InfoText");
            _converter.Parse_OLD(null, _cultureInfo, "Menu");
            _converter.Parse_OLD(null, _cultureInfo, "MenuText");
            _converter.Parse_OLD(null, _cultureInfo, "Scrollbar");
            _converter.Parse_OLD(null, _cultureInfo, "ThreeDDarkShadow");
            _converter.Parse_OLD(null, _cultureInfo, "ThreeDFace");
            _converter.Parse_OLD(null, _cultureInfo, "ThreeDHighlight");
            _converter.Parse_OLD(null, _cultureInfo, "ThreeDLightShadow");
            // TODO: _converter.Parse_OLD(null, _cultureInfo, "ThreeDShadow");
            _converter.Parse_OLD(null, _cultureInfo, "Window");
            _converter.Parse_OLD(null, _cultureInfo, "WindowFrame");
            _converter.Parse_OLD(null, _cultureInfo, "WindowText");
        }
        
        [Benchmark]
        public void SvgColourConverter_Parse_hex_rgb()
        {
            _converter.Parse(null, _cultureInfo, "#f00");
            _converter.Parse(null, _cultureInfo, "#fb0");
            _converter.Parse(null, _cultureInfo, "#fff");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_hex_rgb_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "#f00");
            _converter.Parse_OLD(null, _cultureInfo, "#fb0");
            _converter.Parse_OLD(null, _cultureInfo, "#fff");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_hex_rrggbb()
        {
            _converter.Parse(null, _cultureInfo, "#ff0000");
            _converter.Parse(null, _cultureInfo, "#00ff00");
            _converter.Parse(null, _cultureInfo, "#0000ff");
            _converter.Parse(null, _cultureInfo, "#000000");
            _converter.Parse(null, _cultureInfo, "#ffffff");
            _converter.Parse(null, _cultureInfo, "#ffbb00");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_hex_rrggbb_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "#ff0000");
            _converter.Parse_OLD(null, _cultureInfo, "#00ff00");
            _converter.Parse_OLD(null, _cultureInfo, "#0000ff");
            _converter.Parse_OLD(null, _cultureInfo, "#000000");
            _converter.Parse_OLD(null, _cultureInfo, "#ffffff");
            _converter.Parse_OLD(null, _cultureInfo, "#ffbb00");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_rgb_integer_range()
        {
            _converter.Parse(null, _cultureInfo, "rgb(255,0,0)");
            _converter.Parse(null, _cultureInfo, "rgb(0,255,0)");
            _converter.Parse(null, _cultureInfo, "rgb(0,0,255)");
            _converter.Parse(null, _cultureInfo, "rgb(0,0,0)");
            _converter.Parse(null, _cultureInfo, "rgb(255,255,255)");
            // TODO: _converter.Parse(null, _cultureInfo, "rgb(300,0,0)");
            // TODO: _converter.Parse(null, _cultureInfo, "rgb(255,-10,0)");
        }
        
        [Benchmark]
        public void SvgColourConverter_Parse_rgb_integer_range_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "rgb(255,0,0)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(0,255,0)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(0,0,255)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(0,0,0)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(255,255,255)");
            // TODO: _converter.Parse_OLD(null, _cultureInfo, "rgb(300,0,0)");
            // TODO: _converter.Parse_OLD(null, _cultureInfo, "rgb(255,-10,0)");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_rgb_float_range()
        {
            _converter.Parse(null, _cultureInfo, "rgb(100%, 0%, 0%)");
            _converter.Parse(null, _cultureInfo, "rgb(0%, 100%, 0%)");
            _converter.Parse(null, _cultureInfo, "rgb(0%, 0%, 100%)");
            _converter.Parse(null, _cultureInfo, "rgb(100%, 100%, 100%)");
            _converter.Parse(null, _cultureInfo, "rgb(0%, 0%, 0%)");
            // TODO: _converter.Parse(null, _cultureInfo, "rgb(110%, 0%, 0%)");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_rgb_float_range_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "rgb(100%, 0%, 0%)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(0%, 100%, 0%)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(0%, 0%, 100%)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(100%, 100%, 100%)");
            _converter.Parse_OLD(null, _cultureInfo, "rgb(0%, 0%, 0%)");
            // TODO: _converter.Parse_OLD(null, _cultureInfo, "rgb(110%, 0%, 0%)");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_rgb_hsl()
        {
            _converter.Parse(null, _cultureInfo, "hsl(0, 100%, 50%)");
            _converter.Parse(null, _cultureInfo, "hsl(120, 100%, 50%)");
            _converter.Parse(null, _cultureInfo, "hsl(240, 100%, 50%)");
            _converter.Parse(null, _cultureInfo, "hsl(0, 0%, 100%)");
            _converter.Parse(null, _cultureInfo, "hsl(0, 0%, 0%)");
            _converter.Parse(null, _cultureInfo, "hsl(359, 0%, 0%)");
            _converter.Parse(null, _cultureInfo, "hsl(180, 50%, 50%)");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_rgb_hsl_OLD()
        {
            _converter.Parse_OLD(null, _cultureInfo, "hsl(0, 100%, 50%)");
            _converter.Parse_OLD(null, _cultureInfo, "hsl(120, 100%, 50%)");
            _converter.Parse_OLD(null, _cultureInfo, "hsl(240, 100%, 50%)");
            _converter.Parse_OLD(null, _cultureInfo, "hsl(0, 0%, 100%)");
            _converter.Parse_OLD(null, _cultureInfo, "hsl(0, 0%, 0%)");
            _converter.Parse_OLD(null, _cultureInfo, "hsl(359, 0%, 0%)");
            _converter.Parse_OLD(null, _cultureInfo, "hsl(180, 50%, 50%)");
        }
    }
}
