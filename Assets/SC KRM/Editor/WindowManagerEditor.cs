using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Window;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WindowManager), true)]
    public class WindowManagerEditor : Editor
    {
        bool repaint = false;

        void OnEnable()
        {
            if (Application.isPlaying)
            {
                repaint = true;
                Repainter();
            }
        }

        void OnDisable() => repaint = false;

        async void Repainter()
        {
            while (repaint)
            {
                Repaint();
                await Task.Delay(100);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("윈도우 핸들 - " + WindowManager.GetWindowHandle());

            EditorGUILayout.Space();

            Vector2 pos = WindowManager.GetWindowPos(Vector2.zero, Vector2.zero);
            Vector2 size = WindowManager.GetWindowSize();
            Vector2 size2 = WindowManager.GetClientSize();
            EditorGUILayout.LabelField("윈도우 위치 - " + pos.x + ", " + pos.y);
            EditorGUILayout.LabelField("윈도우 크기 - " + size.x + ", " + size.y);
            EditorGUILayout.LabelField("클라이언트 크기 - " + size2.x + ", " + size2.y);
        }
    }
}