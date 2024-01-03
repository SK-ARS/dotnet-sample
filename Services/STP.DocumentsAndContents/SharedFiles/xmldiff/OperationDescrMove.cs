namespace Microsoft.XmlDiffPatch
{
    using System;

    internal class OperationDescrMove : Microsoft.XmlDiffPatch.OperationDescriptor
    {
        internal OperationDescrMove(ulong opid) : base(opid)
        {
        }

        internal override string Type
        {
            get
            {
                return "move";
            }
        }
    }
}
