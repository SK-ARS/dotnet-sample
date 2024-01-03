namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal class PatchSetPosition : Microsoft.XmlDiffPatch.XmlPatchParentOperation
    {
        private XmlNode _matchNode;

        internal PatchSetPosition(XmlNode matchNode)
        {
            this._matchNode = matchNode;
        }

        internal override void Apply(XmlNode parent, ref XmlNode currentPosition)
        {
            if (this._matchNode.NodeType == XmlNodeType.Element)
            {
                base.ApplyChildren((XmlElement)this._matchNode);
            }
            currentPosition = this._matchNode;
        }
    }
}
