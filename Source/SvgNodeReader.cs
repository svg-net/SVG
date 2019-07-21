using System;
using System.Collections.Generic;
using System.Xml;

namespace Svg
{
    internal sealed class SvgNodeReader : XmlNodeReader
    {
        private Dictionary<string, string> _entities;
        private string _value;
        private bool _customValue = false;
        private string _localName;

        public SvgNodeReader(XmlNode node, Dictionary<string, string> entities)
            : base(node)
        {
            _entities = entities ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the text value of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>The value returned depends on the <see cref="P:System.Xml.XmlTextReader.NodeType"/> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node Type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space within an xml:space= 'preserve' scope. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
        public override string Value
        {
            get
            {
                return _customValue ? _value : base.Value;
            }
        }

        /// <summary>
        /// Gets the local name of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.</returns>
        public override string LocalName
        {
            get
            {
                return _customValue ? _localName : base.LocalName;
            }
        }

        /// <summary>
        /// Moves to the next attribute.
        /// </summary>
        /// <returns>
        /// true if there is a next attribute; false if there are no more attributes.
        /// </returns>
        public override bool MoveToNextAttribute()
        {
            bool moved = base.MoveToNextAttribute();

            if (moved)
            {
                _localName = base.LocalName;

                if (ReadAttributeValue())
                {
                    if (NodeType == XmlNodeType.EntityReference)
                    {
                        ResolveEntity();
                    }
                    else
                    {
                        _value = base.Value;
                    }
                }
                _customValue = true;
            }

            return moved;
        }

        /// <summary>
        /// Reads the next node from the stream.
        /// </summary>
        /// <returns>
        /// true if the next node was read successfully; false if there are no more nodes to read.
        /// </returns>
        /// <exception cref="T:System.Xml.XmlException">An error occurred while parsing the XML. </exception>
        public override bool Read()
        {
            _customValue = false;
            bool read = base.Read();

            if (NodeType == XmlNodeType.DocumentType)
            {
                ParseEntities();
            }

            return read;
        }

        private void ParseEntities()
        {
            const string entityText = "<!ENTITY";
            string[] entities = Value.Split(new string[] { entityText }, StringSplitOptions.None);
            string[] parts = null;
            string name = null;
            string value = null;

            foreach (string entity in entities)
            {
                if (string.IsNullOrEmpty(entity.Trim()))
                {
                    continue;
                }

                parts = entity.Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                name = parts[0];
                value = parts[1].Split(new char[] { QuoteChar }, StringSplitOptions.RemoveEmptyEntries)[0];

                _entities.Add(name, value);
            }
        }

        /// <summary>
        /// Resolves the entity reference for EntityReference nodes.
        /// </summary>
        public override void ResolveEntity()
        {
            if (NodeType == XmlNodeType.EntityReference)
            {
                if (_entities.ContainsKey(Name))
                {
                    _value = _entities[Name];
                }
                else
                {
                    _value = string.Empty;
                }

                _customValue = true;
            }
        }
    }
}