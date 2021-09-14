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
            EditorGUILayout.LabelField("��Ÿ Ÿ�� - " + Kernel.DeltaTime);
            EditorGUILayout.LabelField("FPS ��Ÿ Ÿ�� - " + Kernel.FPSDeltaTime);
            EditorGUILayout.LabelField("������ ���� ���� ��Ÿ Ÿ�� - " + Kernel.UnscaledDeltaTime);
            EditorGUILayout.LabelField("������ ���� ���� FPS ��Ÿ Ÿ�� - " + Kernel.FPSUnscaledDeltaTime);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("FPS - " + Kernel.FPS);
        }
    }
}