using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;

namespace Svg.Benchmark
{
    public class SvgColourConverterBenchmarks
    {
        private readonly CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private readonly SvgColourConverter _converter = new SvgColourConverter();
#if true
        [Benchmark]
        public void SvgColourConverter_Parse_html_4()
        {
           _converter.Parse(null, _cultureInfo, "aqua".AsSpan());
           _converter.Parse(null, _cultureInfo, "black".AsSpan());
           _converter.Parse(null, _cultureInfo, "blue".AsSpan());
           _converter.Parse(null, _cultureInfo, "fuchsia".AsSpan());
           _converter.Parse(null, _cultureInfo, "gray".AsSpan());
           _converter.Parse(null, _cultureInfo, "green".AsSpan());
           _converter.Parse(null, _cultureInfo, "lime".AsSpan());
           _converter.Parse(null, _cultureInfo, "maroon".AsSpan());
           _converter.Parse(null, _cultureInfo, "navy".AsSpan());
           _converter.Parse(null, _cultureInfo, "olive".AsSpan());
           _converter.Parse(null, _cultureInfo, "purple".AsSpan());
           _converter.Parse(null, _cultureInfo, "red".AsSpan());
           _converter.Parse(null, _cultureInfo, "silver".AsSpan());
           _converter.Parse(null, _cultureInfo, "teal".AsSpan());
           _converter.Parse(null, _cultureInfo, "white".AsSpan());
           _converter.Parse(null, _cultureInfo, "yellow".AsSpan());
        }
        
        [Benchmark]
        public void SvgColourConverter_Parse_system_colors()
        {
            _converter.Parse(null, _cultureInfo, "ActiveBorder".AsSpan());
            _converter.Parse(null, _cultureInfo, "ActiveCaption".AsSpan());
            _converter.Parse(null, _cultureInfo, "AppWorkspace".AsSpan());
            _converter.Parse(null, _cultureInfo, "Background".AsSpan());
            _converter.Parse(null, _cultureInfo, "ButtonFace".AsSpan());
            _converter.Parse(null, _cultureInfo, "ButtonHighlight".AsSpan());
            _converter.Parse(null, _cultureInfo, "ButtonShadow".AsSpan());
            _converter.Parse(null, _cultureInfo, "ButtonText".AsSpan());
            _converter.Parse(null, _cultureInfo, "CaptionText".AsSpan());
            _converter.Parse(null, _cultureInfo, "GrayText".AsSpan());
            _converter.Parse(null, _cultureInfo, "Highlight".AsSpan());
            _converter.Parse(null, _cultureInfo, "HighlightText".AsSpan());
            _converter.Parse(null, _cultureInfo, "InactiveBorder".AsSpan());
            _converter.Parse(null, _cultureInfo, "InactiveCaption".AsSpan());
            _converter.Parse(null, _cultureInfo, "InactiveCaptionText".AsSpan());
            _converter.Parse(null, _cultureInfo, "InfoBackground".AsSpan());
            _converter.Parse(null, _cultureInfo, "InfoText".AsSpan());
            _converter.Parse(null, _cultureInfo, "Menu".AsSpan());
            _converter.Parse(null, _cultureInfo, "MenuText".AsSpan());
            _converter.Parse(null, _cultureInfo, "Scrollbar".AsSpan());
            _converter.Parse(null, _cultureInfo, "ThreeDDarkShadow".AsSpan());
            _converter.Parse(null, _cultureInfo, "ThreeDFace".AsSpan());
            _converter.Parse(null, _cultureInfo, "ThreeDHighlight".AsSpan());
            _converter.Parse(null, _cultureInfo, "ThreeDLightShadow".AsSpan());
            // TODO: _converter.Parse(null, _cultureInfo, "ThreeDShadow".AsSpan());
            _converter.Parse(null, _cultureInfo, "Window".AsSpan());
            _converter.Parse(null, _cultureInfo, "WindowFrame".AsSpan());
            _converter.Parse(null, _cultureInfo, "WindowText".AsSpan());
        }
        
