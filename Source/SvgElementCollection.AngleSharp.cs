#if AngleSharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace Svg
{
    public partial class SvgElementCollection : INodeList, IHtmlCollection<IElement>
    {
        IEnumerator<IElement> IEnumerable<IElement>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<INode> IEnumerable<INode>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
        {
            throw new NotImplementedException();
        }

        INode INodeList.this[int index] => _elements[index];

        public IElement this[string id] => _elements.FirstOrDefault(f => f.ID == id);

        public int Length => _elements.Count;

        IElement IHtmlCollection<IElement>.this[int index] => this[index];
    }
}
#endif
