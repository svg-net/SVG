using System;
using System.Collections.Generic;

namespace Svg
{
    [SvgElement("use")]
    public partial class SvgUse : SvgVisualElement
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return GetAttribute<Uri>("href", false); }
            set { Attributes["href"] = value; }
        }

        private bool ElementReferencesUri(SvgElement element, List<Uri> elementUris)
        {
            var useElement = element as SvgUse;
            if (useElement != null)
            {
                if (elementUris.Contains(useElement.ReferencedElement))
                {
                    return true;
                }
                // also detect cycles in referenced elements
                var refElement = this.OwnerDocument.IdManager.GetElementById(useElement.ReferencedElement);
                if (refElement is SvgUse)
                {
                    elementUris.Add(useElement.ReferencedElement);
                }
                return useElement.ReferencedElementReferencesUri(elementUris);
            }
            var groupElement = element as SvgGroup;
            if (groupElement != null)
            {
                foreach (var child in groupElement.Children)
                {
                    if (ElementReferencesUri(child, elementUris))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ReferencedElementReferencesUri(List<Uri> elementUris)
        {
            var refElement = this.OwnerDocument.IdManager.GetElementById(ReferencedElement);
            return ElementReferencesUri(refElement, elementUris);
        }

        /// <summary>
        /// Checks for any direct or indirect recursions in referenced elements,
        /// including recursions via groups.
        /// </summary>
        /// <returns>True if any recursions are found.</returns>
        private bool HasRecursiveReference()
        {
            var refElement = this.OwnerDocument.IdManager.GetElementById(ReferencedElement);
            var uris = new List<Uri>() { ReferencedElement };
            return ElementReferencesUri(refElement, uris);
        }

        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return GetAttribute<SvgUnit>("x", false, 0f); }
            set { Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return GetAttribute<SvgUnit>("y", false, 0f); }
            set { Attributes["y"] = value; }
        }

        [SvgAttribute("width")]
        public virtual SvgUnit Width
        {
            get { return GetAttribute<SvgUnit>("width", false, 0f); }
            set { Attributes["width"] = value; }
        }

        [SvgAttribute("height")]
        public virtual SvgUnit Height
        {
            get { return GetAttribute<SvgUnit>("height", false, 0f); }
            set { Attributes["height"] = value; }
        }

        /// <summary>
        /// Gets an <see cref="SvgPoint"/> representing the top left point of the rectangle.
        /// </summary>
        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

        protected override bool Renderable { get { return false; } }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgUse>();
        }
    }
}
