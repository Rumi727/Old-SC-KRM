using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Language;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LanguageManager), true)]
    public class LanguageManagerEditor : Editor
    {
        LanguageManager _editor;

        int showLength = 8;
        int showPos = 0;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as LanguageManager;

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
            //GUI
            LanguageManager.currentLanguage = EditorGUILayout.TextField("���� ���", LanguageManager.currentLanguage);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("�ε�� ��� ����� ����");

            EditorGUILayout.Space();

            if (GUILayout.Button("�ε�� ��� ��� �����", GUILayout.Width(150)))
                LanguageManager.LangList.Clear();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("����", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= LanguageManager.LangList.Count - showLength - 1)
                GUI.enabled = false;

            if (GUILayout.Button("�Ʒ���", GUILayout.Width(50)) && showPos < LanguageManager.LangList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //���� ����
            if (showLength <= LanguageManager.LangList.Count && showPos > LanguageManager.LangList.Count - showLength)
                showPos = LanguageManager.LangList.Count - showLength;

            if (showLength < 8)
                showLength = 8;
            if (showLength + showPos > LanguageManager.LangList.Count)
                showLength = LanguageManager.LangList.Count - showPos;

            int i = showPos;
            foreach (var item in LanguageManager.LangList)
            {
                if (i >= showPos + showLength)
                    break;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ű", GUILayout.Width(14));
                GUI.enabled = false;
                EditorGUILayout.TextField(item.Key);
                GUI.enabled = true;
                EditorGUILayout.LabelField("��", GUILayout.Width(14));
                GUI.enabled = false;
                EditorGUILayout.TextField(item.Value);
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}