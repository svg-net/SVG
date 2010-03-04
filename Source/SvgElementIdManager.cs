using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Provides methods to ensure element ID's are valid and unique.
    /// </summary>
    public class SvgElementIdManager
    {
        private SvgDocument _document;
        private Dictionary<string, SvgElement> _idValueMap;

        /// <summary>
        /// Retrieves the <see cref="SvgElement"/> with the specified ID.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID of the element to find.</param>
        /// <returns>An <see cref="SvgElement"/> of one exists with the specified ID; otherwise false.</returns>
        public virtual SvgElement GetElementById(string id)
        {
            if (id.StartsWith("url("))
            {
                id = id.Substring(4);
                id = id.TrimEnd(')');
            }
            if (id.StartsWith("#"))
            {
                id = id.Substring(1);
            }

            SvgElement element = null;
            this._idValueMap.TryGetValue(id, out element);

            return element;
        }

        public virtual SvgElement GetElementById(Uri uri)
        {
            return this.GetElementById(uri.ToString());
        }

        /// <summary>
        /// Adds the specified <see cref="SvgElement"/> for ID management.
        /// </summary>
        /// <param name="element">The <see cref="SvgElement"/> to be managed.</param>
        public virtual void Add(SvgElement element)
        {
            if (!string.IsNullOrEmpty(element.ID))
            {
                this.EnsureValidId(element.ID);
                this._idValueMap.Add(element.ID, element);
            }
        }

        /// <summary>
        /// Removed the specified <see cref="SvgElement"/> from ID management.
        /// </summary>
        /// <param name="element">The <see cref="SvgElement"/> to be removed from ID management.</param>
        public virtual void Remove(SvgElement element)
        {
            if (!string.IsNullOrEmpty(element.ID))
            {
                this._idValueMap.Remove(element.ID);
            }
        }

        /// <summary>
        /// Ensures that the specified ID is valid within the containing <see cref="SvgDocument"/>.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID to validate.</param>
        /// <exception cref="SvgException">
        /// <para>The ID cannot start with a digit.</para>
        /// <para>An element with the same ID already exists within the containing <see cref="SvgDocument"/>.</para>
        /// </exception>
        public void EnsureValidId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            if (char.IsDigit(id[0]))
            {
                throw new SvgException("ID cannot start with a digit: '" + id + "'.");
            }

            if (this._idValueMap.ContainsKey(id))
            {
                throw new SvgException("An element with the same ID already exists: '" + id + "'.");
            }
        }

        /// <summary>
        /// Initialises a new instance of an <see cref="SvgElementIdManager"/>.
        /// </summary>
        /// <param name="document">The <see cref="SvgDocument"/> containing the <see cref="SvgElement"/>s to manage.</param>
        public SvgElementIdManager(SvgDocument document)
        {
            this._document = document;
            this._idValueMap = new Dictionary<string, SvgElement>();
        }
    }
}