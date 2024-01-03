namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal abstract class XmlPatchOperation
    {
        internal Microsoft.XmlDiffPatch.XmlPatchOperation _nextOp;

        protected XmlPatchOperation()
        {
        }

        internal abstract void Apply(XmlNode parent, ref XmlNode currentPosition);
    }
}
