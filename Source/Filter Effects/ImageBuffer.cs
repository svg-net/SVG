using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    public enum AlphaState
    {
        Straight,
        Premultiplied,
        AlphaOnly
    }

    public class ImageBuffer : IDictionary<string, Bitmap>
    {
        private const string BufferKey = "__!!BUFFER";

        private Dictionary<string, Bitmap> _images;
        private Dictionary<string, AlphaState> _alphaStates;
        private RectangleF _bounds;
        private ISvgRenderer _renderer;
        private Action<ISvgRenderer> _renderMethod;
        private float _inflate;

        public Matrix Transform { get; set; }

        public Bitmap Buffer
        {
            get { return _images[BufferKey]; }
        }

        public AlphaState GetAlphaState(string key)
        {
            if (!_alphaStates.ContainsKey(key))
            {
                _alphaStates[key] = AlphaState.Straight;
            }
            return _alphaStates[key];
        }

        public static string PremultipliedImageKey(string key)
        {
            return key + "_Premul";
        }

        public static string StraightImageKey(string key)
        {
            return key + "_Straight";
        }

        public void SetAlphaState(string key, AlphaState value)
        {
            if (String.IsNullOrEmpty(key))
                key = BufferKey;
            _alphaStates[key] = value;
        }

        public int Count
        {
            get { return _images.Count; }
        }

        public Bitmap this[string key]
        {
            get
            {
                return ProcessResult(key, _images[ProcessKey(key)]);
            }
            set
            {
                if (!string.IsNullOrEmpty(key))
                    _images[key] = value;
                _images[BufferKey] = value;
            }
        }

        public ImageBuffer(RectangleF bounds, float inflate, ISvgRenderer renderer, Action<ISvgRenderer> renderMethod)
        {
            _bounds = bounds;
            _inflate = inflate;
            _renderer = renderer;
            _renderMethod = renderMethod;
            _images = new Dictionary<string, Bitmap>();
            _alphaStates = new Dictionary<string, AlphaState>();
            _images[SvgFilterPrimitive.BackgroundAlpha] = null;
            _alphaStates[SvgFilterPrimitive.BackgroundAlpha] = AlphaState.AlphaOnly;
            _images[SvgFilterPrimitive.BackgroundImage] = null;
            _alphaStates[SvgFilterPrimitive.BackgroundImage] = AlphaState.Straight;
            _images[SvgFilterPrimitive.FillPaint] = null;
            _alphaStates[SvgFilterPrimitive.FillPaint] = AlphaState.Straight;
            _images[SvgFilterPrimitive.SourceAlpha] = null;
            _alphaStates[SvgFilterPrimitive.SourceAlpha] = AlphaState.AlphaOnly;
            _images[SvgFilterPrimitive.SourceGraphic] = null;
            _alphaStates[SvgFilterPrimitive.SourceGraphic] = AlphaState.Straight;
            _images[SvgFilterPrimitive.StrokePaint] = null;
            _alphaStates[SvgFilterPrimitive.StrokePaint] = AlphaState.Straight;
        }

        public void Add(string key, Bitmap value)
        {
            _images.Add(ProcessKey(key), value);
        }
        public bool ContainsKey(string key)
        {
            return _images.ContainsKey(ProcessKey(key));
        }
        public void Clear()
        {
            _images.Clear();
            _alphaStates.Clear();
        }
        public IEnumerator<KeyValuePair<string, Bitmap>> GetEnumerator()
        {
            return _images.GetEnumerator();
        }
        public bool Remove(string key)
        {
            switch (key)
            {
                case SvgFilterPrimitive.BackgroundAlpha:
                case SvgFilterPrimitive.BackgroundImage:
                case SvgFilterPrimitive.FillPaint:
                case SvgFilterPrimitive.SourceAlpha:
                case SvgFilterPrimitive.SourceGraphic:
                case SvgFilterPrimitive.StrokePaint:
                    return false;
                default:
                    return _images.Remove(ProcessKey(key));
            }
        }
        public bool TryGetValue(string key, out Bitmap value)
        {
            if (_images.TryGetValue(ProcessKey(key), out value))
            {
                value = ProcessResult(key, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        private Bitmap ProcessResult(string key, Bitmap curr)
        {
            if (curr == null)
            {
                if (string.IsNullOrEmpty(key))
                    key = SvgFilterPrimitive.SourceGraphic;

                switch (key)
                {
                    case SvgFilterPrimitive.BackgroundAlpha:
                    case SvgFilterPrimitive.BackgroundImage:
                    case SvgFilterPrimitive.FillPaint:
                    case SvgFilterPrimitive.StrokePaint:
                        // Do nothing
                        return null;
                    case SvgFilterPrimitive.SourceAlpha:
                        if (_images[key] == null)
                            _images[key] = CreateSourceAlpha();
                        return _images[key];
                    case SvgFilterPrimitive.SourceGraphic:
                        if (_images[key] == null)
                            _images[key] = CreateSourceGraphic();
                        return _images[key];
                }
            }
            return curr;
        }
        private string ProcessKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return _images.ContainsKey(BufferKey) ? BufferKey : SvgFilterPrimitive.SourceGraphic;
            return key;
        }

        private Bitmap CreateSourceGraphic()
        {
            var graphic = new Bitmap((int)(_bounds.Width + 2 * _inflate * _bounds.Width + _bounds.X),
                                     (int)(_bounds.Height + 2 * _inflate * _bounds.Height + _bounds.Y));
            using (var renderer = SvgRenderer.FromImage(graphic))
            {
                renderer.SetBoundable(_renderer.GetBoundable());
                var transform = new Matrix();
                transform.Translate(_bounds.Width * _inflate, _bounds.Height * _inflate);
                renderer.Transform = transform;
                //renderer.Transform = _renderer.Transform;
                //renderer.Clip = _renderer.Clip;
                _renderMethod.Invoke(renderer);
            }
            return graphic;
        }

        private Bitmap CreateSourceAlpha()
        {
            Bitmap source = this[SvgFilterPrimitive.SourceGraphic];

            float[][] colorMatrixElements = {
                   new float[] {0, 0, 0, 0, 0},        // red
                   new float[] {0, 0, 0, 0, 0},        // green
                   new float[] {0, 0, 0, 0, 0},        // blue
                   new float[] {0, 0, 0, 1, 1},        // alpha
                   new float[] {0, 0, 0, 0, 0} };    // translations

            var matrix = new ColorMatrix(colorMatrixElements);

            var sourceAlpha = new Bitmap(source.Width, source.Height);

            using (var graphics = Graphics.FromImage(sourceAlpha))
            using (var attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(matrix);
                graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0,
                      source.Width, source.Height, GraphicsUnit.Pixel, attributes);
                graphics.Save();
            }

            return sourceAlpha;
        }

        private Bitmap ToPremultipliedRGBA(string key)
        {
            var inputImage = this[key];
            var bitmapSrc = inputImage as Bitmap;
            if (bitmapSrc == null)
                return null;

            using (var src = new RawBitmap(bitmapSrc))
            {
                using (var dest = new RawBitmap(new Bitmap(inputImage.Width, inputImage.Height)))
                {
                    int pixelCount = src.Width * src.Height;
                    int index = 0;
                    for (int i = 0; i < pixelCount; i++)
                    {
                        var alpha = src.ArgbValues[index + 3] / 255.0;
                        dest.ArgbValues[index] = (byte)(src.ArgbValues[index] * alpha + 0.5);
                        ++index;
                        dest.ArgbValues[index] = (byte)(src.ArgbValues[index] * alpha + 0.5);
                        ++index;
                        dest.ArgbValues[index] = (byte)(src.ArgbValues[index] * alpha + 0.5);
                        ++index;
                        dest.ArgbValues[index] = src.ArgbValues[index];
                        ++index;
                    }
                    return dest.Bitmap;
                }
            }
        }

        private Bitmap ToStraightRGBA(string key)
        {
            var inputImage = this[key];
            var bitmapSrc = inputImage as Bitmap;
            if (bitmapSrc == null)
                return null;

            using (var src = new RawBitmap(bitmapSrc))
            {
                using (var dest = new RawBitmap(new Bitmap(inputImage.Width, inputImage.Height)))
                {
                    int pixelCount = src.Width * src.Height;
                    int index = 0;
                    for (int i = 0; i < pixelCount; i++)
                    {
                        var alpha = src.ArgbValues[index + 3] / 255.0;
                        if (alpha == 0)
                        {
                            dest.ArgbValues[index] = 0;
                            dest.ArgbValues[++index] = 0;
                            dest.ArgbValues[++index] = 0;
                            dest.ArgbValues[++index] = 0;
                            ++index;
                        }
                        else
                        {
                            dest.ArgbValues[index] = (byte)(src.ArgbValues[index] / alpha + 0.5);
                            ++index;
                            dest.ArgbValues[index] = (byte)(src.ArgbValues[index] / alpha + 0.5);
                            ++index;
                            dest.ArgbValues[index] = (byte)(src.ArgbValues[index] / alpha + 0.5);
                            ++index;
                            dest.ArgbValues[index] = src.ArgbValues[index];
                            ++index;
                        }
                    }
                    return dest.Bitmap;
                }
            }
        }

        /// <summary>
        /// If the given image is not premultiplied (e.g. straight), a premultiplied
        /// image with a separate key is returned, otherwise the original image.
        /// Most filter primitives work with premultiplied images.
        /// </summary>
        public Bitmap PremultipliedImage(string key = BufferKey)
        {
            if (GetAlphaState(key) == AlphaState.Straight)
            {
                var premultipliedKey = PremultipliedImageKey(key);
                if (ContainsKey(premultipliedKey))
                {
                    return _images[premultipliedKey];
                }

                var image = ToPremultipliedRGBA(key);
                Add(premultipliedKey, image);
                SetAlphaState(premultipliedKey, AlphaState.Premultiplied);
                return image;
            }
            return this[key];
        }

        /// <summary>
        /// If the given image is premultiplied, a straight
        /// image with a separate key is returned, otherwise the original image.
        /// Usually the image output from the last filter primitive has to be 
        /// converted back to straight format for display.
        /// </summary>
        public Bitmap StraightImage(string key = BufferKey)
        {
            if (GetAlphaState(key) == AlphaState.Premultiplied)
            {
                var straightKey = ImageBuffer.StraightImageKey(key);
                if (ContainsKey(straightKey))
                {
                    return _images[straightKey];
                }

                var image = ToStraightRGBA(key);
                Add(straightKey, image);
                SetAlphaState(straightKey, AlphaState.Straight);
                return image;
            }
            return this[key];
        }


        bool ICollection<KeyValuePair<string, Bitmap>>.IsReadOnly
        {
            get { return false; }
        }
        ICollection<string> IDictionary<string, Bitmap>.Keys
        {
            get { return _images.Keys; }
        }
        ICollection<Bitmap> IDictionary<string, Bitmap>.Values
        {
            get { return _images.Values; }
        }

        void ICollection<KeyValuePair<string, Bitmap>>.Add(KeyValuePair<string, Bitmap> item)
        {
            _images.Add(item.Key, item.Value);
        }
        bool ICollection<KeyValuePair<string, Bitmap>>.Contains(KeyValuePair<string, Bitmap> item)
        {
            return ((IDictionary<string, Bitmap>)_images).Contains(item);
        }
        void ICollection<KeyValuePair<string, Bitmap>>.CopyTo(KeyValuePair<string, Bitmap>[] array, int arrayIndex)
        {
            ((IDictionary<string, Bitmap>)_images).CopyTo(array, arrayIndex);
        }
        bool ICollection<KeyValuePair<string, Bitmap>>.Remove(KeyValuePair<string, Bitmap> item)
        {
            return _images.Remove(item.Key);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _images.GetEnumerator();
        }
    }
}
