namespace Microsoft.XmlDiffPatch
{
    using System;

    internal class HashAlgorithm
    {
        private ulong _hash;

        internal HashAlgorithm()
        {
        }

        internal void AddInt(int i)
        {
            this._hash += (this._hash << 11) + (ulong)i;
        }

        internal void AddString(string data)
        {
            this._hash = GetHash(data, this._hash);
        }

        internal void AddULong(ulong u)
        {
            this._hash += (this._hash << 11) + u;
        }

        internal static ulong GetHash(string data)
        {
            return GetHash(data, 0L);
        }

        private static ulong GetHash(string data, ulong hash)
        {
            hash += (hash << 13) + (ulong)data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                hash += (hash << 0x11) + data[i];
            }
            return hash;
        }

        internal ulong Hash
        {
            get
            {
                return this._hash;
            }
        }
    }
}
