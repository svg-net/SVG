using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Svg
{
    public partial class SvgElement : ITokenList
    {
        public IEnumerator<string> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(string token)
        {
            if (this.TryGetAttribute("class", out var value))
            {
                return value.Split(' ')?.Contains(token) ?? false;
            }

            return false;
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

        public int Length { get; }

        public string this[int index] => throw new NotImplementedException();
    }
}
