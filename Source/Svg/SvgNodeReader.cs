using System;
using System.Collections.Generic;
using System.Xml;

namespace Svg
{
    internal sealed class SvgNodeReader : XmlNodeReader
    {
        private readonly Dictionary<string, string> _entities;
        private string _value;
        private bool _customValue = false;

        public SvgNodeReader(XmlNode node, Dictionary<string, string> entities)
            : base(node)
        {
            _entities = entities ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the text value of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>The value returned depends on the <see cref="P:System.Xml.XmlNodeReader.NodeType"/> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node Type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space within an xml:space= 'preserve' scope. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
        public override string Value
        {
            get { return _customValue ? _value : base.Value; }
        }

        /// <summary>
        /// Reads the attribute value.
        /// </summary>
        /// <returns>
        /// true if there is a attribute value; false if there are no attribute value.
        /// </returns>
        public override bool ReadAttributeValue()
        {
            _customValue = false;
            var read = base.ReadAttributeValue();

            if (read && NodeType == XmlNodeType.EntityReference)
                ResolveEntity();
            return read;
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
                ParseEntities();

            return read;
        }

        private void ParseEntities()
        {
            const string entityText = "<!ENTITY";
            var entities = Value.Split(new string[] { entityText }, StringSplitOptions.None);

            foreach (var entity in entities)
            {
                if (string.IsNullOrEmpty(entity.Trim()))
                    continue;

                var parts = entity.Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var name = parts[0];
                var value = parts[1].Split(new char[] { QuoteChar }, StringSplitOptions.RemoveEmptyEntries)[0];

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
                    base.ResolveEntity();
                    _value = ReadAttributeValue() ? base.Value : string.Empty;
                }

                _customValue = true;
            }
        }
    }
}
