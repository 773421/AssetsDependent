using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Assets.Dependent;

namespace Assets.Graph
{
    public class Node : INode
    {
        public AssetNode node = null;
        protected Object mAsset = null;
        public Rect mRect { get; set; }
        public string Name {
            get {
                if (null != mAsset) {
                    return mAsset.name;
                }
                return "";
            }
        }
        public Node(AssetNode _node)
        {
            this.node = _node;
        }
        public void BuildGraph()
        {
            if (null != this.node)
            {
                mAsset = AssetDatabase.LoadMainAssetAtPath(this.node.mAssetPath);
            }
            else {
                Debug.LogWarning($"Node Is Null.");
            }
        }
        public void DrawNode()
        {
            if (null != mAsset)
            {
                EditorGUILayout.ObjectField(mAsset, mAsset.GetType(), false);
            }
            else {
                Debug.LogWarning($"{node.mAssetPath}[asset is null]");
            }
        }

        public void Dispose()
        {
            this.mAsset = null;
        }
    }
}
