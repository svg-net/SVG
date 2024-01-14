#if !NO_SDC
using System.Drawing;

namespace Svg.FilterEffects
{
    public partial class SvgOffset : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            var inputImage = buffer[this.Input];
            var result = new Bitmap(inputImage.Width, inputImage.Height);

            var pts = new PointF[] { new PointF(this.Dx.ToDeviceValue(null, UnitRenderingType.Horizontal, null),
                this.Dy.ToDeviceValue(null, UnitRenderingType.Vertical, null)) };
            using (var transform = buffer.Transform)
                transform.TransformVectors(pts);

            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(inputImage, new Rectangle((int)pts[0].X, (int)pts[0].Y,
                        inputImage.Width, inputImage.Height),
                    0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel);
                g.Flush();
            }
            buffer[this.Result] = result;
        }
    }
}
#endif
