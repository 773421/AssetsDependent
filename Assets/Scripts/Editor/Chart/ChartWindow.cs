
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Chart
{
    public delegate Action<int> OnDoWindow();
    public class ChartWindow:EditorWindow
    {
        [MenuItem("Dependent/Chart")]
        static void OpenChartWindow() {
            var window = EditorWindow.GetWindow<ChartWindow>();
            window.Init();
            window.Show();
            Selection.selectionChanged = () =>
            {
                ChartDirecfor.Inst.CreateChart(Selection.gameObjects);
                window.Repaint();
            };
            
        }
        private void Init() {
            this.minSize = new Vector2(1024, 540);
            this.maxSize = new Vector2(2048, 1080);
        }
        private void OnGUI()
        {
            BeginWindows();
            var iter = ChartDirecfor.Inst.mNodeCharts.GetEnumerator();
            while (iter.MoveNext()) {
                var chart = iter.Current.Value;
                chart.mRect = GUI.Window(chart.id, chart.mRect, chart.DrawChart, chart.Name);
            }
            iter = ChartDirecfor.Inst.mNodeCharts.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.DrawCurve();
            }
            EndWindows();
        }
    }
}
