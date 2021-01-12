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
        protected Vector3 lineStart;
        protected Vector3 lineEnd;
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
            Rect cRect = new Rect(0, 2, mRect.width, 20);
            mNode = new Node(mAssetNode.mAssetPath);
            mNode.SetRect(cRect);


            lineStart = new Vector3(cRect.x, cRect.y + cRect.height + 1);
            lineEnd = new Vector3(cRect.x + cRect.width, cRect.y + cRect.height + 1);

            cRect.y += 3;
            foreach (var anode in mAssetNode.mQuoteNodes) {
                cRect.y += cRect.height;
                var cnode = new Node(anode.mAssetPath);
                cnode.SetRect(cRect);
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
        /// <summary>
        /// 绘制节点图
        /// </summary>
        /// <param name="id"></param>
        public void DrawChart(int id) {
            GUI.DragWindow();
            mNode.Draw();
            Handles.DrawLine(lineStart, lineEnd);
            foreach (var node in mNodeList) {
                node.Draw();
            }
            
        }
        /// <summary>
        /// 绘制节点图关联曲线
        /// </summary>
        public void DrawCurve() {
            //绘制父节点与当前节点关系图
            foreach (var chart in mParentChart)
            {
                DrawNodeCurve(chart.mRect, mRect);
            }
        }
        static void DrawNodeCurve(Rect start, Rect end)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }
    }
}
