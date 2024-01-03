namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Xml;

    public class XmlPatchNew
    {
        private bool _ignoreChildOrder;
        private XmlNode _sourceRootNode;

        private Microsoft.XmlDiffPatch.Patch CreatePatch(XmlNode sourceNode, XmlElement diffgramElement)
        {
            Microsoft.XmlDiffPatch.Patch patchParent = new Microsoft.XmlDiffPatch.Patch(sourceNode);
            this._sourceRootNode = sourceNode;
            this.CreatePatchForChildren(sourceNode, diffgramElement, patchParent);
            return patchParent;
        }

        private void CreatePatchForChildren(XmlNode sourceParent, XmlElement diffgramParent, Microsoft.XmlDiffPatch.XmlPatchParentOperation patchParent)
        {
            Microsoft.XmlDiffPatch.XmlPatchOperation child = null;
            XmlNode node = diffgramParent.FirstChild;
            while (node != null)
            {
                if (node.NodeType != XmlNodeType.Element)
                {
                    node = node.NextSibling;
                    continue;
                }
                XmlElement element = (XmlElement)node;
                XmlNodeList matchNodes = null;
                string attribute = element.GetAttribute("match");
                if (attribute != string.Empty)
                {
                    matchNodes = Microsoft.XmlDiffPatch.PathDescriptorParser.SelectNodes(this._sourceRootNode, sourceParent, attribute);
                    if (matchNodes.Count == 0)
                    {
                        Microsoft.XmlDiffPatch.XmlPatchError.Error("Invalid XDL diffgram. No node matches the path descriptor '{0}'.", attribute);
                    }
                }
                Microsoft.XmlDiffPatch.XmlPatchOperation newChild = null;
                switch (element.LocalName)
                {
                    case "node":
                        {
                            if (matchNodes.Count != 1)
                            {
                                Microsoft.XmlDiffPatch.XmlPatchError.Error("Invalid XDL diffgram; more than one node matches the '{0}' path descriptor on the xd:node or xd:change element.", attribute);
                            }
                            XmlNode matchNode = matchNodes.Item(0);
                            if ((this._sourceRootNode.NodeType != XmlNodeType.Document) || ((matchNode.NodeType != XmlNodeType.XmlDeclaration) && (matchNode.NodeType != XmlNodeType.DocumentType)))
                            {
                                newChild = new Microsoft.XmlDiffPatch.PatchSetPosition(matchNode);
                                this.CreatePatchForChildren(matchNode, element, (Microsoft.XmlDiffPatch.XmlPatchParentOperation)newChild);
                            }
                            break;
                        }
                    case "add":
                        if (attribute != string.Empty)
                        {
                            bool bSubtree = element.GetAttribute("subtree") != "no";
                            newChild = new Microsoft.XmlDiffPatch.PatchCopy(matchNodes, bSubtree);
                            if (!bSubtree)
                            {
                                this.CreatePatchForChildren(sourceParent, element, (Microsoft.XmlDiffPatch.XmlPatchParentOperation)newChild);
                            }
                        }
                        else
                        {
                            string s = element.GetAttribute("type");
                            if (s != string.Empty)
                            {
                                XmlNodeType nodeType = (XmlNodeType)int.Parse(s);
                                bool flag2 = nodeType == XmlNodeType.Element;
                                if (nodeType != XmlNodeType.DocumentType)
                                {
                                    newChild = new Microsoft.XmlDiffPatch.PatchAddNode(nodeType, element.GetAttribute("name"), element.GetAttribute("ns"), element.GetAttribute("prefix"), flag2 ? string.Empty : element.InnerText, this._ignoreChildOrder);
                                    if (flag2)
                                    {
                                        this.CreatePatchForChildren(sourceParent, element, (Microsoft.XmlDiffPatch.XmlPatchParentOperation)newChild);
                                    }
                                }
                                else
                                {
                                    newChild = new Microsoft.XmlDiffPatch.PatchAddNode(nodeType, element.GetAttribute("name"), element.GetAttribute("systemId"), element.GetAttribute("publicId"), element.InnerText, this._ignoreChildOrder);
                                }
                            }
                            else
                            {
                                newChild = new Microsoft.XmlDiffPatch.PatchAddXmlFragment(element.ChildNodes);
                            }
                        }
                        break;

                    case "remove":
                        {
                            bool flag3 = element.GetAttribute("subtree") != "no";
                            newChild = new Microsoft.XmlDiffPatch.PatchRemove(matchNodes, flag3);
                            if (!flag3)
                            {
                                this.CreatePatchForChildren(matchNodes.Item(0), element, (Microsoft.XmlDiffPatch.XmlPatchParentOperation)newChild);
                            }
                            break;
                        }
                    case "change":
                        {
                            if (matchNodes.Count != 1)
                            {
                                Microsoft.XmlDiffPatch.XmlPatchError.Error("Invalid XDL diffgram; more than one node matches the '{0}' path descriptor on the xd:node or xd:change element.", attribute);
                            }
                            XmlNode node3 = matchNodes.Item(0);
                            if (node3.NodeType != XmlNodeType.DocumentType)
                            {
                                newChild = new Microsoft.XmlDiffPatch.PatchChange(node3, element.HasAttribute("name") ? element.GetAttribute("name") : null, element.HasAttribute("ns") ? element.GetAttribute("ns") : null, element.HasAttribute("prefix") ? element.GetAttribute("prefix") : null, (node3.NodeType == XmlNodeType.Element) ? null : element);
                            }
                            else
                            {
                                newChild = new Microsoft.XmlDiffPatch.PatchChange(node3, element.HasAttribute("name") ? element.GetAttribute("name") : null, element.HasAttribute("systemId") ? element.GetAttribute("systemId") : null, element.HasAttribute("publicId") ? element.GetAttribute("publicId") : null, element.IsEmpty ? null : element);
                            }
                            if (node3.NodeType == XmlNodeType.Element)
                            {
                                this.CreatePatchForChildren(node3, element, (Microsoft.XmlDiffPatch.XmlPatchParentOperation)newChild);
                            }
                            break;
                        }
                    case "descriptor":
                        return;
                }
                if (newChild != null)
                {
                    //code by chirag
                    //
                    patchParent.InsertChildAfter(child, newChild);
                    child = newChild;
                }
                node = node.NextSibling;
            }
        }

        private void Patch(ref XmlNode sourceNode, XmlDocument diffDoc)
        {
            XmlElement diffgramElement = diffDoc.DocumentElement;
            XmlNamedNodeMap map = diffgramElement.Attributes;
            XmlAttribute namedItem = (XmlAttribute)map.GetNamedItem("srcDocHash");
            Microsoft.XmlDiffPatch.XmlDiffOptions none = Microsoft.XmlDiffPatch.XmlDiffOptions.None;
            this._ignoreChildOrder = (none & Microsoft.XmlDiffPatch.XmlDiffOptions.IgnoreChildOrder) != Microsoft.XmlDiffPatch.XmlDiffOptions.None;
            if (sourceNode.NodeType == XmlNodeType.Document)
            {
                Microsoft.XmlDiffPatch.Patch patch = this.CreatePatch(sourceNode, diffgramElement);
                XmlDocument document = (XmlDocument)sourceNode;
                XmlElement parent = document.CreateElement("tempRoot");
                XmlNode node = document.FirstChild;
                while (node != null)
                {
                    XmlNode node2 = node.NextSibling;
                    if ((node.NodeType != XmlNodeType.XmlDeclaration) && (node.NodeType != XmlNodeType.DocumentType))
                    {
                        document.RemoveChild(node);
                        parent.AppendChild(node);
                    }
                    node = node2;
                }
                document.AppendChild(parent);
                XmlNode currentPosition = null;
                patch.Apply(parent, ref currentPosition);
                if (sourceNode.NodeType == XmlNodeType.Document)
                {
                    document.RemoveChild(parent);
                    while ((node = parent.FirstChild) != null)
                    {
                        parent.RemoveChild(node);
                        document.AppendChild(node);
                    }
                }
            }
            else if (sourceNode.NodeType == XmlNodeType.DocumentFragment)
            {
                Microsoft.XmlDiffPatch.Patch patch2 = this.CreatePatch(sourceNode, diffgramElement);
                XmlNode node4 = null;
                patch2.Apply(sourceNode, ref node4);
            }
            else
            {
                XmlDocumentFragment fragment = sourceNode.OwnerDocument.CreateDocumentFragment();
                XmlNode node5 = sourceNode.ParentNode;
                XmlNode node6 = sourceNode.PreviousSibling;
                if (node5 != null)
                {
                    node5.RemoveChild(sourceNode);
                }
                if (sourceNode.NodeType != XmlNodeType.XmlDeclaration)
                {
                    fragment.AppendChild(sourceNode);
                }
                else
                {
                    fragment.InnerXml = sourceNode.OuterXml;
                }
                Microsoft.XmlDiffPatch.Patch patch3 = this.CreatePatch(fragment, diffgramElement);
                XmlNode node7 = null;
                patch3.Apply(fragment, ref node7);
                XmlNodeList list = fragment.ChildNodes;
                if (list.Count != 1)
                {
                    Microsoft.XmlDiffPatch.XmlPatchError.Error("Internal Error. {0} nodes left after patch, expecting 1.", list.Count.ToString());
                }
                sourceNode = list.Item(0);
                fragment.RemoveAll();
                if (node5 != null)
                {
                    node5.InsertAfter(sourceNode, node6);
                }
            }
        }

        public void Patch(XmlDocument sourceDoc, XmlReader diffgram)
        {
            if (sourceDoc == null)
            {
                throw new ArgumentNullException("sourceDoc");
            }
            if (diffgram == null)
            {
                throw new ArgumentNullException("diffgram");
            }
            XmlNode sourceNode = sourceDoc;
            this.Patch(ref sourceNode, diffgram);
        }

        public void Patch(ref XmlNode sourceNode, XmlReader diffgram)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException("sourceNode");
            }
            if (diffgram == null)
            {
                throw new ArgumentNullException("diffgram");
            }
            XmlDocument diffDoc = new XmlDocument();
            diffDoc.Load(diffgram);
            this.Patch(ref sourceNode, diffDoc);
        }
    }
}
