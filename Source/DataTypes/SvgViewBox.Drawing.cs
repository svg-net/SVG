using System;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial struct SvgViewBox
    {
        public void AddViewBoxTransform(SvgAspectRatio aspectRatio, ISvgRenderer renderer, SvgFragment frag)
        {
            var x = frag == null ? 0f : frag.X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, frag);
            var y = frag == null ? 0f : frag.Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, frag);

            if (Equals(Empty))
            {
                renderer.TranslateTransform(x, y, MatrixOrder.Prepend);
                return;
            }

            var width = frag == null ? Width : frag.Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, frag);
            var height = frag == null ? Height : frag.Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, frag);

            var fScaleX = width / Width;
            var fScaleY = height / Height; //(MinY < 0 ? -1 : 1) *
            var fMinX = -MinX * fScaleX;
            var fMinY = -MinY * fScaleY;

            aspectRatio = aspectRatio ?? new SvgAspectRatio(SvgPreserveAspectRatio.xMidYMid);
            if (aspectRatio.Align != SvgPreserveAspectRatio.none)
            {
                if (aspectRatio.Slice)
                {
                    fScaleX = Math.Max(fScaleX, fScaleY);
                    fScaleY = Math.Max(fScaleX, fScaleY);
                }
                else
                {
                    fScaleX = Math.Min(fScaleX, fScaleY);
                    fScaleY = Math.Min(fScaleX, fScaleY);
                }

                var fViewMidX = (Width / 2) * fScaleX;
                var fViewMidY = (Height / 2) * fScaleY;
                var fMidX = width / 2;
                var fMidY = height / 2;
                fMinX = -MinX * fScaleX;
                fMinY = -MinY * fScaleY;

                switch (aspectRatio.Align)
                {
                    case SvgPreserveAspectRatio.xMinYMin:
                        break;
                    case SvgPreserveAspectRatio.xMidYMin:
                        fMinX += fMidX - fViewMidX;
                        break;
                    case SvgPreserveAspectRatio.xMaxYMin:
                        fMinX += width - Width * fScaleX;
                        break;
                    case SvgPreserveAspectRatio.xMinYMid:
                        fMinY += fMidY - fViewMidY;
                        break;
                    case SvgPreserveAspectRatio.xMidYMid:
                        fMinX += fMidX - fViewMidX;
                        fMinY += fMidY - fViewMidY;
                        break;
                    case SvgPreserveAspectRatio.xMaxYMid:
                        fMinX += width - Width * fScaleX;
                        fMinY += fMidY - fViewMidY;
                        break;
                    case SvgPreserveAspectRatio.xMinYMax:
                        fMinY += height - Height * fScaleY;
                        break;
                    case SvgPreserveAspectRatio.xMidYMax:
                        fMinX += fMidX - fViewMidX;
                        fMinY += height - Height * fScaleY;
                        break;
                    case SvgPreserveAspectRatio.xMaxYMax:
                        fMinX += width - Width * fScaleX;
                        fMinY += height - Height * fScaleY;
                        break;
                    default:
                        break;
                }
            }

            renderer.TranslateTransform(x, y, MatrixOrder.Prepend);
            renderer.TranslateTransform(fMinX, fMinY, MatrixOrder.Prepend);
            renderer.ScaleTransform(fScaleX, fScaleY, MatrixOrder.Prepend);
        }
    }
}
