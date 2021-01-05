﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Svg.Generators
{
    /// <summary>
    /// Generates available elements ElementInfo metadata for SvgElementFactory.
    /// </summary>
    [Generator]
    public class AvailableElementsGenerator : ISourceGenerator
    {
        #region Model

        /// <summary>
        /// The object model used to generate SvgElements descriptors.
        /// </summary>
        private const string ModelText = @"// <auto-generated />
#nullable disable
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

        public SvgPropertyDescriptor(DescriptorType descriptorType, string attributeName, string attributeNamespace, TypeConverter converter, Func<T, TU> getter, Action<T, TU> setter)
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
            Setter((T)component, (TU)Converter.ConvertFrom(context, culture, value));
        }
    }

    internal class SvgElementDescriptor
    {
        public Type TargetType { get; set; }
        public Dictionary<string, ISvgPropertyDescriptor> Properties { get; set; }
    }
}";
        #endregion

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
            // NOTE: Uncomment the next line to enable source generator debugging (build project to trigger debugger to be attached).
            // System.Diagnostics.Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            // Add the ElementFactory model source to compilation. 
            context.AddSource("Svg_Model", SourceText.From(ModelText, Encoding.UTF8));

            // Check is we have our SyntaxReceiver object used to filter compiled code.
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            {
                return;
            }

            var options = (context.Compilation as CSharpCompilation)?.SyntaxTrees[0].Options as CSharpParseOptions;
            var compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(ModelText, Encoding.UTF8), options));

            var elementFactoryAttribute = compilation.GetTypeByMetadataName("Svg.ElementFactoryAttribute");
            if (elementFactoryAttribute is null)
            {
                return;
            }

            var svgElementBaseSymbol = compilation.GetTypeByMetadataName("Svg.SvgElement");
            if (svgElementBaseSymbol is null)
            {
                return;
            }

            List<INamedTypeSymbol> elementFactorySymbols = new();
            List<INamedTypeSymbol> svgElementSymbols = new();

            foreach (var candidateClass in receiver.CandidateClasses)
            {
                var semanticModel = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var namedTypeSymbol = semanticModel.GetDeclaredSymbol(candidateClass);
                if (namedTypeSymbol is null)
                {
                    continue;
                }

                // Find classes with ElementFactory attribute.
                if (namedTypeSymbol.GetAttributes().Any(ad => ad?.AttributeClass?.Equals(elementFactoryAttribute, SymbolEqualityComparer.Default) ?? false))
                {
                    elementFactorySymbols.Add(namedTypeSymbol);
                    continue;
                }

                // Find classes derived from SvgElement.
                if (!namedTypeSymbol.IsAbstract && !namedTypeSymbol.IsGenericType && HasBaseType(namedTypeSymbol, svgElementBaseSymbol))
                {
                    svgElementSymbols.Add(namedTypeSymbol);
                }
            }

            // Generate code for each class marked with ElementFactor attribute.
            foreach (var elementFactorySymbol in elementFactorySymbols)
            {
                ProcessClass(context, compilation, elementFactorySymbol, svgElementSymbols, svgElementBaseSymbol);
            }
        }

        /// <summary>
        /// Generates source for for ElementFactory class.
        /// </summary>
        /// <param name="context">The context object.</param>
        /// <param name="compilation">The compilation object.</param>
        /// <param name="elementFactorySymbol">The ElementFactory type object.</param>
        /// <param name="svgElementSymbols">The SvgElement type symbols.</param>
        /// <param name="svgElementBaseSymbol">The base class for SvgElement type symbols.</param>
        private static void ProcessClass(GeneratorExecutionContext context, Compilation compilation, INamedTypeSymbol elementFactorySymbol, List<INamedTypeSymbol> svgElementSymbols, INamedTypeSymbol svgElementBaseSymbol)
        {
            // Get the containing namespace for ElementFactory class.
            if (!elementFactorySymbol.ContainingSymbol.Equals(elementFactorySymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                return;
            }

            // Get SvgElementAttribute symbol using for later attribute retrieval.
            var svgElementAttribute = compilation.GetTypeByMetadataName("Svg.SvgElementAttribute");
            if (svgElementAttribute is null)
            {
                return;
            }

            // Get SvgAttributeAttribute symbol using for later attribute retrieval.
            var svgAttributeAttribute = compilation.GetTypeByMetadataName("Svg.SvgAttributeAttribute");
            if (svgAttributeAttribute is null)
            {
                return;
            }

            // Convert symbol to proper display string.
            string namespaceElementFactory = elementFactorySymbol.ContainingNamespace.ToDisplayString();

            // Format symbols to support generic types and namespaces.
            var format = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance
            );

            // Format symbols to support generic types without namespaces.
            var formatClass = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance
            );

            string classElementFactory = elementFactorySymbol.ToDisplayString(formatClass);

            // Key: ElementName
            SortedDictionary<string, Element> items = new();

            // Include all element even without SvgElementAttribute set (e.g. SvgDocument).
            List<Element> elements = new();

            // Get all classes with SvgElementAttribute attribute set.
            foreach (var svgElementSymbol in svgElementSymbols)
            {
                string classNameSvgElement = svgElementSymbol.ToDisplayString(format);

                var elementName = default(string);

                var attributes = svgElementSymbol.GetAttributes();
                if (attributes.Length > 0)
                {
                    // Find SvgElementAttribute attribute data. The SvgElementAttribute has constructor with one argument of type string.
                    var attributeData = attributes.FirstOrDefault(ad => ad?.AttributeClass?.Equals(svgElementAttribute, SymbolEqualityComparer.Default) ?? false);
                    if (attributeData is not null && attributeData.ConstructorArguments.Length == 1)
                    {
                        // The ElementName is set in attribute by providing constructor argument.
                        elementName = (string?)attributeData.ConstructorArguments[0].Value;
                    }
                }

                if (elementName is not null && items.TryGetValue(elementName, out var element))
                {
                    element.ClassNames.Add(classNameSvgElement);
                }
                else
                {
                    element = new Element(
                        svgElementSymbol,
                        GetBaseTypes(svgElementSymbol, svgElementBaseSymbol).ToList(),
                        elementName,
                        new List<string> { classNameSvgElement },
                        GetElementProperties(compilation, svgElementSymbol, svgElementBaseSymbol, svgAttributeAttribute).ToList()
                    );
                    if (elementName is not null)
                    {
                        items.Add(elementName, element);
                    }
                    elements.Add(element);
                }
            }

            static int ElementNameComparison(Element x, Element y)
            {
                if (x.ElementName is null && y.ElementName is null)
                {
                    return string.Compare(x.Symbol.ToDisplayString(), y.Symbol.ToDisplayString(), StringComparison.Ordinal);
                }
                if (x.ElementName is null)
                {
                    if (y.ElementName is null)
                    {
                        return 0;
                    }
                    return -1;
                }
                if (y.ElementName is null)
                {
                    return 1;
                }
                return string.Compare(x.ElementName, y.ElementName, StringComparison.Ordinal);
            }

            elements.Sort(ElementNameComparison);

            var source = new StringBuilder();

            // Generate SvgElement base class with Properties dictionary.

            source.Append($@"// <auto-generated />
using System;
using System.Collections.Generic;

namespace Svg
{{
    public abstract partial class SvgElement
    {{
        internal abstract string AttributeName {{ get; }}
        internal abstract List<Type> ClassNames {{ get; }}
        internal abstract Dictionary<string, ISvgPropertyDescriptor> Properties {{ get; }}
    }}
}}
");
            context.AddSource($"Svg_SvgElement_Properties.cs", SourceText.From(source.ToString(), Encoding.UTF8));
            source.Clear();

            // Generate SvgElement derived classes with descriptor Properties dictionary.

            foreach (var element in elements)
            {
                string namespaceElement = element.Symbol.ContainingNamespace.ToDisplayString();
                string classElement = element.Symbol.ToDisplayString(formatClass);

                source.Append($@"// <auto-generated />
using System;
using System.Collections.Generic;

namespace {namespaceElement}
{{
    public partial class {classElement}
    {{
        internal override string AttributeName => ""{element.ElementName}"";

        internal override List<Type> ClassNames => {classElement}ClassNames;

        internal override Dictionary<string, ISvgPropertyDescriptor> Properties => {classElement}Properties;
");

                var classNames = element.ClassNames;
                source.Append($@"
        internal static List<Type> {classElement}ClassNames = new List<Type>() {{ ");
                for (var i = 0; i < classNames.Count; i++)
                {
                    var className = classNames[i];
                    if (!string.IsNullOrWhiteSpace(className))
                    {
                        source.Append($"typeof({className}){((i < classNames.Count && classNames.Count > 1) ? ", " : "")}");
                    }
                }
                source.AppendLine($" }};");

                source.Append($@"
        internal static Dictionary<string, ISvgPropertyDescriptor> {classElement}Properties = new Dictionary<string, ISvgPropertyDescriptor>()
        {{
");
                foreach (var property in element.Properties)
                {
                    var symbolType = GetTypeSymbol(property.Symbol);
                    if (symbolType is null)
                    {
                        continue;
                    }

                    var descriptorType = property.MemberType.ToString();
                    var containingType = property.Symbol.ContainingType.ToDisplayString(format);
                    var propertyType = symbolType.ToDisplayString(format);
                    var propertyName = property.Symbol.Name;
                    if (property.MemberType == MemberType.Property)
                    {
                        source.AppendLine($"            [\"{property.AttributeName}\"] = new SvgPropertyDescriptor<{containingType}, {propertyType}>(DescriptorType.{descriptorType}, \"{property.AttributeName}\", \"{property.AttributeNamespace}\", {(property.Converter == null ? "null" : $"new {property.Converter}()")}, (t) => t.{propertyName}, (t, v) => t.{propertyName} = v),");
                    }

                    if (property.MemberType == MemberType.Event)
                    {
                        // TODO: Generate descriptor for event.
                        // source.AppendLine($"            [\"{property.AttributeName}\"] = new SvgPropertyDescriptor<{containingType}, {propertyType}>(DescriptorType.{descriptorType}, \"{property.AttributeName}\", \"{property.AttributeNamespace}\", {(property.Converter == null ? "null" : $"new {property.Converter}()")}, (t) => t.{propertyName}, (t, v) => t.{propertyName} += v),");
                    }
                }
                source.Append(@$"        }};
    }}
}}
");
                context.AddSource($"{namespaceElement.Replace('.', '_')}_{element.Symbol.Name}_Properties.cs", SourceText.From(source.ToString(), Encoding.UTF8));
                source.Clear();
            }

            // Generate SvgElements class with ElementNames.

            source.Append(@"// <auto-generated />
using System;
using System.Collections.Generic;

namespace Svg
{
    internal static class SvgElements
    {
        public static Dictionary<Type, string> ElementNames { get; } = new Dictionary<Type, string>()
        {");
            foreach (var element in items)
            {
                var elementName = element.Key;
                var className = element.Value.ClassNames.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(className))
                {
                    continue;
                }
                source.Append(@$"
            [typeof({className})] = ""{elementName}"",");
            }
            source.Append(@"
        };
    }
}
");
            context.AddSource($"Svg_SvgElements.cs", SourceText.From(source.ToString(), Encoding.UTF8));
            source.Clear();

            // Generate ElementFactory class.

            source.Append($@"// <auto-generated />
using System;
using System.Collections.Generic;

namespace {namespaceElementFactory}
{{
    internal partial class {classElementFactory}
    {{");

            // Generate availableElements list.

            source.Append($@"
        private static readonly List<ElementInfo> availableElements = new List<ElementInfo>()
        {{
");
            foreach (var element in items)
            {
                var elementName = element.Key;
                var className = element.Value.ClassNames.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(className))
                {
                    continue;
                }

                source.AppendLine($@"            new ElementInfo() {{ ElementName = ""{elementName}"", ElementType = typeof({className}), CreateInstance = () => new {className}() }},");
            }
            source.Append($@"        }};");

            // Generate availableElementsWithoutSvg dictionary.

            source.Append($@"

        private static readonly Dictionary<string, ElementInfo> availableElementsWithoutSvg = new Dictionary<string, ElementInfo>()
        {{
");
            foreach (var element in items)
            {
                var elementName = element.Key;
                var className = element.Value.ClassNames.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(className))
                {
                    continue;
                }
                if (elementName.Equals("svg", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                source.AppendLine($@"            [""{elementName}""] = new ElementInfo() {{ ElementName = ""{elementName}"", ElementType = typeof({className}), CreateInstance = () => new {className}() }},");
            }
            source.Append($@"        }};");

            // Generate availableElementsDictionary dictionary.

            source.Append($@"

        private static readonly Dictionary<string, List<Type>> availableElementsDictionary = new Dictionary<string, List<Type>>()
        {{
");
            foreach (var element in items)
            {
                var elementName = element.Key;
                var classNames = element.Value.ClassNames;
                source.Append($@"            [""{elementName}""] = new List<Type>() {{ ");
                for (var i = 0; i < classNames.Count; i++)
                {
                    var className = classNames[i];
                    if (!string.IsNullOrWhiteSpace(className))
                    {
                        source.Append($"typeof({className}){((i < classNames.Count && classNames.Count > 1) ? ", " : "")}");
                    }
                }
                source.AppendLine($" }},");
            }
            source.Append($@"        }};
    }}
}}");

            context.AddSource($"{namespaceElementFactory.Replace('.', '_')}_{elementFactorySymbol.Name}_ElementFactory.cs", SourceText.From(source.ToString(), Encoding.UTF8));
            source.Clear();
        }

        /// <summary>
        /// Check if symbol has target base class
        /// </summary>
        /// <param name="namedTypeSymbol">The candidate class symbol.</param>
        /// <param name="targetBaseType">The base type class symbol</param>
        /// <returns>True is candidate class derives from base type, otherwise false.</returns>
        private static bool HasBaseType(INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol targetBaseType)
        {
            var baseType = namedTypeSymbol.BaseType;
            while (true)
            {
                if (baseType is null)
                {
                    break;
                }

                // We need to use SymbolEqualityComparer for symbol comparison.
                if (SymbolEqualityComparer.Default.Equals(baseType, targetBaseType))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Get symbol base types until provided type.
        /// </summary>
        /// <param name="namedTypeSymbol">The candidate class symbol.</param>
        /// <param name="targetBaseType">The base type class symbol</param>
        /// <returns>Returns a list of all base class symbol until target base type is reached.</returns>
        private static IEnumerable<INamedTypeSymbol> GetBaseTypes(INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol targetBaseType)
        {
            var baseType = namedTypeSymbol.BaseType;
            while (true)
            {
                if (baseType is null)
                {
                    break;
                }

                // We need to use SymbolEqualityComparer for symbol comparison.
                if (SymbolEqualityComparer.Default.Equals(baseType, targetBaseType))
                {
                    yield return baseType;
                    break;
                }

                yield return baseType;
                baseType = baseType.BaseType;
            }
        }

        /// <summary>
        /// Get the <see cref="ITypeSymbol"/> from symbol.
        /// </summary>
        /// <param name="symbol">The symbol object.</param>
        /// <returns>The <see cref="ITypeSymbol"/> for symbol or null value.</returns>
        private static ITypeSymbol? GetTypeSymbol(ISymbol symbol)
        {
            return symbol switch
            {
                IEventSymbol eventSymbol => eventSymbol.Type,
                IPropertySymbol propertySymbol => propertySymbol.Type,
                _ => null
            };
        }

        /// <summary>
        /// Get the <see cref="TypeConverter"/> type string set for property/event symbol or property/event symbol type.
        /// </summary>
        /// <param name="compilation">The compilation object.</param>
        /// <param name="symbol">The symbol object for property/event.</param>
        /// <returns>The <see cref="TypeConverter"/> type string set for property/event symbol or property/event symbol type.</returns>
        private static string? GetTypeConverter(Compilation compilation, ISymbol symbol)
        {
            var symbolType = GetTypeSymbol(symbol);
            if (symbolType is null)
            {
                return null;
            }

            // Get TypeConverterAttribute symbol using for later attribute retrieval.
            var typeConverterAttribute = compilation.GetTypeByMetadataName("System.ComponentModel.TypeConverterAttribute");
            if (typeConverterAttribute is null)
            {
                return null;
            }

            // Get converter from attribute explicitly set on property/event.
            var typeConverter = GetTypeConverter(symbol, typeConverterAttribute);
            if (typeConverter is not null)
            {
                return typeConverter;
            }

            // Get converter from attribute explicitly set on property/event type.
            var typeTypeConverter = GetTypeConverter(symbolType, typeConverterAttribute);
            if (typeTypeConverter is not null)
            {
                return typeTypeConverter;
            }

            // Get converter from property/event type.
            var format = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance
            );
            var typeString = $"{symbolType.ToDisplayString(format)}";
            var typeName = $"{typeString},{symbolType.ContainingAssembly}";
            var type = Type.GetType(typeName);
            if (type is not null)
            {
                return TypeDescriptor.GetConverter(type).ToString();
            }

            // Fallback for basic types if Type.GetType does not work.
            return typeString switch
            {
                "System.String" => "System.ComponentModel.StringConverter",
                "System.Single" => "System.ComponentModel.SingleConverter",
                "System.Uri" => "System.UriTypeConverter",
                _ => null
            };
        }

        /// <summary>
        /// Get the <see cref="TypeConverter"/> type string set as attribute on symbol.
        /// </summary>
        /// <param name="symbol">The symbol object.</param>
        /// <param name="typeConverterAttribute">The <see cref="TypeConverterAttribute"/> symbol.</param>
        /// <returns>The <see cref="TypeConverter"/> type string set as attribute on symbol.</returns>
        private static string? GetTypeConverter(ISymbol symbol, INamedTypeSymbol typeConverterAttribute)
        {
            var attributes = symbol.GetAttributes();
            if (attributes.Length == 0)
            {
                return null;
            }

            // Find typeConverterAttribute attribute data. We need only first constructor argument for attribute type.
            var attributeData = attributes.FirstOrDefault(ad => ad?.AttributeClass?.Equals(typeConverterAttribute, SymbolEqualityComparer.Default) ?? false);
            if (attributeData is null || attributeData.ConstructorArguments.Length < 1)
            {
                return null;
            }

            // The Type is set in attribute by providing constructor argument.

            return attributeData.ConstructorArguments[0].Value?.ToString();
        }

        /// <summary>
        /// Gets all properties/events from class that are annotated with SvgAttributeAttribute attribute.
        /// </summary>
        /// <param name="compilation">The compilation object.</param>
        /// <param name="svgElementSymbol">The target class symbol that derives from SvgElement.</param>
        /// <param name="svgElementBaseSymbol">The SvgElement base class symbol.</param>
        /// <param name="svgAttributeAttribute">The SvgAttributeAttribute attribute symbol.</param>
        /// <returns>List of all properties that are annotated with SvgAttributeAttribute attribute.</returns>
        private static IEnumerable<Property> GetElementProperties(Compilation compilation, INamedTypeSymbol svgElementSymbol, INamedTypeSymbol svgElementBaseSymbol, INamedTypeSymbol svgAttributeAttribute)
        {
            // Get all types base types plus target type so we get all properties from base objects too.
            var types = GetBaseTypes(svgElementSymbol, svgElementBaseSymbol).Prepend(svgElementSymbol);

            foreach (var type in types)
            {
                var members = type.GetMembers();
                foreach (var member in members)
                {
                    MemberType memberType;

                    // Filter type members to include only properties and events.
                    if (member is IPropertySymbol)
                    {
                        memberType = MemberType.Property;
                    }
                    else if (member is IEventSymbol)
                    {
                        memberType = MemberType.Event;
                    }
                    else
                    {
                        continue;
                    }

                    var attributes = member.GetAttributes();
                    if (attributes.Length == 0)
                    {
                        continue;
                    }

                    // Find svgAttributeAttribute attribute data. We need only first constructor argument for attribute 'name'.
                    var attributeData = attributes.FirstOrDefault(ad => ad?.AttributeClass?.Equals(svgAttributeAttribute, SymbolEqualityComparer.Default) ?? false);
                    if (attributeData is null || attributeData.ConstructorArguments.Length < 1)
                    {
                        continue;
                    }

                    // The Name is set in attribute by providing constructor argument.
                    var attributeName = (string?)attributeData.ConstructorArguments[0].Value;
                    if (attributeName is null)
                    {
                        continue;
                    }

                    var attributeNamespace = "http://www.w3.org/2000/svg";
                    if (attributeData.ConstructorArguments.Length == 2)
                    {
                        attributeNamespace = (string?)attributeData.ConstructorArguments[1].Value;
                    }

                    if (attributeNamespace is null)
                    {
                        continue;
                    }

                    var property = new Property(
                        member,
                        memberType,
                        attributeName,
                        attributeNamespace,
                        GetTypeConverter(compilation, member)
                    );

                    yield return property;
                }
            }
        }

        /// <summary>
        /// Symbol member type.
        /// </summary>
        private enum MemberType
        {
            /// <summary>
            /// Unknown symbol.
            /// </summary>
            Unknown,
            /// <summary>
            /// Property symbol.
            /// </summary>
            Property,
            /// <summary>
            /// Event symbol.
            /// </summary>
            Event
        }

        /// <summary>
        /// The SvgElement object property/event.
        /// </summary>
        private class Property
        {
            /// <summary>
            /// Gets or sets property/event symbol.
            /// </summary>
            public ISymbol Symbol { get; }

            /// <summary>
            /// Gets or sets property/event symbol member type.
            /// </summary>
            public MemberType MemberType { get; }

            /// <summary>
            /// Gets or sets property/event attribute name.
            /// </summary>
            public string AttributeName { get; }

            /// <summary>
            /// Gets or sets property/event attribute namespace.
            /// </summary>
            public string AttributeNamespace { get; }

            /// <summary>
            /// Gets or sets property/event type converter type string.
            /// </summary>
            public string? Converter { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Property"/> class.
            /// </summary>
            /// <param name="symbol">The property/event symbol.</param>
            /// <param name="memberType">The property/event symbol member type.</param>
            /// <param name="attributeName">The property/event attribute name.</param>
            /// <param name="attributeNamespace">The property/event attribute namespace.</param>
            /// <param name="converter">The property/event type converter type string.</param>
            public Property(ISymbol symbol, MemberType memberType, string attributeName, string attributeNamespace, string? converter)
            {
                Symbol = symbol;
                MemberType = memberType;
                AttributeName = attributeName;
                AttributeNamespace = attributeNamespace;
                Converter = converter;
            }
        }

        /// <summary>
        /// The SvgElement object.
        /// </summary>
        private class Element
        {
            /// <summary>
            /// Gets or sets element type symbol.
            /// </summary>
            public INamedTypeSymbol Symbol { get; }

            /// <summary>
            /// Gets or sets element base types list.
            /// </summary>
            public List<INamedTypeSymbol> BaseTypes { get; }

            /// <summary>
            /// Gets or sets element name.
            /// </summary>
            public string? ElementName { get; }

            /// <summary>
            /// Gets or sets classes that use element name.
            /// </summary>
            public List<string> ClassNames { get; }

            /// <summary>
            /// Gets or sets element properties list.
            /// </summary>
            public List<Property> Properties { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Element"/> class.
            /// </summary>
            /// <param name="symbol">The element type symbol.</param>
            /// <param name="baseTypes">The element base types list.</param>
            /// <param name="elementName">The element name.</param>
            /// <param name="classNames">The classes that use element name.</param>
            /// <param name="properties">The element properties list.</param>
            public Element(INamedTypeSymbol symbol, List<INamedTypeSymbol> baseTypes, string? elementName, List<string> classNames, List<Property> properties)
            {
                Symbol = symbol;
                BaseTypes = baseTypes;
                ElementName = elementName;
                ClassNames = classNames;
                Properties = properties;
            }
        }

        /// <summary>
        /// The SyntaxReceiver is used to filter compiled code. This enable quick and easy way to filter compiled code.
        /// </summary>
        private class SyntaxReceiver : ISyntaxReceiver
        {
            /// <summary>
            /// Gets the list of all candidate class.
            /// </summary>
            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

            /// <inheritdoc/>
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    CandidateClasses.Add(classDeclarationSyntax);
                }
            }
        }
    }
}