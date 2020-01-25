using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Svg.FilterEffects
{
    public class ImageBuffer : Dictionary<string, Bitmap>, IDisposable
    {
        private const string BufferKey = "__!!BUFFER";

        private readonly RectangleF _bounds;
        private readonly ISvgRenderer _renderer;
        private readonly Action<ISvgRenderer> _renderMethod;
        private readonly float _inflate;

        private Matrix _transform;

        public Matrix Transform
        {
            get { return _transform?.Clone(); }
            set
            {
                _transform?.Dispose();
                _transform = value?.Clone();
            }
        }

        public Bitmap Buffer
        {
            get { return this[BufferKey]; }
        }

        public new Bitmap this[string key]
        {
            get { return ProcessResult(ProcessKey(key), base[ProcessKey(key)]); }
            set { base[string.IsNullOrEmpty(key) ? BufferKey : key] = value; }
        }

        public ImageBuffer(RectangleF bounds, float inflate, ISvgRenderer renderer, Action<ISvgRenderer> renderMethod)
        {
            _bounds = bounds;
            _inflate = inflate;
            _renderer = renderer;
            _renderMethod = renderMethod;

            this[SvgFilterPrimitive.SourceGraphic] = null;
            this[SvgFilterPrimitive.SourceAlpha] = null;
            this[SvgFilterPrimitive.BackgroundImage] = null;
            this[SvgFilterPrimitive.BackgroundAlpha] = null;
            this[SvgFilterPrimitive.FillPaint] = null;
            this[SvgFilterPrimitive.StrokePaint] = null;
        }

        public new void Add(string key, Bitmap value)
        {
            base.Add(ProcessKey(key), value);
        }

        public new bool ContainsKey(string key)
        {
            return base.ContainsKey(ProcessKey(key));
        }

        public new void Clear()
        {
            this.Clear(i => i?.Dispose());
        }

        public new bool Remove(string key)
        {
            switch (ProcessKey(key))
            {
                case SvgFilterPrimitive.SourceGraphic:
                case SvgFilterPrimitive.SourceAlpha:
                case SvgFilterPrimitive.BackgroundImage:
                case SvgFilterPrimitive.BackgroundAlpha:
                case SvgFilterPrimitive.FillPaint:
                case SvgFilterPrimitive.StrokePaint:
                    return false;
                default:
                    return this.Remove(ProcessKey(key), i => i?.Dispose());
            }
        }

        public new bool TryGetValue(string key, out Bitmap value)
        {
            if (base.TryGetValue(ProcessKey(key), out value))
            {
                value = ProcessResult(ProcessKey(key), value);
                return true;
            }
            return false;
        }

        private Bitmap ProcessResult(string key, Bitmap curr)
        {
            if (curr == null)
            {
                switch (key)
                {
                    case SvgFilterPrimitive.SourceGraphic:
                        this[key] = CreateSourceGraphic();
                        return this[key];
                    case SvgFilterPrimitive.SourceAlpha:
                        this[key] = CreateSourceAlpha();
                        return this[key];
                    case SvgFilterPrimitive.BackgroundImage:
                    case SvgFilterPrimitive.BackgroundAlpha:
                    case SvgFilterPrimitive.FillPaint:
                    case SvgFilterPrimitive.StrokePaint:
                        // Do nothing
                        return null;
                }
            }
            return curr;
        }

        private string ProcessKey(string key)
        {
            return string.IsNullOrEmpty(key) ? ContainsKey(BufferKey) ? BufferKey : SvgFilterPrimitive.SourceGraphic : key;
        }

        private Bitmap CreateSourceGraphic()
        {
            var graphic = new Bitmap((int)(_bounds.Width + 2 * _inflate * _bounds.Width + _bounds.X),
                                     (int)(_bounds.Height + 2 * _inflate * _bounds.Height + _bounds.Y));
            using (var renderer = SvgRenderer.FromImage(graphic))
            using (var transform = new Matrix())
            {
                renderer.SetBoundable(_renderer.GetBoundable());
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
            var source = this[SvgFilterPrimitive.SourceGraphic];

            float[][] colorMatrixElements = {
                new float[] {0, 0, 0, 0, 0},    // red
                new float[] {0, 0, 0, 0, 0},    // green
                new float[] {0, 0, 0, 0, 0},    // blue
                new float[] {0, 0, 0, 1, 1},    // alpha
                new float[] {0, 0, 0, 0, 0},    // translations
            };

            var matrix = new ColorMatrix(colorMatrixElements);

            var sourceAlpha = new Bitmap(source.Width, source.Height);

            using (var graphics = Graphics.FromImage(sourceAlpha))
            using (var attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(matrix);
                graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
                graphics.Save();
            }

            return sourceAlpha;
        }

        public void Dispose()
        {
            Clear();
            _transform?.Dispose();
        }
    }

    static class DictionaryExtensions
    {
        public static void Clear<TKey, TValue>(this Dictionary<TKey, TValue> self, Action<TValue> action)
        {
            foreach (var kvp in self)
                action(kvp.Value);
            self.Clear();
        }

        public static bool Remove<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Action<TValue> action)
        {
            if (!self.ContainsKey(key))
                return false;
            action(self[key]);
            return self.Remove(key);
        }
    }
}
