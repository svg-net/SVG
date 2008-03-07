using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// The base class of which all SVG elements are derived from.
    /// </summary>
    public abstract class SvgElement : ISvgElement, ISvgTransformable, ICloneable
    {
        internal SvgElement _parent;
        private string _content;
        private string _elementName;
        private SvgAttributeCollection _attributes;
        private EventHandlerList _eventHandlers;
        private SvgElementCollection _children;
        private static readonly object _loadEventKey = new object();
        private Matrix _graphicsMatrix;

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        protected virtual string ElementName
        {
            get { return this._elementName; }
        }

        /// <summary>
        /// Gets or sets the content of the element.
        /// </summary>
        public virtual string Content
        {
            get { return this._content; }
            set { this._content = value; }
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
        /// Gets a collection of all child <see cref="SvgElements"/>.
        /// </summary>
        public virtual SvgElementCollection Children
        {
            get { return this._children; }
        }

        /// <summary>
        /// Gets a value to determine whether the element has children.
        /// </summary>
        /// <returns></returns>
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
                    if (this is SvgDocument)
                        return (SvgDocument)this;
                    else
                        return null;
                }
                else
                    return Parent.OwnerDocument;
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

        protected internal virtual void PushTransforms(Graphics graphics)
        {
            // Return if there are no transforms
            if (this.Transforms == null || this.Transforms.Count == 0)
            {
                return;
            }

            _graphicsMatrix = graphics.Transform;

            Matrix transformMatrix = new Matrix();

            foreach (SvgTransform transformation in this.Transforms)
            {
                transformMatrix.Multiply(transformation.Matrix);
            }

            graphics.Transform = transformMatrix;
        }

        protected internal virtual void PopTransforms(Graphics graphics)
        {
            if (this.Transforms == null || this.Transforms.Count == 0 || _graphicsMatrix == null)
            {
                return;
            }

            graphics.Transform = _graphicsMatrix;
            _graphicsMatrix = null;
        }

        void ISvgTransformable.PushTransforms(Graphics graphics)
        {
            this.PushTransforms(graphics);
        }

        void ISvgTransformable.PopTransforms(Graphics graphics)
        {
            this.PopTransforms(graphics);
        }

        /// <summary>
        /// Gets or sets the element transforms.
        /// </summary>
        /// <value>The transforms.</value>
        [SvgAttribute("transform")]
        public SvgTransformCollection Transforms
        {
            get { return this.Attributes.GetAttribute<SvgTransformCollection>("Transforms"); }
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
                    return;

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

        protected virtual void ElementAdded(SvgElement child, int index)
        {
        }

        internal void OnElementAdded(SvgElement child, int index)
        {
            this.ElementAdded(child, index);
        }

        protected virtual void ElementRemoved(SvgElement child)
        {
        }

        internal void OnElementRemoved(SvgElement child)
        {
            this.ElementRemoved(child);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgElement"/> class.
        /// </summary>
        internal SvgElement()
        {
            this._children = new SvgElementCollection(this);
            this._eventHandlers = new EventHandlerList();
            this._elementName = string.Empty;
        }

        public void RenderElement(Graphics graphics)
        {
            this.Render(graphics);
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
            foreach (SvgElement child in this.Children)
            {
                child.Write(writer);
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object to render to.</param>
        protected virtual void Render(Graphics graphics)
        {
            this.PushTransforms(graphics);
            this.RenderContents(graphics);
            this.PopTransforms(graphics);
        }

        protected virtual void RenderContents(Graphics graphics)
        {
            foreach (SvgElement element in this.Children)
            {
                element.Render(graphics);
            }
        }

        void ISvgElement.Render(Graphics graphics)
        {
            this.Render(graphics);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    internal interface ISvgElement
    {
        void Render(Graphics graphics);
    }
}