using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// The base class of which all SVG elements are derived from.
    /// </summary>
    public abstract class SvgElement : ISvgElement, ISvgTransformable, ICloneable
    {
        //optimization
        protected class AttributeTuple
        {
            public PropertyDescriptor Property;
            public SvgAttributeAttribute Attribute;
        }
        protected readonly IEnumerable<AttributeTuple> _svgAttributes;


        internal SvgElement _parent;
        private string _elementName;
        private SvgAttributeCollection _attributes;
        private readonly EventHandlerList _eventHandlers;
        private readonly SvgElementCollection _children;
        private static readonly object _loadEventKey = new object();
        private Matrix _graphicsMatrix;
        private readonly Dictionary<string, string> _customAttributes;

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        protected internal string ElementName
        {
            get
            {
                if (string.IsNullOrEmpty(this._elementName))
                {
                    var attr = TypeDescriptor.GetAttributes(this).OfType<SvgElementAttribute>().SingleOrDefault();

                    if (attr != null)
                    {
                        this._elementName = attr.ElementName;
                    }
                }

                return this._elementName;
            }
            internal set { this._elementName = value; }
        }

        /// <summary>
        /// Gets or sets the content of the element.
        /// </summary>
        public virtual string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets an <see cref="EventHandlerList"/> of all events belonging to the element.
        /// </summary>
        protected virtual EventHandlerList Events
        {
            get { return this._eventHandlers; }
        }

        /// <summary>
        /// Occurs when the element is loaded.
        /// </summary>
        public event EventHandler Load
        {
            add { this.Events.AddHandler(_loadEventKey, value); }
            remove { this.Events.RemoveHandler(_loadEventKey, value); }
        }

        /// <summary>
        /// Gets a collection of all child <see cref="SvgElement"/> objects.
        /// </summary>
        public virtual SvgElementCollection Children
        {
            get { return this._children; }
        }

        /// <summary>
        /// Gets a value to determine whether the element has children.
        /// </summary>
        public virtual bool HasChildren()
        {
            return (this.Children.Count > 0);
        }

        /// <summary>
        /// Gets the parent <see cref="SvgElement"/>.
        /// </summary>
        /// <value>An <see cref="SvgElement"/> if one exists; otherwise null.</value>
        public virtual SvgElement Parent
        {
            get { return this._parent; }
        }

        /// <summary>
        /// Gets the owner <see cref="SvgDocument"/>.
        /// </summary>
        public virtual SvgDocument OwnerDocument
        {
            get
            {
                if (Parent == null)
                {
                    var document = this as SvgDocument;
                    if (document != null)
                    {
                        return document;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return Parent.OwnerDocument;
                }
            }
        }

        /// <summary>
        /// Gets a collection of element attributes.
        /// </summary>
        protected internal virtual SvgAttributeCollection Attributes
        {
            get
            {
                if (this._attributes == null)
                {
                    this._attributes = new SvgAttributeCollection(this);
                }

                return this._attributes;
            }
        }

        public Dictionary<string, string> CustomAttributes
        {
            get { return this._customAttributes; }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to be transformed.</param>
        protected internal virtual void PushTransforms(SvgRenderer renderer)
        {
            _graphicsMatrix = renderer.Transform;

            // Return if there are no transforms
            if (this.Transforms == null || this.Transforms.Count == 0)
            {
                return;
            }

            Matrix transformMatrix = renderer.Transform;

            foreach (SvgTransform transformation in this.Transforms)
            {
                transformMatrix.Multiply(transformation.Matrix, MatrixOrder.Append);
            }

            renderer.Transform = transformMatrix;
        }

        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> that should have transforms removed.</param>
        protected internal virtual void PopTransforms(SvgRenderer renderer)
        {
            renderer.Transform = _graphicsMatrix;
            _graphicsMatrix = null;
        }

        /// <summary>
        /// Applies the required transforms to <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to be transformed.</param>
        void ISvgTransformable.PushTransforms(SvgRenderer renderer)
        {
            this.PushTransforms(renderer);
        }

        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> that should have transforms removed.</param>
        void ISvgTransformable.PopTransforms(SvgRenderer renderer)
        {
            this.PopTransforms(renderer);
        }

        /// <summary>
        /// Gets or sets the element transforms.
        /// </summary>
        /// <value>The transforms.</value>
        [SvgAttribute("transform")]
        public SvgTransformCollection Transforms
        {
            get { return (this.Attributes.GetAttribute<SvgTransformCollection>("Transforms") ?? new SvgTransformCollection()); }
            set { this.Attributes["Transforms"] = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the element.
        /// </summary>
        /// <exception cref="SvgException">The ID is already used within the <see cref="SvgDocument"/>.</exception>
        [SvgAttribute("id")]
        public string ID
        {
            get { return this.Attributes.GetAttribute<string>("ID"); }
            set
            {
                // Don't do anything if it hasn't changed
                if (string.Compare(this.ID, value) == 0)
                {
                    return;
                }

                if (this.OwnerDocument != null)
                {
                    this.OwnerDocument.IdManager.Remove(this);
                }

                this.Attributes["ID"] = value;

                if (this.OwnerDocument != null)
                {
                    this.OwnerDocument.IdManager.Add(this);
                }
            }
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected virtual void AddElement(SvgElement child, int index)
        {
        }

        /// <summary>
        /// Calls the <see cref="AddElement"/> method with the specified parameters.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        internal void OnElementAdded(SvgElement child, int index)
        {
            this.AddElement(child, index);
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected virtual void RemoveElement(SvgElement child)
        {
        }

        /// <summary>
        /// Calls the <see cref="RemoveElement"/> method with the specified <see cref="SvgElement"/> as the parameter.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        internal void OnElementRemoved(SvgElement child)
        {
            this.RemoveElement(child);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgElement"/> class.
        /// </summary>
        public SvgElement()
        {
            this._children = new SvgElementCollection(this);
            this._eventHandlers = new EventHandlerList();
            this._elementName = string.Empty;
            this._customAttributes = new Dictionary<string, string>();

            //fill svg attribute description
            _svgAttributes = from PropertyDescriptor a in TypeDescriptor.GetProperties(this)
                            let attribute = a.Attributes[typeof(SvgAttributeAttribute)] as SvgAttributeAttribute
                            where attribute != null
                            select new AttributeTuple { Property = a, Attribute = attribute };
        }


        public virtual void InitialiseFromXML(XmlTextReader reader, SvgDocument document)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders this element to the <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> that the element should use to render itself.</param>
        public void RenderElement(SvgRenderer renderer)
        {
            this.Render(renderer);
        }

        public void WriteElement(XmlTextWriter writer)
        {
            this.Write(writer);
        }

        protected virtual void WriteStartElement(XmlTextWriter writer)
        {
            if (this.ElementName != String.Empty)
            {
                writer.WriteStartElement(this.ElementName);
                if (this.ElementName == "svg")
                {
                    foreach (var ns in SvgAttributeAttribute.Namespaces)
                    {
                        if (string.IsNullOrEmpty(ns.Key))
                            writer.WriteAttributeString("xmlns", ns.Value);
                        else
                            writer.WriteAttributeString("xmlns:" + ns.Key, ns.Value);
                    }
                    writer.WriteAttributeString("version", "1.1");
                }
            }
            this.WriteAttributes(writer);
        }

        protected virtual void WriteEndElement(XmlTextWriter writer)
        {
            if (this.ElementName != String.Empty)
            {
                writer.WriteEndElement();
            }
        }

        protected virtual void WriteAttributes(XmlTextWriter writer)
        {
            foreach (var attr in _svgAttributes)
            {
                if (attr.Property.Converter.CanConvertTo(typeof(string)))
                {
                    object propertyValue = attr.Property.GetValue(this);

                    var forceWrite = false;
                    if ((attr.Attribute.Name == "fill") && (Parent != null))
                    {
                        object parentValue;
                        if (TryResolveParentAttributeValue(attr.Attribute.Name, out parentValue))
                        {
                            if ((parentValue == propertyValue) 
                                || ((parentValue != null) &&  parentValue.Equals(propertyValue)))
                                continue;
                            
                            forceWrite = true;
                        }
                    }

                    if (propertyValue != null)
                    {
                        var type = propertyValue.GetType();
                        string value = (string)attr.Property.Converter.ConvertTo(propertyValue, typeof(string));

                        if (!SvgDefaults.IsDefault(attr.Attribute.Name, value) || forceWrite)
                        {
                            writer.WriteAttributeString(attr.Attribute.NamespaceAndName, value);
                        }
                    }
                    else if(attr.Attribute.Name == "fill") //if fill equals null, write 'none'
                    {
                        string value = (string)attr.Property.Converter.ConvertTo(propertyValue, typeof(string));
                        writer.WriteAttributeString(attr.Attribute.NamespaceAndName, value);
                    }
                }
            }

            //add the custom attributes
            foreach (var item in this._customAttributes)
            {
                writer.WriteAttributeString(item.Key, item.Value);
            }
        }

        private bool TryResolveParentAttributeValue(string attributeKey, out object parentAttributeValue)
        {
            parentAttributeValue = null;

            attributeKey = char.ToUpper(attributeKey[0]) + attributeKey.Substring(1);

            var currentParent = Parent;
            var resolved = false;
            while (currentParent != null)
            {
                if (currentParent.Attributes.ContainsKey(attributeKey))
                {
                    resolved = true;
                    parentAttributeValue = currentParent.Attributes[attributeKey];
                    if (parentAttributeValue != null)
                        break;
                }
                currentParent = currentParent.Parent;
            }

            return resolved;
        }

        protected virtual void Write(XmlTextWriter writer)
        {
            if (this.ElementName != String.Empty)
            {
                this.WriteStartElement(writer);
                this.WriteChildren(writer);
                this.WriteEndElement(writer);
            }
        }

        protected virtual void WriteChildren(XmlTextWriter writer)
        {
            //write the content
            if(!String.IsNullOrEmpty(this.Content))
                writer.WriteString(this.Content);

            //write all children
            foreach (SvgElement child in this.Children)
            {
                child.Write(writer);
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="SvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> object to render to.</param>
        protected virtual void Render(SvgRenderer renderer)
        {
            this.PushTransforms(renderer);
            this.RenderChildren(renderer);
            this.PopTransforms(renderer);
        }

        /// <summary>
        /// Renders the children of this <see cref="SvgElement"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to render the child <see cref="SvgElement"/>s to.</param>
        protected virtual void RenderChildren(SvgRenderer renderer)
        {
            foreach (SvgElement element in this.Children)
            {
                element.Render(renderer);
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="SvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> object to render to.</param>
        void ISvgElement.Render(SvgRenderer renderer)
        {
            this.Render(renderer);
        }
        
        /// <summary>
        /// Recursive method to add up the paths of all children
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="path"></param>
        protected void AddPaths(SvgElement elem, GraphicsPath path)
        {
            foreach(var child in elem.Children)
            {
                var element = child as SvgVisualElement;
                if (element != null)
                {
                    if(!(child is SvgGroup))
                    {
                        var childPath = element.Path;
                        
                        if (childPath != null)
                        {
                            childPath = (GraphicsPath)childPath.Clone();
                            if(element.Transforms != null)
                                childPath.Transform(element.Transforms.GetMatrix());
                            
                            path.AddPath(childPath, false);
                        }
                    }
                }
                    
                AddPaths(child, path);
            }
        }
        
        /// <summary>
        /// Recursive method to add up the paths of all children
        /// </summary>
        /// <param name="elem"></param>
        protected GraphicsPath GetPaths(SvgElement elem)
        {
            var ret = new GraphicsPath();
            
            foreach(var child in elem.Children)
            {
                var element = child as SvgVisualElement;
                if (element != null)
                {
                    if(!(child is SvgGroup))
                    {
                        var childPath = element.Path;
                        
                        if (childPath != null)
                        {
                            childPath = (GraphicsPath)childPath.Clone();
                            if(element.Transforms != null)
                                childPath.Transform(element.Transforms.GetMatrix());
                            
                            ret.AddPath(childPath, false);
                        }
                    }
                    else
                    {
                        var childPath = GetPaths(element);
                        if(element.Transforms != null)
                            childPath.Transform(element.Transforms.GetMatrix());
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public abstract SvgElement DeepCopy();

        public virtual SvgElement DeepCopy<T>() where T : SvgElement, new()
        {
            var newObj = new T();
            newObj.Content = this.Content;
            newObj.ElementName = this.ElementName;
//            if (this.Parent != null)
    //            this.Parent.Children.Add(newObj);

            if (this.Transforms != null)
            {
                newObj.Transforms = new SvgTransformCollection();
                foreach (var transform in this.Transforms)
                    newObj.Transforms.Add(transform.Clone() as SvgTransform);
            }

            foreach (var child in this.Children)
            {
                newObj.Children.Add(child.DeepCopy());
            }
                

            return newObj;
        }
    }

    internal interface ISvgElement
    {
        SvgElement Parent {get;}
        SvgElementCollection Children { get; }

        void Render(SvgRenderer renderer);
    }
}