namespace Microsoft.XmlDiffPatch
{
    using System;

    public enum XmlDiffOptions
    {
        IgnoreChildOrder = 1,
        IgnoreComments = 2,
        IgnoreDtd = 0x80,
        IgnoreNamespaces = 0x10,
        IgnorePI = 4,
        IgnorePrefixes = 0x20,
        IgnoreWhitespace = 8,
        IgnoreXmlDecl = 0x40,
        None = 0
    }
}
