using UnityEngine;
using UnityEditor;

namespace Assets.Graph
{
    public class DependencyWindow : EditorWindow
    {
        public static DependencyWindow dw = null;
        public GraphDirector graphDirector = new GraphDirector();
        [MenuItem("Dependent/依赖关系窗口")]
        static void DependenceWindow()
        {
            DependencyWindow window = EditorWindow.GetWindow<DependencyWindow>();
            window.Show();
            dw = window;

            Selection.selectionChanged = () =>
            {
                window.graphDirector.CreateGraph(Selection.gameObjects);
                window.Repaint();
            };
        }
        void OnGUI()
        {
            graphDirector.DrawGraph();
        }
    }
}
