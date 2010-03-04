using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// A collection of Scalable Vector Attributes that can be inherited from the owner elements ancestors.
    /// </summary>
    public sealed class SvgAttributeCollection : Dictionary<string, object>
    {
        private SvgElement _owner;

        /// <summary>
        /// Initialises a new instance of a <see cref="SvgAttributeCollection"/> with the given <see cref="SvgElement"/> as the owner.
        /// </summary>
        /// <param name="owner">The <see cref="SvgElement"/> owner of the collection.</param>
        public SvgAttributeCollection(SvgElement owner)
        {
            this._owner = owner;
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <returns>The attribute value if available; otherwise the default value of <typeparamref name="TAttributeType"/>.</returns>
        public TAttributeType GetAttribute<TAttributeType>(string attributeName)
        {
            if (this.ContainsKey(attributeName) && base[attributeName] != null)
            {
                return (TAttributeType)base[attributeName];
            }

            return this.GetAttribute<TAttributeType>(attributeName, default(TAttributeType));
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <param name="defaultValue">The value to return if a value hasn't already been specified.</param>
        /// <returns>The attribute value if available; otherwise the default value of <typeparamref name="T"/>.</returns>
        public T GetAttribute<T>(string attributeName, T defaultValue)
        {
            if (this.ContainsKey(attributeName) && base[attributeName] != null)
            {
                return (T)base[attributeName];
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the attribute with the specified name and inherits from ancestors if there is no attribute set.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <returns>The attribute value if available; otherwise the ancestors value for the same attribute; otherwise the default value of <typeparamref name="TAttributeType"/>.</returns>
        public TAttributeType GetInheritedAttribute<TAttributeType>(string attributeName)
        {
            if (this.ContainsKey(attributeName) && base[attributeName] != null)
            {
                return (TAttributeType)base[attributeName];
            }

            if (this._owner.Parent != null)
            {
                if (this._owner.Parent.Attributes[attributeName] != null)
                {
                    return (TAttributeType)this._owner.Parent.Attributes[attributeName];
                }
            }

            return default(TAttributeType);
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="attributeName">A <see cref="string"/> containing the attribute name.</param>
        /// <returns>The attribute value associated with the specified name; If there is no attribute the parent's value will be inherited.</returns>
        public new object this[string attributeName]
        {
            get { return this.GetInheritedAttribute<object>(attributeName); }
            set { base[attributeName] = value; }
        }
    }
}