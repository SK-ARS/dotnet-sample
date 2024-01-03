namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Collections;
    using System.Xml;

    internal class PatchAddXmlFragment : Microsoft.XmlDiffPatch.XmlPatchOperation
    {
        private XmlNodeList _nodes;

        internal PatchAddXmlFragment(XmlNodeList nodes)
        {
            this._nodes = nodes;
        }

        internal override void Apply(XmlNode parent, ref XmlNode currentPosition)
        {
            XmlDocument document = parent.OwnerDocument;
            IEnumerator enumerator = this._nodes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                XmlNode node = document.ImportNode((XmlNode)enumerator.Current, true);
                parent.InsertAfter(node, currentPosition);
                currentPosition = node;
            }
        }
    }
}
