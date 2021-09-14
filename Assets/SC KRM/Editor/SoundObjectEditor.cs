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
            EditorGUILayout.LabelField("사운드 타입 - " + _editor.soundType);
            EditorGUILayout.LabelField("현재 시간 - " + _editor.audioSource.time);
            EditorGUILayout.LabelField("오디오 파일 경로 - " + _editor.path);

            EditorGUILayout.Space();

            UseProperty("_volume", "볼륨");
            UseProperty("_pitch", "피치");

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