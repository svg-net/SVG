using System;
using System.Drawing;

namespace Svg.FilterEffects
{
    [SvgElement("feFlood")]
	public class SvgFlood : SvgFilterPrimitive
    {
		/// <summary>
		/// The color to use across the entire filter primitive subregion.
		/// </summary>
		[SvgAttribute("flood-color")]
		public SvgPaintServer FloodColor { get; set; }


		/// <summary>
        /// The opacity value to use across the entire filter primitive subregion.
		/// </summary>
		[SvgAttribute("flood-opacity")]
        public float FloodOpacity { get; set; }


        public override void Process(ImageBuffer buffer)
		{
            var inputImage = buffer[this.Input];
            var result = new Bitmap(inputImage.Width, inputImage.Height);
            var color = SvgDeferredPaintServer.TryGet<SvgColourServer>(this.FloodColor, this).Colour;
            color = System.Drawing.Color.FromArgb((int)Math.Round(this.FloodOpacity * 255), color);
            using (var g = Graphics.FromImage(result))
            {
                g.Clear(color);
                g.Flush();
            }
            buffer[this.Result] = result;
		}


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgOffset>();
		}


        public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgFlood;
			newObj.FloodColor = this.FloodColor;
			newObj.FloodOpacity = this.FloodOpacity;
	
			return newObj;
		}

    }
}
