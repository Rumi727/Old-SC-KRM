using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Renderer;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomSpriteRenderer), true)]
    public class CustomSpriteRendererEditor : Editor
    {
        CustomSpriteRenderer _editor;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as CustomSpriteRenderer;

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
            UseProperty("_path");

            EditorGUILayout.Space();

            UseProperty("_customPath", "커스텀 경로");

            EditorGUILayout.Space();

            UseProperty("customSprite", "스프라이트 설정");

            if (_editor.GetComponent<SpriteRenderer>().drawMode != SpriteDrawMode.Simple)
            {
                EditorGUILayout.Space();
                UseProperty("_size");
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("새로고침"))
                _editor.Rerender();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                _editor.Rerender();
            }
        }

        void UseProperty(string propertyName)
        {
            SerializedProperty tps = serializedObject.FindProperty(propertyName);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
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