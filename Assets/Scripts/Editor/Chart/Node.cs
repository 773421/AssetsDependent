using Assets.Dependent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Chart
{
    public class Node: System.IDisposable
    {
        public string assetPath;
        public Object mAsset;
        public Rect mRect;
        public string Name {
            get {
                if (null != mAsset) {
                    return mAsset.name;
                }
                return "";
            }
        }
        public Node(string assetPath)
        {
            this.assetPath = assetPath;
            this.Init();
        }
        private void Init() {
            if (string.IsNullOrEmpty(assetPath)) {
                return;
            }
            mAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);
        }
        public void SetRect(Rect rect) {
            this.mRect = rect;
        }
        public void Draw() {
            EditorGUI.ObjectField(mRect, mAsset, mAsset.GetType(), false);
        }
        public void Dispose()
        {
            mAsset = null;
        }
    }
}
