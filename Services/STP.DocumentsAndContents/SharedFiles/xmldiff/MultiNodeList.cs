namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Xml;

    internal class MultiNodeList : Microsoft.XmlDiffPatch.XmlPatchNodeList
    {
        internal ListChunk _chunks = null;
        private int _count = 0;
        private ListChunk _lastChunk = null;

        internal MultiNodeList()
        {
        }

        internal override void AddNode(XmlNode node)
        {
            if (this._lastChunk == null)
            {
                this._chunks = new ListChunk();
                this._lastChunk = this._chunks;
            }
            else if (this._lastChunk._count == 10)
            {
                this._lastChunk._next = new ListChunk();
                this._lastChunk = this._lastChunk._next;
            }
            this._lastChunk.AddNode(node);
            this._count++;
        }

        public override IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public override XmlNode Item(int index)
        {
            if (this._chunks == null)
            {
                return null;
            }
            if (index < 10)
            {
                return this._chunks[index];
            }
            int num = index / 10;
            ListChunk chunk = this._chunks;
            while (num > 0)
            {
                chunk = chunk._next;
                num--;
            }
            return chunk[index % 10];
        }

        public override int Count
        {
            get
            {
                return this._count;
            }
        }

        private class Enumerator : IEnumerator
        {
            private Microsoft.XmlDiffPatch.MultiNodeList.ListChunk _currentChunk;
            private int _currentChunkIndex = 0;
            private Microsoft.XmlDiffPatch.MultiNodeList _nodeList;

            internal Enumerator(Microsoft.XmlDiffPatch.MultiNodeList nodeList)
            {
                this._nodeList = nodeList;
                this._currentChunk = nodeList._chunks;
            }

            public bool MoveNext()
            {
                if (this._currentChunk == null)
                {
                    return false;
                }
                if (this._currentChunkIndex >= (this._currentChunk._count - 1))
                {
                    if (this._currentChunk._next == null)
                    {
                        return false;
                    }
                    this._currentChunk = this._currentChunk._next;
                    this._currentChunkIndex = 0;
                    return true;
                }
                this._currentChunkIndex++;
                return true;
            }

            public void Reset()
            {
                this._currentChunk = this._nodeList._chunks;
                this._currentChunkIndex = -1;
            }

            public object Current
            {
                get
                {
                    if (this._currentChunk == null)
                    {
                        return null;
                    }
                    return this._currentChunk[this._currentChunkIndex];
                }
            }
        }

        internal class ListChunk
        {
            internal int _count = 0;
            internal Microsoft.XmlDiffPatch.MultiNodeList.ListChunk _next = null;
            internal XmlNode[] _nodes = new XmlNode[10];
            internal const int ChunkSize = 10;

            internal void AddNode(XmlNode node)
            {
                this._nodes[this._count++] = node;
            }

            internal XmlNode this[int i]
            {
                get
                {
                    return this._nodes[i];
                }
            }
        }
    }
}
