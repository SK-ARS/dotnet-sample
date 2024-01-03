namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Collections;
    using System.Xml;

    internal class PatchCopy : Microsoft.XmlDiffPatch.XmlPatchParentOperation
    {
        private bool _bSubtree;
        private XmlNodeList _matchNodes;

        internal PatchCopy(XmlNodeList matchNodes, bool bSubtree)
        {
            this._matchNodes = matchNodes;
            this._bSubtree = bSubtree;
        }

        internal override void Apply(XmlNode parent, ref XmlNode currentPosition)
        {
            IEnumerator enumerator = this._matchNodes.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                XmlNode node2;
                XmlNode current = (XmlNode) enumerator.Current;
                if (this._bSubtree)
                {
                    node2 = current.Clone();
                }
                else
                {
                    node2 = current.CloneNode(false);
                    base.ApplyChildren(node2);
                }
                parent.InsertAfter(node2, currentPosition);
                currentPosition = node2;
            }
        }
    }
}
