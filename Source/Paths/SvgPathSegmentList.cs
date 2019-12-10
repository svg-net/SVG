using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Svg.Pathing
{
    [TypeConverter(typeof(SvgPathBuilder))]
    public sealed class SvgPathSegmentList : IList<SvgPathSegment>, ICloneable
    {
        private readonly List<SvgPathSegment> _segments = new List<SvgPathSegment>();

        public ISvgPathElement Owner { get; set; }

        public SvgPathSegment First
        {
            get { return _segments[0]; }
        }

        public SvgPathSegment Last
        {
            get { return _segments[_segments.Count - 1]; }
        }

        public int IndexOf(SvgPathSegment item)
        {
            return _segments.IndexOf(item);
        }

        public void Insert(int index, SvgPathSegment item)
        {
            _segments.Insert(index, item);
            if (Owner != null)
                Owner.OnPathUpdated();
        }

        public void RemoveAt(int index)
        {
            _segments.RemoveAt(index);
            if (Owner != null)
                Owner.OnPathUpdated();
        }

        public SvgPathSegment this[int index]
        {
            get { return _segments[index]; }
            set
            {
                _segments[index] = value;
                if (Owner != null)
                    Owner.OnPathUpdated();
            }
        }

        public void Add(SvgPathSegment item)
        {
            _segments.Add(item);
            if (Owner != null)
                Owner.OnPathUpdated();
        }

        public void Clear()
        {
            _segments.Clear();
        }

        public bool Contains(SvgPathSegment item)
        {
            return _segments.Contains(item);
        }

        public void CopyTo(SvgPathSegment[] array, int arrayIndex)
        {
            _segments.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _segments.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(SvgPathSegment item)
        {
            var removed = _segments.Remove(item);

            if (removed && Owner != null)
                Owner.OnPathUpdated();

            return removed;
        }

        public IEnumerator<SvgPathSegment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        public object Clone()
        {
            var segments = new SvgPathSegmentList();
            foreach (var segment in this)
                segments.Add(segment.Clone());
            return segments;
        }

        public override string ToString()
        {
            return string.Join(" ", this.Select(p => p.ToString()).ToArray());
        }
    }

    public interface ISvgPathElement
    {
        void OnPathUpdated();
    }
}
