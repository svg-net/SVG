using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace Svg
{
    /// <summary>
    /// An element used to define scripts within SVG documents.
    /// Use the Script property to get the script content (proxies the content)
    /// </summary>
    [SvgElement("script")]
    public class SvgScript : SvgElement
    {

        public string Script 
        {
            get { return this.Content; }
            set { this.Content = value; }
        }

        private string _scriptType;

        [SvgAttribute("type")]
        public string ScriptType 
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
        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }
    
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgScript>();        
        }

        protected override void WriteAttributes(XmlTextWriter writer)
        {
            if(!string.IsNullOrEmpty(Href))
            {
                writer.WriteAttributeString("href", Href);
            }
            if(!string.IsNullOrEmpty(CrossOrigin))
            {
                writer.WriteAttributeString("crossorigin", CrossOrigin);
            }
            if(!string.IsNullOrEmpty(ScriptType))
            {
                writer.WriteAttributeString("type", ScriptType);
            }
        }

        protected override void WriteChildren(System.Xml.XmlTextWriter writer)
        {
            if(!string.IsNullOrEmpty(Content))
            {
                //Always put the script in a CDATA tag
                writer.WriteCData(this.Content);
            }
        }
    }
}
