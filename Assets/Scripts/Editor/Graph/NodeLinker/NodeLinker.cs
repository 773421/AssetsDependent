using UnityEditor;
using UnityEngine;
namespace Assets.Graph
{
    public class NodeLinker : INode
    {
        private Vector3 startPos;
        private Vector3 endPos;
        virtual protected Color color { get; set; } = Color.white;
        protected Color shadowCol = new Color(0, 0, 0, 0.06f);

        public NodeLinker(Rect leftRect, Rect _rightRect)
        {
            startPos.x = leftRect.x + leftRect.width;
            startPos.y = leftRect.y + leftRect.height * .5f;

            endPos.x = _rightRect.x;
            endPos.x = _rightRect.y + _rightRect.height * .5f;
        }
        public NodeLinker(Node from, Node to) {
            startPos.x = from.mRect.x + from.mRect.width;
            startPos.y = from.mRect.y + from.mRect.height * 0.5f;

            endPos.x = to.mRect.x;
            endPos.y = to.mRect.y + to.mRect.height * 0.5f;
        }

        public void Dispose()
        {
        }

        public void DrawNode()
        {
            //Vector3 startTan = startPos + Vector3.right * 50;
            //Vector3 endTan = endPos + Vector3.left * 50;
            //for (int i = 0; i < 3; i++) // Draw a shadow
            //    Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            //Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 1);
            var col = Handles.color;
            Handles.color = color;
            Handles.DrawLine(startPos, endPos);
            Handles.color = col;
        }
    }
}
