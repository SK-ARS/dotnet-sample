namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal class PatchAddNode : Microsoft.XmlDiffPatch.XmlPatchParentOperation
    {
        private bool _ignoreChildOrder;
        private string _name;
        private XmlNodeType _nodeType;
        private string _ns;
        private string _prefix;
        private string _value;

        internal PatchAddNode(XmlNodeType nodeType, string name, string ns, string prefix, string value, bool ignoreChildOrder)
        {
            this._nodeType = nodeType;
            this._name = name;
            this._ns = ns;
            this._prefix = prefix;
            this._value = value;
            this._ignoreChildOrder = ignoreChildOrder;
        }

        internal override void Apply(XmlNode parent, ref XmlNode currentPosition)
        {
            XmlNode node = null;
            switch (this._nodeType)
            {
                case XmlNodeType.Element:
                    node = parent.OwnerDocument.CreateElement(this._prefix, this._name, this._ns);
                    base.ApplyChildren(node);
                    break;

                case XmlNodeType.Text:
                    node = parent.OwnerDocument.CreateTextNode(this._value);
                    break;

                case XmlNodeType.CDATA:
                    node = parent.OwnerDocument.CreateCDataSection(this._value);
                    break;

                case XmlNodeType.EntityReference:
                    node = parent.OwnerDocument.CreateEntityReference(this._name);
                    break;

                case XmlNodeType.ProcessingInstruction:
                    node = parent.OwnerDocument.CreateProcessingInstruction(this._name, this._value);
                    break;

                case XmlNodeType.Comment:
                    node = parent.OwnerDocument.CreateComment(this._value);
                    break;

                case XmlNodeType.DocumentType:
                    {
                        XmlDocument document2 = parent.OwnerDocument;
                        if (this._prefix == string.Empty)
                        {
                            this._prefix = null;
                        }
                        if (this._ns == string.Empty)
                        {
                            this._ns = null;
                        }
                        XmlDocumentType type = document2.CreateDocumentType(this._name, this._prefix, this._ns, this._value);
                        if (document2.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                        {
                            document2.InsertAfter(type, document2.FirstChild);
                            return;
                        }
                        document2.InsertBefore(type, document2.FirstChild);
                        return;
                    }
                case XmlNodeType.XmlDeclaration:
                    {
                        XmlDocument document = parent.OwnerDocument;
                        XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
                        declaration.Value = this._value;
                        document.InsertBefore(declaration, document.FirstChild);
                        return;
                    }
                case XmlNodeType.Attribute:
                    if (this._prefix == "xmlns")
                    {
                        node = parent.OwnerDocument.CreateAttribute(this._prefix + ":" + this._name);
                    }
                    else if ((this._prefix == "") && (this._name == "xmlns"))
                    {
                        node = parent.OwnerDocument.CreateAttribute(this._name);
                    }
                    else
                    {
                        node = parent.OwnerDocument.CreateAttribute(this._prefix, this._name, this._ns);
                    }
                    ((XmlAttribute)node).Value = this._value;
                    parent.Attributes.Append((XmlAttribute)node);
                    return;
            }
            if (this._ignoreChildOrder)
            {
                parent.AppendChild(node);
            }
            else
            {
                parent.InsertAfter(node, currentPosition);
            }
            currentPosition = node;
        }
    }
}
