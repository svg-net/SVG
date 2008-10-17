using System;
using System.Collections.Generic;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Represents a collection of <see cref="SvgElement"/>s.
    /// </summary>
    public sealed class SvgElementCollection : IList<SvgElement>
    {
        private List<SvgElement> _elements;
        private SvgElement _owner;
        private bool _mock;

        /// <summary>
        /// Initialises a new instance of an <see cref="SvgElementCollection"/> class.
        /// </summary>
        /// <param name="owner">The owner <see cref="SvgElement"/> of the collection.</param>
        internal SvgElementCollection(SvgElement owner)
            : this(owner, false)
        {

        }

        internal SvgElementCollection(SvgElement owner, bool mock)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this._elements = new List<SvgElement>();
            this._owner = owner;
            this._mock = mock;
        }

        /// <summary>
        /// Returns the index of the specified <see cref="SvgElement"/> in the collection.
        /// </summary>
        /// <param name="item">The <see cref="SvgElement"/> to search for.</param>
        /// <returns>The index of the element if it is present; otherwise -1.</returns>
        public int IndexOf(SvgElement item)
        {
            return this._elements.IndexOf(item);
        }

        /// <summary>
        /// Inserts the given <see cref="SvgElement"/> to the collection at the specified index.
        /// </summary>
        /// <param name="index">The index that the <paramref name="item"/> should be added at.</param>
        /// <param name="item">The <see cref="SvgElement"/> to be added.</param>
        public void Insert(int index, SvgElement item)
        {
            this._elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            SvgElement element = this[index];

            if (element != null)
            {
                this.Remove(element);
            }
        }

        public SvgElement this[int index]
        {
            get { return this._elements[index]; }
            set { this._elements[index] = value; }
        }

        public void Add(SvgElement item)
        {
            if (!this._mock)
            {
                if (this._owner.OwnerDocument != null)
                {
                    this._owner.OwnerDocument.IdManager.Add(item);
                }

                item._parent = this._owner;
            }

            item._parent.OnElementAdded(item, this.Count - 1);

            this._elements.Add(item);
        }

        public void Clear()
        {
            while (this.Count > 0)
            {
                SvgElement element = this[0];
                this.Remove(element);
            }
        }

        public bool Contains(SvgElement item)
        {
            return this._elements.Contains(item);
        }

        public void CopyTo(SvgElement[] array, int arrayIndex)
        {
            this._elements.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._elements.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(SvgElement item)
        {
            bool removed = this._elements.Remove(item);

            if (removed)
            {
                this._owner.OnElementRemoved(item);

                if (!this._mock)
                {
                    item._parent = null;

                    if (this._owner.OwnerDocument != null)
                    {
                        this._owner.OwnerDocument.IdManager.Remove(item);
                    }
                }
            }

            return removed;
        }

        public IEnumerator<SvgElement> GetEnumerator()
        {
            return this._elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._elements.GetEnumerator();
        }
    }
}