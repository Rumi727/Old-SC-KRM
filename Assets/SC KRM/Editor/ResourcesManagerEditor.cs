using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Resources;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ResourcesManager), true)]
    public class ResourcesManagerEditor : Editor
    {
        /*ResourcesManager _editor;

        int showLength = 8;
        int showPos = 0;*/

        bool repaint = false;

        void OnEnable()
        {
            //_editor = target as ResourcesManager;

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
            /*//GUI
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("�ε�� ��� ����� Ŭ���� ����");

            EditorGUILayout.Space();

            if (GUILayout.Button("�ε�� ��� ����� Ŭ�� �����", GUILayout.Width(190)))
                ResourcesManager.AudioList.Clear();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("����", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= ResourcesManager.AudioList.Count - showLength - 1)
                GUI.enabled = false;

            if (GUILayout.Button("�Ʒ���", GUILayout.Width(50)) && showPos < ResourcesManager.AudioList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //���� ����
            if (showLength <= ResourcesManager.AudioList.Count && showPos > ResourcesManager.AudioList.Count - showLength)
                showPos = ResourcesManager.AudioList.Count - showLength;

            if (showLength < 8)
                showLength = 8;
            if (showLength + showPos > ResourcesManager.AudioList.Count)
                showLength = ResourcesManager.AudioList.Count - showPos;

            int i = showPos;
            foreach (var item in ResourcesManager.AudioList)
            {
                if (i >= showPos + showLength)
                    break;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("���", GUILayout.Width(27));
                GUI.enabled = false;
                EditorGUILayout.TextField(item.Key);
                GUI.enabled = true;
                EditorGUILayout.LabelField("����� Ŭ��", GUILayout.Width(38));
                GUI.enabled = false;
                EditorGUILayout.ObjectField(item.Value, typeof(AudioClip), true);
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }*/

            if (!Application.isPlaying)
                GUI.enabled = false;

            if (GUILayout.Button("���ΰ�ħ"))
                Kernel.AllRefresh();

            if (GUILayout.Button("��� ���ΰ�ħ"))
                Kernel.AllRefresh(true, true);

            GUI.enabled = true;
        }
    }
}