namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal class Patch : Microsoft.XmlDiffPatch.XmlPatchParentOperation
    {
        internal XmlNode _sourceRootNode;

        internal Patch(XmlNode sourceRootNode)
        {
            this._sourceRootNode = sourceRootNode;
        }

        internal override void Apply(XmlNode parent, ref XmlNode currentPosition)
        {
            base.ApplyChildren(parent.OwnerDocument);
        }
    }
}
