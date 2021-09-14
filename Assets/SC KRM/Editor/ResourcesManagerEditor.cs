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
            EditorGUILayout.LabelField("로드된 모든 오디오 클립의 정보");

            EditorGUILayout.Space();

            if (GUILayout.Button("로드된 모든 오디오 클립 지우기", GUILayout.Width(190)))
                ResourcesManager.AudioList.Clear();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("위로", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= ResourcesManager.AudioList.Count - showLength - 1)
                GUI.enabled = false;

            if (GUILayout.Button("아래로", GUILayout.Width(50)) && showPos < ResourcesManager.AudioList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //변수 설정
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
                EditorGUILayout.LabelField("경로", GUILayout.Width(27));
                GUI.enabled = false;
                EditorGUILayout.TextField(item.Key);
                GUI.enabled = true;
                EditorGUILayout.LabelField("오디오 클립", GUILayout.Width(38));
                GUI.enabled = false;
                EditorGUILayout.ObjectField(item.Value, typeof(AudioClip), true);
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }*/

            if (!Application.isPlaying)
                GUI.enabled = false;

            if (GUILayout.Button("새로고침"))
                Kernel.AllRefresh();

            if (GUILayout.Button("언어 새로고침"))
                Kernel.AllRefresh(true, true);

            GUI.enabled = true;
        }
    }
}