        [Benchmark]
        public void SvgColourConverter_Parse_hex_rgb()
        {
            _converter.Parse(null, _cultureInfo, "#f00".AsSpan());
            _converter.Parse(null, _cultureInfo, "#fb0".AsSpan());
            _converter.Parse(null, _cultureInfo, "#fff".AsSpan());
            
        }

        [Benchmark]
        public void SvgColourConverter_Parse_hex_rrggbb()
        {
            _converter.Parse(null, _cultureInfo, "#ff0000".AsSpan());
            _converter.Parse(null, _cultureInfo, "#00ff00".AsSpan());
            _converter.Parse(null, _cultureInfo, "#0000ff".AsSpan());
            _converter.Parse(null, _cultureInfo, "#000000".AsSpan());
            _converter.Parse(null, _cultureInfo, "#ffffff".AsSpan());
            _converter.Parse(null, _cultureInfo, "#ffbb00".AsSpan());
        }

        [Benchmark]
        public void SvgColourConverter_Parse_rgb_integer_range()
        {
            _converter.Parse(null, _cultureInfo, "rgb(255,0,0)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(0,255,0)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(0,0,255)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(0,0,0)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(255,255,255)".AsSpan());
            // TODO: _converter.Parse(null, _cultureInfo, "rgb(300,0,0)".AsSpan());
            // TODO: _converter.Parse(null, _cultureInfo, "rgb(255,-10,0)".AsSpan());
        }
        
        [Benchmark]
        public void SvgColourConverter_Parse_rgb_float_range()
        {
            _converter.Parse(null, _cultureInfo, "rgb(100%, 0%, 0%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(0%, 100%, 0%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(0%, 100%, 0%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(100%, 100%, 100%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "rgb(0%, 0%, 0%)".AsSpan());
            // TODO: _converter.Parse(null, _cultureInfo, "rgb(110%, 0%, 0%)".AsSpan());
        }

        [Benchmark]
        public void SvgColourConverter_Parse_hsl()
        {
            _converter.Parse(null, _cultureInfo, "hsl(0, 100%, 50%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "hsl(120, 100%, 50%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "hsl(240, 100%, 50%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "hsl(0, 0%, 100%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "hsl(0, 0%, 0%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "hsl(359, 0%, 0%)".AsSpan());
            _converter.Parse(null, _cultureInfo, "hsl(180, 50%, 50%)".AsSpan());
        }
#else
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
            _converter.Parse(null, _cultureInfo, "Scrollbar");
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
        public void SvgColourConverter_Parse_hex_rgb()
        {
            _converter.Parse(null, _cultureInfo, "#f00");
            _converter.Parse(null, _cultureInfo, "#fb0");
            _converter.Parse(null, _cultureInfo, "#fff");
            
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
        public void SvgColourConverter_Parse_rgb_float_range()
        {
            _converter.Parse(null, _cultureInfo, "rgb(100%, 0%, 0%)");
            _converter.Parse(null, _cultureInfo, "rgb(0%, 100%, 0%)");
            _converter.Parse(null, _cultureInfo, "rgb(0%, 100%, 0%)");
            _converter.Parse(null, _cultureInfo, "rgb(100%, 100%, 100%)");
            _converter.Parse(null, _cultureInfo, "rgb(0%, 0%, 0%)");
            // TODO: _converter.Parse(null, _cultureInfo, "rgb(110%, 0%, 0%)");
        }

        [Benchmark]
        public void SvgColourConverter_Parse_hsl()
        {
            _converter.Parse(null, _cultureInfo, "hsl(0, 100%, 50%)");
            _converter.Parse(null, _cultureInfo, "hsl(120, 100%, 50%)");
            _converter.Parse(null, _cultureInfo, "hsl(240, 100%, 50%)");
            _converter.Parse(null, _cultureInfo, "hsl(0, 0%, 100%)");
            _converter.Parse(null, _cultureInfo, "hsl(0, 0%, 0%)");
            _converter.Parse(null, _cultureInfo, "hsl(359, 0%, 0%)");
            _converter.Parse(null, _cultureInfo, "hsl(180, 50%, 50%)");
        }
#endif
    }
}
