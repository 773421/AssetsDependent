using Assets.Dependent;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Chart
{
    public class NodeChart
    {
        static public int idGen = 1;
        public int id { private set; get; }
        /// <summary>
        /// 当前节点
        /// </summary>
        protected Node mNode;
        public Rect mRect;
        protected List<Node> mNodeList = new List<Node>();
        /// <summary>
        /// 节点父类图
        /// </summary>
        protected List<NodeChart> mParentChart = new List<NodeChart>();
        protected AssetNode mAssetNode;
        public string Name {
            get {
                return mNode.Name;
            }
        }
        public NodeChart(AssetNode node) {
            mAssetNode = node;
            id = idGen++;
        }
        public void Init(Rect rect)
        {
            mRect = rect;
            mNode = new Node(mAssetNode.mAssetPath);

            foreach (var anode in mAssetNode.mQuoteNodes) {
                var cnode = new Node(anode.mAssetPath);
                mNodeList.Add(cnode);
            }

            //获取父节点图
            foreach (var pnode in mAssetNode.mQuotedNodes) {
                var chart = ChartDirecfor.GetOrCreateNodeChart(pnode);
                mParentChart.Add(chart);
            }
        }
        public void SetRect(Rect rect) {
            mRect = rect;
        }
        Vector2 scrollPos;
        /// <summary>
        /// 绘制节点图
        /// </summary>
        /// <param name="id"></param>
        public void DrawChart() {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(mRect.width), GUILayout.Height(mRect.height));
            mNode.Draw();
            EditorGUILayout.LabelField("children:");
            foreach (var node in mNodeList) {
                node.Draw();
            }
            EditorGUILayout.EndScrollView();
        }
        public void DoWindow(int id) {
            this.DrawChart();
            GUI.DragWindow();
        }
        /// <summary>
        /// 绘制节点图关联曲线
        /// </summary>
        public void DrawCurve() {
            //绘制父节点与当前节点关系图
            foreach (var chart in mParentChart)
            {
                NodeUtil.DrawNodeCurve(chart.mRect, mRect, Color.black);
            }
        }
    }
}
