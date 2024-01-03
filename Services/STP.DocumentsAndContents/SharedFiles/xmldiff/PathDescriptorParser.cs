namespace Microsoft.XmlDiffPatch
{
    using System;
    using System.Collections;
    using System.Xml;

    internal class PathDescriptorParser
    {
        private static char[] Delimiters = new char[] { '|', '-', '/' };
        private static char[] MultiNodesDelimiters = new char[] { '|', '-' };

        private static void OnInvalidExpression(string path)
        {
            Microsoft.XmlDiffPatch.XmlPatchError.Error("Invalid XDL diffgram. '{0}' is an invalid path descriptor.", path);
        }

        private static void OnNoMatchingNode(string path)
        {
            Microsoft.XmlDiffPatch.XmlPatchError.Error("Invalid XDL diffgram. No node matches the path descriptor '{0}'.", path);
        }

        private static string ReadAttrName(string str, ref int pos)
        {
            int index = str.IndexOf('|', pos);
            if (index < 0)
            {
                index = str.Length;
            }
            string str2 = str.Substring(pos, index - pos);
            pos = index;
            return str2;
        }

        private static int ReadPosition(string str, ref int pos)
        {
            int length = str.IndexOfAny(Delimiters, pos);
            if (length < 0)
            {
                length = str.Length;
            }
            int num2 = int.Parse(str.Substring(pos, length - pos));
            pos = length;
            return num2;
        }

        private static XmlNodeList SelectAbsoluteNodes(XmlNode rootNode, string path)
        {
            int pos = 1;
            XmlNode node = rootNode;
            while (true)
            {
                int startPos = pos;
                XmlNodeList list = node.ChildNodes;
                int num3 = ReadPosition(path, ref pos);
                if ((pos == path.Length) || (path[pos] == '/'))
                {
                    if (((list.Count == 0) || (num3 < 0)) || (num3 > list.Count))
                    {
                        OnNoMatchingNode(path);
                    }
                    node = list.Item(num3 - 1);
                    if (pos == path.Length)
                    {
                        Microsoft.XmlDiffPatch.XmlPatchNodeList list2 = new Microsoft.XmlDiffPatch.SingleNodeList();
                        list2.AddNode(node);
                        return list2;
                    }
                    pos++;
                }
                else
                {
                    if ((path[pos] == '-') || (path[pos] == '|'))
                    {
                        return SelectChildNodes(node, path, startPos);
                    }
                    OnInvalidExpression(path);
                }
            }
        }

        private static XmlNodeList SelectAllAttributes(XmlNode parentNode)
        {
            XmlAttributeCollection attributes = parentNode.Attributes;
            if (attributes.Count == 0)
            {
                OnNoMatchingNode("@*");
                return null;
            }
            if (attributes.Count == 1)
            {
                Microsoft.XmlDiffPatch.XmlPatchNodeList list = new Microsoft.XmlDiffPatch.SingleNodeList();
                list.AddNode(attributes.Item(0));
                return list;
            }
            IEnumerator enumerator = attributes.GetEnumerator();
            Microsoft.XmlDiffPatch.XmlPatchNodeList list2 = new Microsoft.XmlDiffPatch.MultiNodeList();
            while (enumerator.MoveNext())
            {
                list2.AddNode((XmlNode)enumerator.Current);
            }
            return list2;
        }

        private static XmlNodeList SelectAllChildren(XmlNode parentNode)
        {
            XmlNodeList list = parentNode.ChildNodes;
            if (list.Count == 0)
            {
                OnNoMatchingNode("*");
                return null;
            }
            if (list.Count == 1)
            {
                Microsoft.XmlDiffPatch.XmlPatchNodeList list2 = new Microsoft.XmlDiffPatch.SingleNodeList();
                list2.AddNode(list.Item(0));
                return list2;
            }
            IEnumerator enumerator = list.GetEnumerator();
            Microsoft.XmlDiffPatch.XmlPatchNodeList list3 = new Microsoft.XmlDiffPatch.MultiNodeList();
            while (enumerator.MoveNext())
            {
                list3.AddNode((XmlNode)enumerator.Current);
            }
            return list3;
        }

        private static XmlNodeList SelectAttributes(XmlNode parentNode, string path)
        {
            int pos = 1;
            XmlAttributeCollection attributes = parentNode.Attributes;
            Microsoft.XmlDiffPatch.XmlPatchNodeList list = null;
            while (true)
            {
                string str = ReadAttrName(path, ref pos);
                if (list == null)
                {
                    if (pos == path.Length)
                    {
                        list = new Microsoft.XmlDiffPatch.SingleNodeList();
                    }
                    else
                    {
                        list = new Microsoft.XmlDiffPatch.MultiNodeList();
                    }
                }
                XmlNode namedItem = attributes.GetNamedItem(str);
                if (namedItem == null)
                {
                    OnNoMatchingNode(path);
                }
                list.AddNode(namedItem);
                if (pos == path.Length)
                {
                    return list;
                }
                if (path[pos] == '|')
                {
                    pos++;
                    if (path[pos] != '@')
                    {
                        OnInvalidExpression(path);
                    }
                    pos++;
                }
                else
                {
                    OnInvalidExpression(path);
                }
            }
        }

        private static XmlNodeList SelectChildNodes(XmlNode parentNode, string path, int startPos)
        {
            int pos = startPos;
            Microsoft.XmlDiffPatch.XmlPatchNodeList list = null;
            XmlNodeList list2 = parentNode.ChildNodes;
            int num2 = ReadPosition(path, ref pos);
            if (pos == path.Length)
            {
                list = new Microsoft.XmlDiffPatch.SingleNodeList();
            }
            else
            {
                list = new Microsoft.XmlDiffPatch.MultiNodeList();
            }
            while (true)
            {
                if ((num2 <= 0) || (num2 > list2.Count))
                {
                    OnNoMatchingNode(path);
                }
                XmlNode node = list2.Item(num2 - 1);
                list.AddNode(node);
                if (pos == path.Length)
                {
                    return list;
                }
                if (path[pos] == '|')
                {
                    pos++;
                }
                else if (path[pos] == '-')
                {
                    pos++;
                    int num3 = ReadPosition(path, ref pos);
                    if ((num3 <= 0) || (num3 > list2.Count))
                    {
                        OnNoMatchingNode(path);
                    }
                    while (num2 < num3)
                    {
                        num2++;
                        node = node.NextSibling;
                        list.AddNode(node);
                    }
                    if (pos == path.Length)
                    {
                        return list;
                    }
                    if (path[pos] == '|')
                    {
                        pos++;
                    }
                    else
                    {
                        OnInvalidExpression(path);
                    }
                }
                num2 = ReadPosition(path, ref pos);
            }
        }

        internal static XmlNodeList SelectNodes(XmlNode rootNode, XmlNode currentParentNode, string pathDescriptor)
        {
            switch (pathDescriptor[0])
            {
                case '*':
                    if (pathDescriptor.Length == 1)
                    {
                        return SelectAllChildren(currentParentNode);
                    }
                    OnInvalidExpression(pathDescriptor);
                    return null;

                case '/':
                    return SelectAbsoluteNodes(rootNode, pathDescriptor);

                case '@':
                    if (pathDescriptor.Length < 2)
                    {
                        OnInvalidExpression(pathDescriptor);
                    }
                    if (pathDescriptor[1] == '*')
                    {
                        return SelectAllAttributes(currentParentNode);
                    }
                    return SelectAttributes(currentParentNode, pathDescriptor);
            }
            return SelectChildNodes(currentParentNode, pathDescriptor, 0);
        }
    }
}
