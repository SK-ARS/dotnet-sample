namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal abstract class XmlPatchNodeList : XmlNodeList
    {
        protected XmlPatchNodeList()
        {
        }

        internal abstract void AddNode(XmlNode node);
    }
}
