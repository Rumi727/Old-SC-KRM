using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Input;
using System.Linq;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(InputManager), true)]
    public class InputManagerEditor : Editor
    {
        int showLength = 8;
        int showPos = 0;

        bool repaint = false;

        void OnEnable()
        {
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
            //JSON 읽기
            InputManager.SettingFileLoad();

            //GUI
            EditorGUILayout.LabelField("오브젝트 리스트");

            //GUI
            EditorGUILayout.BeginHorizontal();

            if (InputManager.controlSettingList.ContainsKey(""))
                GUI.enabled = false;

            if (GUILayout.Button("추가", GUILayout.Width(40)))
                InputManager.controlSettingList.Add("", KeyCode.None);

            GUI.enabled = true;

            if (InputManager.controlSettingList.Count <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("삭제", GUILayout.Width(40)) && InputManager.controlSettingList.Count > 0)
                InputManager.controlSettingList.Remove(InputManager.controlSettingList.ToList()[InputManager.controlSettingList.Count - 1].Key);

            GUI.enabled = true;

            EditorGUILayout.Space();

            int count = EditorGUILayout.IntField("리스트 길이", InputManager.controlSettingList.Count, GUILayout.Height(21));

            EditorGUILayout.Space();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("위로", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= InputManager.controlSettingList.Count - showLength)
                GUI.enabled = false;

            if (GUILayout.Button("아래로", GUILayout.Width(50)) && showPos < InputManager.controlSettingList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //변수 설정
            if (count < 0)
                count = 0;

            if (count > InputManager.controlSettingList.Count)
            {
                for (int i = InputManager.controlSettingList.Count; i < count; i++)
                    InputManager.controlSettingList.Add("", KeyCode.None);
            }
            else if (count < InputManager.controlSettingList.Count)
            {
                for (int i = InputManager.controlSettingList.Count - 1; i >= count; i--)
                    InputManager.controlSettingList.Remove(InputManager.controlSettingList.ToList()[InputManager.controlSettingList.Count - 1].Key);
            }

            if (showLength <= InputManager.controlSettingList.Count && showPos > InputManager.controlSettingList.Count - showLength)
                showPos = InputManager.controlSettingList.Count - showLength;

            if (showLength < 33)
                showLength = 33;
            if (showLength + showPos > InputManager.controlSettingList.Count)
                showLength = InputManager.controlSettingList.Count - showPos;



            List<KeyValuePair<string, KeyCode>> controlList = InputManager.controlSettingList.ToList();

            //딕셔너리는 키를 수정할수 없기때문에, 리스트로 분활해줘야함
            List<string> keyList = new List<string>();
            List<KeyCode> valueList = new List<KeyCode>();

            for (int i = showPos; i < showPos + showLength; i++)
            {
                KeyValuePair<string, KeyCode> item = controlList[i];

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("프리팹 키", GUILayout.Width(53));
                keyList.Add(EditorGUILayout.TextField(item.Key));

                EditorGUILayout.LabelField("프리팹", GUILayout.Width(38));
                valueList.Add((KeyCode)EditorGUILayout.EnumPopup(item.Value));

                EditorGUILayout.EndHorizontal();
            }

            //키 중복 감지
            bool overlap = keyList.Count != keyList.Distinct().Count();
            if (!overlap)
            {
                //리스트 2개를 딕셔너리로 변환
                InputManager.controlSettingList = keyList.Zip(valueList, (key, value) => new { key, value }).ToDictionary(a => a.key, a => a.value);
            }

            if (GUI.changed)
                InputManager.SettingFileSave();
        }
    }
}