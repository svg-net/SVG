namespace Svg
{
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    [SvgElement("text")]
    public class SvgText : SvgTextBase
    {
        /// <summary>
        /// Initializes the <see cref="SvgText"/> class.
        /// </summary>
        public SvgText()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgText"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public SvgText(string text)
        {
            Text = text;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgText>();
        }
    }
}
