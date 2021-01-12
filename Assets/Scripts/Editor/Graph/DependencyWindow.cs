using UnityEngine;
using UnityEditor;

namespace Assets.Graph
{
    public class DependencyWindow : EditorWindow
    {
        public GraphDirector graphDirector = new GraphDirector();
        [MenuItem("Dependent/依赖关系图")]
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
            //graphDirector.DrawGraph();
            BeginWindows();
            foreach (var graph in graphDirector.mGraphNodeList) {
                graph.mRect = GUI.Window(graph.id, graph.mRect, graph.DoWnidow, graph.Name);
            }
            EndWindows();
        }
    }
}
