using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Assets.Dependent;
using UnityEditor;

namespace Assets.Graph
{
    public class AssetGraph : IGraph
    {
        private static int idGen = 1;
        public int id { get; private set; } = idGen++;
        /// <summary>
        /// 打AssetBundle，需要去除父节点资源和子节点资源间接引用关系，以及循环依赖关系
        /// </summary>
        private AssetNode mAssetNode;
        /// <summary>
        /// 存放父节点和子节点
        /// </summary>
        private List<INode> nodeList = new List<INode>();
        const float nodeWidth = 100;
        const float nodeHeight = 20;
        const float vSpan = 30f;
        const float hSpan = 50f;
        public Vector2 mSize;
        public Rect mRect;
        public string Name { get; private set; }
        public AssetGraph(string assetPath) {

            this.mAssetNode = AssetNode.Get(assetPath);
        }

        public void BuildGraph(Vector2 pos)
        {
            if (null == mAssetNode) {
                return;
            }
            int nChild = mAssetNode.mQuoteNodes.Count;
            int nParent = mAssetNode.mQuotedNodes.Count;
            float height = (nParent > nChild ? nParent : nChild) *(vSpan + nodeHeight);
            float width = nodeWidth;
            if (nChild > 0 && nParent > 0)
            {
                width = nodeWidth * 3 + hSpan * 2;
            }
            else if (nChild > 0 || nParent > 0) {
                width = nodeWidth * 2 + hSpan * 1;
            }
            width += 100;
            mRect = new Rect(pos.x + hSpan * 2, pos.y +  vSpan * 2, width, height);


            var curNodeRect = new Rect((mRect.width - nodeWidth) * 0.5f, (mRect.height - nodeHeight) * 0.5f,nodeWidth, nodeHeight);
            var curNode = new Node(mAssetNode);
            curNode.BuildGraph(curNodeRect);
            this.Name = curNode.Name;

            nodeList.Clear();
            var parents = CreateParentNodes(curNodeRect);
            var childs = CreateChildNodes(curNodeRect);
            nodeList.Add(curNode);

            foreach (var p in parents) {
                nodeList.Add(p);
                nodeList.Add(new ParentNodeLinker(p, curNode));
            }
            foreach (var c in childs)
            {
                nodeList.Add(c);
                nodeList.Add(new ChildNodeLinker(curNode, c));
            }
        }
        /// <summary>
        /// 创建父节点图
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private List<Node> CreateParentNodes(Rect rect) {
            List<Node> nodeLiset = new List<Node>();
            foreach (var childNode in mAssetNode.mQuotedNodes)
            {
                var gNode = new Node(childNode);
                nodeLiset.Add(gNode);
            }
            int nCount = nodeLiset.Count;
            Rect parentRect = new Rect(rect.x - (hSpan + nodeWidth), rect.y + nodeHeight * 0.5f - (nodeHeight * nCount + vSpan * (nCount - 1)) * 0.5f, nodeWidth, nodeHeight);
            foreach (var node in nodeLiset)
            {
                node.BuildGraph(parentRect);
                parentRect.y += nodeHeight + vSpan;

            }
            return nodeLiset;
        }
        /// <summary>
        /// 创建子节点图
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private List<Node> CreateChildNodes(Rect rect) {
            List<Node> nodeLiset = new List<Node>();
            foreach (var childNode in mAssetNode.mQuoteNodes)
            {
                var gNode = new Node(childNode);
                nodeLiset.Add(gNode);
            }
            int nCount = nodeLiset.Count;
            Rect parentRect = new Rect(rect.x + (hSpan + nodeWidth), rect.y + nodeHeight * 0.5f - (nodeHeight * nCount + vSpan * (nCount - 1)) * 0.5f, nodeWidth, nodeHeight);
            foreach (var node in nodeLiset)
            {
                node.BuildGraph(parentRect);
                parentRect.y += nodeHeight + vSpan;
            }
            return nodeLiset;
        }
        public void DrawGraph()
        {
            if (nodeList.Count > 0)
            {
                foreach (var node in nodeList)
                {
                    node.DrawNode();
                }
            }
        }
        public void DoWnidow(int id) {
            this.DrawGraph();
            GUI.DragWindow();
        }
        public void Dispose()
        {
            foreach (INode node in nodeList) {
                node.Dispose();
            }
            nodeList.Clear();
        }
    }
}
