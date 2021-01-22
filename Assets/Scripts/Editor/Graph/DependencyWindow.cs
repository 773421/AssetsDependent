using UnityEngine;
using UnityEditor;

namespace Assets.Graph
{
    public class DependencyWindow : EditorWindow
    {
        public GraphDirector graphDirector = new GraphDirector();
        [MenuItem("Tools/Dependent/依赖关系图  #1")]
        static void DependenceWindow()
        {
            DependencyWindow window = EditorWindow.GetWindow<DependencyWindow>();
            window.minSize = new Vector2(1024, 540);
            window.maxSize = new Vector2(2048, 1080);
            window.Show();
            Selection.selectionChanged -= window.SelectionChanged;
            Selection.selectionChanged += window.SelectionChanged;
        }
        private void SelectionChanged()
        {
            this.graphDirector.CreateGraph(Selection.objects);
            this.Repaint();
        }

        void OnGUI()
        {
            BeginWindows();
            foreach (var graph in graphDirector.mGraphNodeList) {
                graph.mRect = GUI.Window(graph.id, graph.mRect, graph.DrawGraph, graph.Name);
                if (graph.ChildCount > 0)
                {
                    graph.mChildRect = GUI.Window(graph.id * 1000 + 1, graph.mChildRect, graph.DrawChilds, "children");
                }
                if (graph.parentCount > 0)
                {
                    graph.mParentRect = GUI.Window(graph.id * 1000 + 2, graph.mParentRect, graph.DrawParent, "parents");
                }
                graph.DrawLine();
            }
            EndWindows();
        }
    }
}
