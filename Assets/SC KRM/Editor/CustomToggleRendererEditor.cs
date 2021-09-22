using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Renderer;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomToggleRenderer), true)]
    public class CustomToggleRendererEditor : Editor
    {
        CustomToggleRenderer _editor;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as CustomToggleRenderer;
            
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
            UseProperty("_normalPath", "일반 스프라이트 경로");
            UseProperty("_highlightedPath", "하이라이트된 스프라이트 경로");
            UseProperty("_pressedPath", "눌린 스프라이트 경로");
            UseProperty("_selectedPath", "선택된 스프라이트 경로");
            UseProperty("_disabledPath", "비활성화된 스프라이트 경로");

            EditorGUILayout.Space();

            UseProperty("_customPath", "커스텀 경로");

            EditorGUILayout.Space();

            UseProperty("buttonSprite", "버튼 스프라이트 설정");

            if (GUILayout.Button("새로고침"))
                _editor.Rerender();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                _editor.Rerender();
            }
        }

        void UseProperty(string propertyName, string label)
        {
            GUIContent GUIContent = new GUIContent();
            GUIContent.text = label;

            SerializedProperty tps = serializedObject.FindProperty(propertyName);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, GUIContent, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}