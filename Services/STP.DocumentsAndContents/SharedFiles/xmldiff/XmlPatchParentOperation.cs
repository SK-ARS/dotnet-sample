namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    internal abstract class XmlPatchParentOperation : Microsoft.XmlDiffPatch.XmlPatchOperation
    {
        internal Microsoft.XmlDiffPatch.XmlPatchOperation _firstChild;

        protected XmlPatchParentOperation()
        {
        }

        protected void ApplyChildren(XmlNode parent)
        {
            XmlNode currentPosition = null;
            for (Microsoft.XmlDiffPatch.XmlPatchOperation operation = this._firstChild; operation != null; operation = operation._nextOp)
            {
                operation.Apply(parent, ref currentPosition);
            }
        }

        internal void InsertChildAfter(Microsoft.XmlDiffPatch.XmlPatchOperation child, Microsoft.XmlDiffPatch.XmlPatchOperation newChild)
        {
            if (child == null)
            {
                newChild._nextOp = this._firstChild;
                this._firstChild = newChild;
            }
            else
            {
                newChild._nextOp = child._nextOp;
                child._nextOp = newChild;
            }
        }
    }
}
