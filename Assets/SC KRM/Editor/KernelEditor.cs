using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Kernel), true)]
    public class KernelEditor : Editor
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
            EditorGUILayout.LabelField("델타 타임 - " + Kernel.DeltaTime);
            EditorGUILayout.LabelField("FPS 델타 타임 - " + Kernel.FPSDeltaTime);
            EditorGUILayout.LabelField("스케일 되지 않은 델타 타임 - " + Kernel.UnscaledDeltaTime);
            EditorGUILayout.LabelField("스케일 되지 않은 FPS 델타 타임 - " + Kernel.FPSUnscaledDeltaTime);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("FPS - " + Kernel.FPS);
        }
    }
}