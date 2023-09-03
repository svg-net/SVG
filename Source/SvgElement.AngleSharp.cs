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
        private IHtmlCollection<IElement> _children1;

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

        public string BaseUri { get; }
        public Url BaseUrl { get; }
        public string NodeName { get; }
        public INodeList ChildNodes { get; }
        public IDocument Owner { get; }
        public IElement ParentElement { get; }
        INode INode.Parent => Parent;

        public INode FirstChild { get; }
        public INode LastChild { get; }
        public INode NextSibling { get; }
        public INode PreviousSibling { get; }
        public NodeType NodeType { get; }
        public string NodeValue { get; set; }
        public string TextContent { get; set; }
        public bool HasChildNodes { get; }
        public NodeFlags Flags { get; }
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
            throw new NotImplementedException();
        }

        IHtmlCollection<IElement> IParentNode.Children => _children1;

        public IElement FirstElementChild { get; }
        public IElement LastElementChild { get; }
        public int ChildElementCount { get; }
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

        public IElement NextElementSibling { get; }
        public IElement PreviousElementSibling { get; }
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

        public string Prefix { get; }
        public string LocalName { get; }
        public string NamespaceUri { get; }
        public string GivenNamespaceUri { get; }
        public ITokenList ClassList { get; }
        public string ClassName { get; set; }
        public string Id { get; set; }
        public string InnerHtml { get; set; }
        public string OuterHtml { get; set; }
        public string TagName { get; }
        public IElement AssignedSlot { get; }
        public string Slot { get; set; }
        public IShadowRoot ShadowRoot { get; }
        public bool IsFocused { get; }
        public ISourceReference SourceReference { get; }
        INamedNodeMap IElement.Attributes { get; } 
    }
}
#endif
