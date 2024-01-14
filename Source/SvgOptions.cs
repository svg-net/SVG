using System;
using System.Collections;
using System.Collections.Generic;
#if NETCORE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Svg
{
    public class SvgOptions : IDictionary<string, string>, ICloneable
    {
        private readonly IDictionary<string, string> _properties;
        private Dictionary<string, string> _entities;

        public SvgOptions()
        {
            _properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    
        public SvgOptions(Dictionary<string, string> entities)
            : this()
        {
            _entities = entities;
        }

        public SvgOptions(Dictionary<string, string> entities, string css)
            : this()
        {
            _entities = entities;
            this.SetValue(nameof(Css), css);
        }

        public SvgOptions(string css)
            : this()
        {
            this.SetValue(nameof(Css), css);
        }

        public Dictionary<string, string> Entities {
            get => _entities;
            set => _entities = value;
        }

        public string Css {
            get => this.GetValue(nameof(Css));
            set => this.SetValue(nameof(Css), value);
        }

        public string this[string key]
        {
            get => this.GetValue(key, string.Empty);
            set => this.SetValue(key, value);
        }

        public ICollection<string> Keys => _properties.Keys;

        public ICollection<string> Values => _properties.Values;

        public int Count => _properties.Count;

        public bool IsReadOnly => _properties.IsReadOnly;

        public void Add(string key, string value)
        {
            if (key != null)
            {
                _properties.Add(key, value);
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _properties.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _properties.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _properties.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _properties.Remove(item);
        }

#if NETCORE
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            return _properties.TryGetValue(key, out value);
        }
#else
        public bool TryGetValue(string key, out string value)
        {
            return _properties.TryGetValue(key, out value);
        }
#endif

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_properties).GetEnumerator();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public SvgOptions Clone()
        {
            SvgOptions options = new SvgOptions();
            foreach (KeyValuePair<string, string> item in _properties)
            {
                options.Add(item);
            }
            if (_entities != null)
            {
                Dictionary<string, string> entities = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> item in _entities)
                {
                    entities.Add(item.Key, item.Value);
                }
                options._entities = entities;
            }

            return options;
        }

        protected string GetValue(string key, string defaultVal = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultVal;
            }
            if (_properties.TryGetValue(key, out string value))
            {
                return value;
            }

            return defaultVal;
        }

        protected void SetValue(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            _properties[key] = value;
        }
    }
}

