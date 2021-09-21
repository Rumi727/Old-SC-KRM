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
            if (!Application.isPlaying)
                GUI.enabled = false;

            LanguageManager.currentLanguage = EditorGUILayout.TextField("현재 언어", LanguageManager.currentLanguage);

            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("로드된 모든 언어의 정보");

            EditorGUILayout.Space();

            if (GUILayout.Button("로드된 모든 언어 지우기", GUILayout.Width(150)))
                LanguageManager.LangList.Clear();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("위로", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= LanguageManager.LangList.Count - showLength - 1)
                GUI.enabled = false;

            if (GUILayout.Button("아래로", GUILayout.Width(50)) && showPos < LanguageManager.LangList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //변수 설정
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
                EditorGUILayout.LabelField("키", GUILayout.Width(14));
                GUI.enabled = false;
                EditorGUILayout.TextField(item.Key);
                GUI.enabled = true;
                EditorGUILayout.LabelField("값", GUILayout.Width(14));
                GUI.enabled = false;
                EditorGUILayout.TextField(item.Value);
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}