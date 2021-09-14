using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SCKRM.Sound;
using System.Threading.Tasks;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SoundObject), true)]
    public class SoundObjectEditor : Editor
    {
        SoundObject _editor;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as SoundObject;

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
            EditorGUILayout.LabelField("���� Ÿ�� - " + _editor.soundType);
            EditorGUILayout.LabelField("���� �ð� - " + _editor.audioSource.time);
            EditorGUILayout.LabelField("����� ���� ��� - " + _editor.path);

            EditorGUILayout.Space();

            UseProperty("_volume", "����");
            UseProperty("_pitch", "��ġ");

            if (GUI.changed)
                EditorUtility.SetDirty(target);
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