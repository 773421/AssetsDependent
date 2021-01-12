using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Assets.Dependent;
using UnityEditor;

namespace Assets.Graph
{
    public class AssetGraph
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
        private List<INode> parentList = new List<INode>();
        private List<INode> childList = new List<INode>();
        const float nodeWidth = 100;
        const float nodeHeight = 20;
        const float vSpan = 30f;
        const float hSpan = 50f;
        public Vector2 mSize;
        public Rect mRect;
        public Rect mParentRect;
        public Rect mChildRect;
        public Node mCurNode;
        public string Name { get;private set; }
        public AssetGraph(string assetPath) {

            this.mAssetNode = AssetNode.Get(assetPath);
        }

        public void BuildGraph(Vector2 pos)
        {
            if (null == mAssetNode) {
                return;
            }
            mCurNode = new Node(mAssetNode);
            mCurNode.BuildGraph();
            this.Name = mCurNode.Name;

            parentList.Clear();
            childList.Clear();
            var parents = CreateParentNodes(mAssetNode.mQuotedNodes);
            var childs = CreateParentNodes(mAssetNode.mQuoteNodes);
            parentList.AddRange(parents);
            childList.AddRange(childs);
            float width = 100f;
            float height = 50;
            mRect = new Rect(pos.x + hSpan * 2, pos.y + vSpan * 2, width, height);

            mParentRect = new Rect(mRect.x - mRect.width - hSpan * 2, mRect.y, width, height * 3);
            mChildRect = new Rect(mRect.x + mRect.width + hSpan * 2, mRect.y, width, height * 3);
        }
        /// <summary>
        /// 创建父节点图
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private List<Node> CreateParentNodes(List<AssetNode> nodes) {
            List<Node> nodeLiset = new List<Node>();
            foreach (var childNode in nodes)
            {
                var gNode = new Node(childNode);
                nodeLiset.Add(gNode);
            }
            foreach (var node in nodeLiset)
            {
                node.BuildGraph();

            }
            return nodeLiset;
        }
       
        private Vector2 scrollPos;
        private Vector2 mScrollChilds;
        private Vector2 mScrollParents;
        public void DrawGraph(int id)
        {
            if (null != mCurNode)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(mRect.width), GUILayout.Height(mRect.height));
                mCurNode.DrawNode();
                EditorGUILayout.EndScrollView();
                GUI.DragWindow();
            }
        }
        public void DrawChilds(int id)
        {
            mScrollChilds = EditorGUILayout.BeginScrollView(mScrollChilds, GUILayout.Width(mChildRect.width), GUILayout.Height(mChildRect.height));
            foreach (var node in childList)
            {
                node.DrawNode();
            }
            EditorGUILayout.EndScrollView();
            GUI.DragWindow();
        }
        public void DrawParent(int id)
        {
            mScrollParents = EditorGUILayout.BeginScrollView(mScrollParents, GUILayout.Width(mParentRect.width), GUILayout.Height(mParentRect.height));
            foreach (var node in parentList)
            {
                node.DrawNode();
            }
            EditorGUILayout.EndScrollView();
            GUI.DragWindow();
        }
        public void DrawLine() {
            NodeUtil.DrawNodeCurve(mParentRect, mRect, Color.blue);
            NodeUtil.DrawNodeCurve(mRect, mChildRect, Color.green);
        }
        public void Dispose()
        {
            foreach (INode node in parentList) {
                node.Dispose();
            }
            parentList.Clear();
        }
    }
}
