using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace Assets.Graph
{
    public class GraphDirector:System.IDisposable
    {
        private List<AssetGraph> graphNodeList = new List<AssetGraph>();
        public void CreateGraph(Object[] objs) {
            graphNodeList.Clear();
            foreach (var gbo in objs)
            {
                var assetPath = AssetDatabase.GetAssetPath(gbo);
                graphNodeList.Add(new AssetGraph(assetPath));
            }

            var nCount = graphNodeList.Count;
            var winheight = 500f;
            var graphHeight = winheight / nCount;
            Rect rect = new Rect(0, 0, 800f, graphHeight);
            foreach (var graph in graphNodeList)
            {
                graph.BuildGraph(rect);
                rect.y = rect.height;
            }
        }

        public void Dispose()
        {
            graphNodeList.Clear();
        }

        public void DrawGraph()
        {
            foreach (IGraph graph in graphNodeList)
            {
                graph.DrawGraph();
            }
        }
    }
}
