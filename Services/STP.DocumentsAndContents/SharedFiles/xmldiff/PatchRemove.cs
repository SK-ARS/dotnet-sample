namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Collections;
    using System.Xml;

    internal class PatchRemove : Microsoft.XmlDiffPatch.XmlPatchParentOperation
    {
        private bool _bSubtree;
        private XmlNodeList _sourceNodes;

        internal PatchRemove(XmlNodeList sourceNodes, bool bSubtree)
        {
            this._sourceNodes = sourceNodes;
            this._bSubtree = bSubtree;
        }

        internal override void Apply(XmlNode parent, ref XmlNode currentPosition)
        {
            if (!this._bSubtree)
            {
                XmlNode node = this._sourceNodes.Item(0);
                base.ApplyChildren(node);
                currentPosition = node.PreviousSibling;
            }
            IEnumerator enumerator = this._sourceNodes.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                XmlNode current = (XmlNode)enumerator.Current;
                if (current.NodeType == XmlNodeType.Attribute)
                {
                    ((XmlElement)parent).RemoveAttributeNode((XmlAttribute)current);
                }
                else
                {
                    if (!this._bSubtree)
                    {
                        while (current.FirstChild != null)
                        {
                            XmlNode node3 = current.FirstChild;
                            current.RemoveChild(node3);
                            parent.InsertAfter(node3, currentPosition);
                            currentPosition = node3;
                        }
                    }
                    current.ParentNode.RemoveChild(current);
                }
            }
        }
    }
}
