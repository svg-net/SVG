using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Svg
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ElementFactoryAttribute : Attribute
    {
    }

    internal enum DescriptorType
    {
        Property,
        Event
    }

    internal interface ISvgPropertyDescriptor
    {
        DescriptorType DescriptorType { get; }
        string AttributeName { get; }
        string AttributeNamespace { get; }
        TypeConverter Converter { get; }
        Type Type { get; }
        object GetValue(object component);
        void SetValue(object component, ITypeDescriptorContext context, CultureInfo culture, object value);
    }

    internal class SvgPropertyDescriptor<T, TU> : ISvgPropertyDescriptor
    {
        public DescriptorType DescriptorType { get; }
        public string AttributeName { get; }
        public string AttributeNamespace { get; }
        public TypeConverter Converter { get; }
        public Type Type { get; } = typeof(TU);
        private Func<T, TU> Getter { get; }
        private Action<T, TU> Setter { get; }

        public SvgPropertyDescriptor(DescriptorType descriptorType, string attributeName, 
            string attributeNamespace, TypeConverter converter, Func<T, TU> getter, Action<T, TU> setter)
        {
            DescriptorType = descriptorType;
            AttributeName = attributeName;
            AttributeNamespace = attributeNamespace;
            Converter = converter;
            Getter = getter;
            Setter = setter;
        }

        public object GetValue(object component)
        {
            return (object)Getter((T)component);
        }

        public void SetValue(object component, ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (Converter != null)
            {
                Setter((T)component, (TU)Converter.ConvertFrom(context, culture, value));
            }
        }
    }
}
