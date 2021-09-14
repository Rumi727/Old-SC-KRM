using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Renderer;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomScrollbarRenderer), true)]
    public class CustomScrollbarRendererEditor : Editor
    {
        CustomScrollbarRenderer _editor;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as CustomScrollbarRenderer;
            
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
            UseProperty("_normalPath", "�Ϲ� ��������Ʈ ���");
            UseProperty("_highlightedPath", "���̶���Ʈ�� ��������Ʈ ���");
            UseProperty("_pressedPath", "���� ��������Ʈ ���");
            UseProperty("_selectedPath", "���õ� ��������Ʈ ���");
            UseProperty("_disabledPath", "��Ȱ��ȭ�� ��������Ʈ ���");

            EditorGUILayout.Space();

            UseProperty("buttonSprite", "��ư ��������Ʈ ����");

            if (GUILayout.Button("���ΰ�ħ"))
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