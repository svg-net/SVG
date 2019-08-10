using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// An element used to define scripts within SVG documents.
    /// </summary>
    [SvgElement("script")]
    public class SvgScript : SvgElement
    {

        private string _scriptType;

        [SvgAttribute("type")]
        public string ScriptType  //TODO: Enum?
        { 
            get { return _scriptType; } 
            set { _scriptType = value; } 
        }

        private string _crossOrigin;

        [SvgAttribute("crossorigin")]
        public string CrossOrigin 
        {
            get { return _crossOrigin; }
            set { _crossOrigin = value; }
        }

        private string _href;
        [SvgAttribute("href")]
        public string Href //TODO: URI? 
        {
            get { return _href; }
            set { _href = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgScript>();        
        }
    }
}
