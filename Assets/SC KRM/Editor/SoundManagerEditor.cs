using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SCKRM.Sound;
using System.Threading.Tasks;
using System;

namespace SCKRM.InspectorEditor
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SoundManager), true)]
    public class SoundManagerEditor : Editor
    {
        SoundManager _editor;

        int showLength = 8;
        int showPos = 0;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as SoundManager;

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
            if (_editor.BGM == null)
            {
                EditorGUILayout.LabelField("음악이 생성될 오브젝트의 부모 오브젝트를 아래에 넣어주세요!!");
                UseProperty("_BGM", "음악을 담을 오브젝트");
                EditorGUILayout.Space();
            }
            if (_editor.Sound == null)
            {
                EditorGUILayout.LabelField("효과음이 생성될 오브젝트의 부모 오브젝트를 아래에 넣어주세요!!");
                UseProperty("_Sound", "효과음을 담을 오브젝트");
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("재생되고 있는 음악 - " + SoundManager.BGMList.Count + " / " + SoundManager.MaxBGMCount);
            EditorGUILayout.LabelField("재생되고 있는 효과음 - " + SoundManager.SoundList.Count + " / " + SoundManager.MaxSoundCount);

            EditorGUILayout.Space();

            //GUI
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("재생되고 있는 모든 오디오 클립의 정보");

            EditorGUILayout.Space();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("위로", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= SoundManager.AllList.Count - showLength - 1)
                GUI.enabled = false;

            if (GUILayout.Button("아래로", GUILayout.Width(50)) && showPos < SoundManager.AllList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //변수 설정
            if (showLength <= SoundManager.AllList.Count && showPos > SoundManager.AllList.Count - showLength)
                showPos = SoundManager.AllList.Count - showLength;

            if (showLength < 8)
                showLength = 8;
            if (showLength + showPos > SoundManager.AllList.Count)
                showLength = SoundManager.AllList.Count - showPos;

            for (int i = showPos; i < showPos + showLength; i++)
            {
                AudioSource audioSource = SoundManager.AllList[i].audioSource;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(audioSource.gameObject.name);
                Rect rect = GUILayoutUtility.GetRect(50, 21);
                string time;
                string endTime;

                
                if (audioSource.clip.length >= 3600)
                {
                    time = TimeSpan.FromSeconds(audioSource.time).ToString(@"h\:mm\:ss\.ff");
                    endTime = TimeSpan.FromSeconds(audioSource.clip.length).ToString(@"h\:mm\:ss\.ff");
                }
                else if (audioSource.clip.length >= 60)
                {
                    time = TimeSpan.FromSeconds(audioSource.time).ToString(@"m\:ss");
                    endTime = TimeSpan.FromSeconds(audioSource.clip.length).ToString(@"m\:ss");
                }
                else
                {
                    time = TimeSpan.FromSeconds(audioSource.time).ToString(@"s\.ff");
                    endTime = TimeSpan.FromSeconds(audioSource.clip.length).ToString(@"s\.ff");
                }

                if (audioSource.time > TimeSpan.MaxValue.TotalSeconds)
                    time = TimeSpan.FromSeconds(TimeSpan.MaxValue.TotalSeconds).ToString(@"h\:mm\:ss\.ff");

                if (audioSource.clip.length > TimeSpan.MaxValue.TotalSeconds)
                    endTime = TimeSpan.FromSeconds(TimeSpan.MaxValue.TotalSeconds).ToString(@"h\:mm\:ss\.ff");

                EditorGUI.ProgressBar(rect, audioSource.time / audioSource.clip.length, time + " / " + endTime);
                EditorGUILayout.EndHorizontal();
            }
            
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
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}