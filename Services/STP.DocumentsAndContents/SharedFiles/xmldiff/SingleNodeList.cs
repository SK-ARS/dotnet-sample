namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Collections;
    using System.Xml;

    internal class SingleNodeList : Microsoft.XmlDiffPatch.XmlPatchNodeList
    {
        private XmlNode _node;

        internal SingleNodeList()
        {
        }

        internal override void AddNode(XmlNode node)
        {
            if (this._node != null)
            {
                Microsoft.XmlDiffPatch.XmlPatchError.Error("Internal Error. XmlDiffPathSingleNodeList can contain one node only.");
            }
            this._node = node;
        }

        public override IEnumerator GetEnumerator()
        {
            return new Enumerator(this._node);
        }

        public override XmlNode Item(int index)
        {
            if (index == 0)
            {
                return this._node;
            }
            return null;
        }

        public override int Count
        {
            get
            {
                return 1;
            }
        }

        private class Enumerator : IEnumerator
        {
            private XmlNode _node;
            private State _state = State.BeforeNode;

            internal Enumerator(XmlNode node)
            {
                this._node = node;
            }

            public bool MoveNext()
            {
                switch (this._state)
                {
                    case State.BeforeNode:
                        this._state = State.OnNode;
                        return true;

                    case State.OnNode:
                        this._state = State.AfterNode;
                        return false;

                    case State.AfterNode:
                        return false;
                }
                return false;
            }

            public void Reset()
            {
                this._state = State.BeforeNode;
            }

            public object Current
            {
                get
                {
                    if (this._state != State.OnNode)
                    {
                        return null;
                    }
                    return this._node;
                }
            }

            private enum State
            {
                BeforeNode,
                OnNode,
                AfterNode
            }
        }
    }
}
