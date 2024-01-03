namespace Microsoft.XmlDiffPatch
{
    using System;

    internal enum XmlDiffNodeType
    {
        Attribute = 2,
        CDATA = 4,
        Comment = 8,
        Document = 9,
        DocumentType = -1,
        Element = 1,
        EntityReference = 5,
        Namespace = 100,
        None = 0,
        ProcessingInstruction = 7,
        ShrankNode = 0x65,
        SignificantWhitespace = 14,
        Text = 3,
        XmlDeclaration = -2
    }
}
