using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg.Transforms
{
    [TypeConverter(typeof(SvgTransformConverter))]
    public class SvgTransformCollection : List<SvgTransform>, ICloneable
    {
        private void AddItem(SvgTransform item)
        {
            base.Add(item);
        }

        public new void Add(SvgTransform item)
        {
            AddItem(item);
            OnTransformChanged();
        }

        public new void AddRange(IEnumerable<SvgTransform> collection)
        {
            base.AddRange(collection);
            OnTransformChanged();
        }

        public new void Remove(SvgTransform item)
        {
            base.Remove(item);
            OnTransformChanged();
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnTransformChanged();
        }

        /// <summary>
        /// Multiplies all matrices
        /// </summary>
        /// <returns>The result of all transforms</returns>
        public Matrix GetMatrix()
        {
            var transformMatrix = new Matrix();

            foreach (var transform in this)
                using (var matrix = transform.Matrix)
                    transformMatrix.Multiply(matrix);

            return transformMatrix;
        }

        public override bool Equals(object obj)
        {
            if (Count == 0 && Count == base.Count) // default will be an empty list 
                return true;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public new SvgTransform this[int i]
        {
            get { return base[i]; }
            set
            {
                var oldVal = base[i];
                base[i] = value;
                if (oldVal != value)
                    OnTransformChanged();
            }
        }

        /// <summary>
        /// Fired when an SvgTransform has changed
        /// </summary>
        public event EventHandler<AttributeEventArgs> TransformChanged;

        protected void OnTransformChanged()
        {
            var handler = TransformChanged;
            if (handler != null)
                // make a copy of the current value to avoid collection changed exceptions
                handler(this, new AttributeEventArgs { Attribute = "transform", Value = Clone() });
        }

        public object Clone()
        {
            var result = new SvgTransformCollection();
            foreach (var transform in this)
                 result.AddItem(transform.Clone() as SvgTransform);
             return result;
        }

        public override string ToString()
        {
            if (Count < 1)
                return string.Empty;
            return (from t in this select t.ToString()).Aggregate((p, c) => p + " " + c);
        }
    }
}
