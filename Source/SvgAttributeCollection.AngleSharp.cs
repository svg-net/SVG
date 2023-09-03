#if AngleSharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Svg
{
    public partial class SvgAttributeCollection : ITokenList
    {
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.Keys.GetEnumerator();
        }

        public bool Contains(string token)
        {
            return this.TryGetValue(token, out _);
        }

        public void Add(params string[] tokens)
        {
            throw new NotImplementedException();
        }

        public void Remove(params string[] tokens)
        {
            throw new NotImplementedException();
        }

        public bool Toggle(string token, bool force = false)
        {
            throw new NotImplementedException();
        }

        public int Length => throw new NotImplementedException();

        public string this[int index] => throw new NotImplementedException();
    }
}
#endif


