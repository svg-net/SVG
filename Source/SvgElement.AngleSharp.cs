#if AngleSharp
using System;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;

namespace Svg
{
    public partial class SvgElement : IElement, INode, IParentNode, IChildNode, INonDocumentTypeChildNode
    {
        public void AddEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new NotImplementedException();
        }

        public void InvokeEventListener(Event ev)
        {
            throw new NotImplementedException();
        }

        public bool Dispatch(Event ev)
        {
            throw new NotImplementedException();
        }

        public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
        {
            throw new NotImplementedException();
        }

        public INode Clone(bool deep = true)
        {
            throw new NotImplementedException();
        }

        public bool Equals(INode otherNode)
        {
            throw new NotImplementedException();
        }

        public DocumentPositions CompareDocumentPosition(INode otherNode)
        {
            throw new NotImplementedException();
        }

        public void Normalize()
        {
            throw new NotImplementedException();
        }

        public bool Contains(INode otherNode)
        {
            throw new NotImplementedException();
        }

        public bool IsDefaultNamespace(string namespaceUri)
        {
            throw new NotImplementedException();
        }

        public string LookupNamespaceUri(string prefix)
        {
            throw new NotImplementedException();
        }

        public string LookupPrefix(string namespaceUri)
        {
            throw new NotImplementedException();
        }

        public INode AppendChild(INode child)
        {
            throw new NotImplementedException();
        }

        public INode InsertBefore(INode newElement, INode referenceElement)
        {
            throw new NotImplementedException();
        }

        public INode RemoveChild(INode child)
        {
            throw new NotImplementedException();
        }

        public INode ReplaceChild(INode newChild, INode oldChild)
        {
            throw new NotImplementedException();
        }

        public string BaseUri  => throw new NotImplementedException();
        public Url BaseUrl  => throw new NotImplementedException();
        public string NodeName  => throw new NotImplementedException();
        public INodeList ChildNodes => this.Children;
        public IDocument Owner  => throw new NotImplementedException();
        public IElement ParentElement  => Parent;
        INode INode.Parent => Parent;

        public INode FirstChild => throw new NotImplementedException();
        public INode LastChild  => throw new NotImplementedException();
        public INode NextSibling  => throw new NotImplementedException();
        public INode PreviousSibling  => throw new NotImplementedException();
        public NodeType NodeType  => throw new NotImplementedException();
        public string NodeValue  {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public string TextContent  {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public bool HasChildNodes  => throw new NotImplementedException();
        public NodeFlags Flags  => throw new NotImplementedException();
        public void Append(params INode[] nodes)
        {
            throw new NotImplementedException();
        }

        public void Prepend(params INode[] nodes)
        {
            throw new NotImplementedException();
        }

        public IElement QuerySelector(string selectors)
        {
            throw new NotImplementedException();
        }

        public IHtmlCollection<IElement> QuerySelectorAll(string selectors)
        {
            return ChildNodes.QuerySelectorAll(selectors, this);
        }

        IHtmlCollection<IElement> IParentNode.Children => Children;

        public IElement FirstElementChild  => throw new NotImplementedException();
        public IElement LastElementChild  => throw new NotImplementedException();
        public int ChildElementCount  => throw new NotImplementedException();
        public void Before(params INode[] nodes)
        {
            throw new NotImplementedException();
        }

        public void After(params INode[] nodes)
        {
            throw new NotImplementedException();
        }

        public void Replace(params INode[] nodes)
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public IElement NextElementSibling => throw new NotImplementedException();
        public IElement PreviousElementSibling  => throw new NotImplementedException();
        public void Insert(AdjacentPosition position, string html)
        {
            throw new NotImplementedException();
        }

        public bool HasAttribute(string name)
        {
            throw new NotImplementedException();
        }

        public bool HasAttribute(string namespaceUri, string localName)
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string name)
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string namespaceUri, string localName)
        {
            throw new NotImplementedException();
        }

        public void SetAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetAttribute(string namespaceUri, string name, string value)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAttribute(string name)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAttribute(string namespaceUri, string localName)
        {
            throw new NotImplementedException();
        }

        public IHtmlCollection<IElement> GetElementsByClassName(string classNames)
        {
            throw new NotImplementedException();
        }

        public IHtmlCollection<IElement> GetElementsByTagName(string tagName)
        {
            throw new NotImplementedException();
        }

        public IHtmlCollection<IElement> GetElementsByTagNameNS(string namespaceUri, string tagName)
        {
            throw new NotImplementedException();
        }

        public bool Matches(string selectors)
        {
            throw new NotImplementedException();
        }

        public IElement Closest(string selectors)
        {
            throw new NotImplementedException();
        }

        public IShadowRoot AttachShadow(ShadowRootMode mode = ShadowRootMode.Open)
        {
            throw new NotImplementedException();
        }

        public string Prefix  => throw new NotImplementedException();
        public string LocalName  => AttributeName;
        public string NamespaceUri  => throw new NotImplementedException();
        public string GivenNamespaceUri  => throw new NotImplementedException();
        public ITokenList ClassList  => throw new NotImplementedException();

        public string ClassName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string Id
        {
            get => ID;
            set => ID = value;
        }

        public string InnerHtml
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public string OuterHtml
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public string TagName => throw new NotImplementedException();
        public IElement AssignedSlot => throw new NotImplementedException();
        public string Slot
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public IShadowRoot ShadowRoot => throw new NotImplementedException();
        public bool IsFocused => throw new NotImplementedException();
        public ISourceReference SourceReference => throw new NotImplementedException();
        INamedNodeMap IElement.Attributes => throw new NotImplementedException();
    }
}
#endif
