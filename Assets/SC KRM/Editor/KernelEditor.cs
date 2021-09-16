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
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("��Ÿ Ÿ�� - " + Kernel.deltaTime);
                EditorGUILayout.LabelField("FPS ��Ÿ Ÿ�� - " + Kernel.fpsDeltaTime);
                EditorGUILayout.LabelField("������ ���� ���� ��Ÿ Ÿ�� - " + Kernel.unscaledDeltaTime);
                EditorGUILayout.LabelField("������ ���� ���� FPS ��Ÿ Ÿ�� - " + Kernel.fpsUnscaledDeltaTime);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("FPS - " + Kernel.fps);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("������ ��� - " + Kernel.dataPath);
                EditorGUILayout.LabelField("��Ʈ���� ���� ��� - " + Kernel.streamingAssetsPath);
                EditorGUILayout.LabelField("���� ������ ��� - " + Kernel.persistentDataPath);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("ȸ�� �̸� - " + Kernel.companyName);
                EditorGUILayout.LabelField("��ǰ �̸� - " + Kernel.productName);
                EditorGUILayout.LabelField("���� - " + Kernel.version);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("���� ���� �÷��� - " + Kernel.platform);
            }
            else
            {
                EditorGUILayout.LabelField("��Ʈ���� ���� ��� - " + Kernel.streamingAssetsPath);
                EditorGUILayout.LabelField("���� ���� �÷��� - " + Kernel.platform);
            }
        }
    }
}