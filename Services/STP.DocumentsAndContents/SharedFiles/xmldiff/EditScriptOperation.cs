namespace Microsoft.XmlDiffPatch
{
    using System;

    internal enum EditScriptOperation
    {
        None,
        Match,
        Add,
        Remove,
        ChangeNode,
        EditScriptReference,
        EditScriptPostponed,
        OpenedAdd,
        OpenedRemove,
        OpenedMatch
    }
}
