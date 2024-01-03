namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal abstract class OperationDescriptor
    {
        internal Microsoft.XmlDiffPatch.OperationDescriptor _nextDescriptor;
        protected ulong _operationID;

        internal OperationDescriptor(ulong opid)
        {
            this._operationID = opid;
        }

        internal virtual void WriteTo(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("xd", "descriptor", "http://schemas.microsoft.com/xmltools/2002/xmldiff");
            xmlWriter.WriteAttributeString("opid", this._operationID.ToString());
            xmlWriter.WriteAttributeString("type", this.Type);
            xmlWriter.WriteEndElement();
        }

        internal abstract string Type { get; }
    }
}
