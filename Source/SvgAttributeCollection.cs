using System;
using System.Collections.Generic;

namespace Svg
{
    /// <summary>
    /// A collection of Scalable Vector Attributes that can be inherited from the owner elements ancestors.
    /// </summary>
    public sealed class SvgAttributeCollection : Dictionary<string, object>
    {
        private readonly SvgElement _owner;

        /// <summary>
        /// Initialises a new instance of a <see cref="SvgAttributeCollection"/> with the given <see cref="SvgElement"/> as the owner.
        /// </summary>
        /// <param name="owner">The <see cref="SvgElement"/> owner of the collection.</param>
        public SvgAttributeCollection(SvgElement owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <param name="defaultValue">The value to return if a value hasn't already been specified.</param>
        /// <returns>The attribute value if available; otherwise the default value of <typeparamref name="TAttributeType"/>.</returns>
        public TAttributeType GetAttribute<TAttributeType>(string attributeName, TAttributeType defaultValue = default(TAttributeType))
        {
            if (ContainsKey(attributeName) && base[attributeName] != null)
                return (TAttributeType)base[attributeName];

            return defaultValue;
        }

        /// <summary>
        /// Gets the attribute with the specified name and inherits from ancestors if there is no attribute set.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute value.</typeparam>
        /// <param name="attributeName">A <see cref="string"/> containing the name of the attribute.</param>
        /// <param name="inherited">Used only if the attribute value is not available. If set to true, the inherited value is returned in this case, otherwise the default value.</param>
        /// <param name="defaultValue">The value to return if a value hasn't already been specified.</param>
        /// <returns>The attribute value if available and not set to "inherit"; the ancestors value for the same attribute if it exists and if either the attribute value is set to "inherit", or <paramref name="inherited"/> is true; the default value otherwise.</returns>
        public TAttributeType GetInheritedAttribute<TAttributeType>(string attributeName, bool inherited, TAttributeType defaultValue = default(TAttributeType))
        {
            var inherit = false;

            if (ContainsKey(attributeName))
            {
                var result = (TAttributeType)base[attributeName];

                if (IsInheritValue(result))
                    inherit = true;
                else
                {
                    var deferred = result as SvgDeferredPaintServer;
                    if (deferred == null)
                        return result;
                    else
                    {
                        var server = SvgDeferredPaintServer.TryGet<SvgPaintServer>(deferred, _owner);
                        if (server == SvgPaintServer.Inherit)
                            inherit = true;
                        else
                            return result;
                    }
                }
            }

            if (inherited || inherit)
            {
                var parentAttribute = _owner.Parent?.Attributes.GetInheritedAttribute<object>(attributeName, inherited);
                if (parentAttribute != null)
                    return (TAttributeType)parentAttribute;
            }

            return defaultValue;
        }

        private bool IsInheritValue(object value)
        {
            return string.Equals(value?.ToString().Trim(), "inherit", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="attributeName">A <see cref="string"/> containing the attribute name.</param>
        /// <returns>The attribute value associated with the specified name; If there is no attribute the parent's value will be inherited.</returns>
        public new object this[string attributeName]
        {
            get { return GetInheritedAttribute<object>(attributeName, true); }
            set
            {
                if (ContainsKey(attributeName))
                {
                    var oldVal = base[attributeName];
                    if (TryUnboxedCheck(oldVal, value))
                    {
                        base[attributeName] = value;
                        OnAttributeChanged(attributeName, value);
                    }
                }
                else
                {
                    base[attributeName] = value;
                    OnAttributeChanged(attributeName, value);
                }
            }
        }

        private bool TryUnboxedCheck(object a, object b)
        {
            if (IsValueType(a))
            {
                if (a is SvgUnit)
                    return UnboxAndCheck<SvgUnit>(a, b);
                else if (a is bool)
                    return UnboxAndCheck<bool>(a, b);
                else if (a is int)
                    return UnboxAndCheck<int>(a, b);
                else if (a is float)
                    return UnboxAndCheck<float>(a, b);
                else if (a is SvgViewBox)
                    return UnboxAndCheck<SvgViewBox>(a, b);
                else
                    return true;
            }
            else
                return a != b;
        }

        private bool UnboxAndCheck<T>(object a, object b)
        {
            return !((T)a).Equals((T)b);
        }

        private bool IsValueType(object obj)
        {
            return obj != null && obj.GetType().IsValueType;
        }

        /// <summary>
        /// Fired when an Atrribute has changed
        /// </summary>
        public event EventHandler<AttributeEventArgs> AttributeChanged;

        private void OnAttributeChanged(string attribute, object value)
        {
            var handler = AttributeChanged;
            if (handler != null)
                handler(_owner, new AttributeEventArgs { Attribute = attribute, Value = value });
        }
    }

    /// <summary>
    /// A collection of Custom Attributes
    /// </summary>
    public sealed class SvgCustomAttributeCollection : Dictionary<string, string>
    {
        private readonly SvgElement _owner;

        /// <summary>
        /// Initialises a new instance of a <see cref="SvgAttributeCollection"/> with the given <see cref="SvgElement"/> as the owner.
        /// </summary>
        /// <param name="owner">The <see cref="SvgElement"/> owner of the collection.</param>
        public SvgCustomAttributeCollection(SvgElement owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="attributeName">A <see cref="string"/> containing the attribute name.</param>
        /// <returns>The attribute value associated with the specified name; If there is no attribute the parent's value will be inherited.</returns>
        public new string this[string attributeName]
        {
            get { return base[attributeName]; }
            set
            {
                if (ContainsKey(attributeName))
                {
                    var oldVal = base[attributeName];
                    base[attributeName] = value;
                    if (oldVal != value) OnAttributeChanged(attributeName, value);
                }
                else
                {
                    base[attributeName] = value;
                    OnAttributeChanged(attributeName, value);
                }
            }
        }

        /// <summary>
        /// Fired when an Atrribute has changed
        /// </summary>
        public event EventHandler<AttributeEventArgs> AttributeChanged;

        private void OnAttributeChanged(string attribute, object value)
        {
            var handler = AttributeChanged;
            if (handler != null)
                handler(_owner, new AttributeEventArgs { Attribute = attribute, Value = value });
        }
    }
}
