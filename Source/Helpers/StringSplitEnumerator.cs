using System;

namespace Svg.Helpers
{
    internal ref struct StringSplitEnumerator
    {
        private ReadOnlySpan<char> _str;
        private readonly ReadOnlySpan<char> _chars;

        public StringSplitEnumerator(ReadOnlySpan<char> str, ReadOnlySpan<char> chars)
        {
            _str = str;
            _chars = chars;
            Current = default;
        }

        public StringSplitEnumerator GetEnumerator() => this;

        public bool MoveNext()
        {
            while (true)
            {
                var span = _str;

                if (span.Length == 0)
                {
                    return false;
                }

                var index = span.IndexOfAny(_chars);
                if (index == -1)
                {
                    Current = new StringPart(span);
                    _str = ReadOnlySpan<char>.Empty;
                    return true;
                }

                var slice = span.Slice(0, index).Trim();
                _str = span.Slice(index + 1);
                if (slice.Length == 0)
                {
                    continue;
                }
                Current = new StringPart(slice);
                return true;
            }
        }

        public StringPart Current { get; private set; }
    }

    internal readonly ref struct StringPart
    {
        public StringPart(ReadOnlySpan<char> value)
        {
            Value = value;
        }

        public ReadOnlySpan<char> Value { get; }

        public static implicit operator ReadOnlySpan<char>(StringPart part) => part.Value;
    }
}
