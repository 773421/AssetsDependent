using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Assets.Dependent;

namespace Assets.Graph
{
    public class AssetGraph : IGraph
    {
        /// <summary>
        /// 打AssetBundle，需要去除父节点资源和子节点资源间接引用关系，以及循环依赖关系
        /// </summary>
        private AssetNode mAssetNode;
        private List<INode> mGraphNodeList = new List<INode>();
        const float nodeWidth = 100;
        const float nodeHeight = 20;
        const float vSpan = 30f;
        const float hSpan = 50f;
        public Vector2 mSize;
        public Rect mRect;
        public string Name {
            get {
                return "";
            }
        }
        public AssetGraph(string assetPath) {

            this.mAssetNode = AssetNode.Get(assetPath);
        }

        public void BuildGraph(Rect _rect)
        {
            if (null == mAssetNode) {
                return;
            }
            mRect = _rect;
            var curNodeRect = new Rect((_rect.x + _rect.width - nodeHeight) * .5f, _rect.y +  (_rect.height - nodeHeight) * 0.5f, nodeWidth, nodeHeight);
            var curNode = new Node(mAssetNode);
            curNode.BuildGraph(curNodeRect);

            mGraphNodeList.Clear();
            var parents = CreateParentNodes(curNodeRect);
            var childs = CreateChildNodes(curNodeRect);
            mGraphNodeList.Add(curNode);
            foreach (var p in parents) {
                mGraphNodeList.Add(p);
                mGraphNodeList.Add(new ParentNodeLinker(p, curNode));
            }
            foreach (var c in childs)
            {
                mGraphNodeList.Add(c);
                mGraphNodeList.Add(new ChildNodeLinker(curNode, c));
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
            if (mGraphNodeList.Count > 0)
            {
                int id = 0;
                foreach (var node in mGraphNodeList)
                {
                    node.DrawNode();
                }
            }
        }

        public void Dispose()
        {
            foreach (INode node in mGraphNodeList) {
                node.Dispose();
            }
            mGraphNodeList.Clear();
        }
    }
}
