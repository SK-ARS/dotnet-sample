namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal class PatchChange : Microsoft.XmlDiffPatch.XmlPatchParentOperation
    {
        private XmlNode _matchNode;
        private string _name;
        private string _ns;
        private string _prefix;
        private string _value;

        internal PatchChange(XmlNode matchNode, string name, string ns, string prefix, XmlNode diffChangeNode)
        {
            this._matchNode = matchNode;
            this._name = name;
            this._ns = ns;
            this._prefix = prefix;
            if (diffChangeNode == null)
            {
                this._value = null;
            }
            else
            {
                switch (matchNode.NodeType)
                {
                    case XmlNodeType.ProcessingInstruction:
                        if (name == null)
                        {
                            this._name = diffChangeNode.FirstChild.Name;
                            this._value = diffChangeNode.FirstChild.Value;
                        }
                        return;

                    case XmlNodeType.Comment:
                        this._value = diffChangeNode.FirstChild.Value;
                        return;
                }
                this._value = matchNode.InnerText + "##**##" + diffChangeNode.InnerText;

            }
        }

        internal override void Apply(XmlNode parent, ref XmlNode currentPosition)
        {
            switch (this._matchNode.NodeType)
            {
                case XmlNodeType.Element:
                    {
                        XmlNode node2;
                        if (this._name == null)
                        {
                            this._name = ((XmlElement)this._matchNode).LocalName;
                        }
                        if (this._ns == null)
                        {
                            this._ns = ((XmlElement)this._matchNode).NamespaceURI;
                        }
                        if (this._prefix == null)
                        {
                            this._prefix = ((XmlElement)this._matchNode).Prefix;
                        }
                        XmlElement element = parent.OwnerDocument.CreateElement(this._prefix, this._name, this._ns);
                        XmlAttributeCollection attributes = this._matchNode.Attributes;
                        while (attributes.Count > 0)
                        {
                            XmlAttribute attribute = (XmlAttribute)attributes.Item(0);
                            attributes.RemoveAt(0);
                            element.Attributes.Append(attribute);
                        }
                        for (XmlNode node = this._matchNode.FirstChild; node != null; node = node2)
                        {
                            node2 = node.NextSibling;
                            this._matchNode.RemoveChild(node);
                            element.AppendChild(node);
                        }
                        parent.ReplaceChild(element, this._matchNode);
                        currentPosition = element;
                        base.ApplyChildren(element);
                        return;
                    }
                case XmlNodeType.Attribute:
                    {
                        if (this._name == null)
                        {
                            this._name = ((XmlAttribute)this._matchNode).LocalName;
                        }
                        if (this._ns == null)
                        {
                            this._ns = ((XmlAttribute)this._matchNode).NamespaceURI;
                        }
                        if (this._prefix == null)
                        {
                            this._prefix = ((XmlAttribute)this._matchNode).Prefix;
                        }
                        if (this._value == null)
                        {
                            this._value = ((XmlAttribute)this._matchNode).Value;
                        }
                        XmlAttribute attribute2 = parent.OwnerDocument.CreateAttribute(this._prefix, this._name, this._ns);
                        attribute2.Value = this._value;
                        parent.Attributes.Remove((XmlAttribute)this._matchNode);
                        parent.Attributes.Append(attribute2);
                        return;
                    }
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.Comment:
                    ((XmlCharacterData)this._matchNode).Data = this._value;
                    currentPosition = this._matchNode;
                    return;

                case XmlNodeType.EntityReference:
                    {
                        XmlEntityReference reference = parent.OwnerDocument.CreateEntityReference(this._name);
                        parent.ReplaceChild(reference, this._matchNode);
                        currentPosition = reference;
                        return;
                    }
                case XmlNodeType.Entity:
                case XmlNodeType.Document:
                case XmlNodeType.DocumentFragment:
                case XmlNodeType.Notation:
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                case XmlNodeType.EndElement:
                case XmlNodeType.EndEntity:
                    return;

                case XmlNodeType.ProcessingInstruction:
                    {
                        if (this._name == null)
                        {
                            ((XmlProcessingInstruction)this._matchNode).Data = this._value;
                            currentPosition = this._matchNode;
                            return;
                        }
                        if (this._value == null)
                        {
                            this._value = ((XmlProcessingInstruction)this._matchNode).Data;
                        }
                        XmlProcessingInstruction instruction = parent.OwnerDocument.CreateProcessingInstruction(this._name, this._value);
                        parent.ReplaceChild(instruction, this._matchNode);
                        currentPosition = instruction;
                        return;
                    }
                case XmlNodeType.DocumentType:
                    {
                        if (this._name == null)
                        {
                            this._name = ((XmlDocumentType)this._matchNode).LocalName;
                        }
                        if (this._ns == null)
                        {
                            this._ns = ((XmlDocumentType)this._matchNode).SystemId;
                        }
                        else if (this._ns == string.Empty)
                        {
                            this._ns = null;
                        }
                        if (this._prefix == null)
                        {
                            this._prefix = ((XmlDocumentType)this._matchNode).PublicId;
                        }
                        else if (this._prefix == string.Empty)
                        {
                            this._prefix = null;
                        }
                        if (this._value == null)
                        {
                            this._value = ((XmlDocumentType)this._matchNode).InternalSubset;
                        }
                        XmlDocumentType type = this._matchNode.OwnerDocument.CreateDocumentType(this._name, this._prefix, this._ns, this._value);
                        this._matchNode.ParentNode.ReplaceChild(type, this._matchNode);
                        return;
                    }
                case XmlNodeType.XmlDeclaration:
                    {
                        XmlDeclaration declaration = (XmlDeclaration)this._matchNode;
                        declaration.Encoding = null;
                        declaration.Standalone = null;
                        declaration.InnerText = this._value;
                        return;
                    }
            }
        }
    }
}
