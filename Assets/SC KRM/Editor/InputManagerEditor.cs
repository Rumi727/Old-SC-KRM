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
        bool deleteSafety = true;

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
            //플레이 모드가 아니면 파일에서 JSON을 읽어서 리스트 불러오기
            if (!Application.isPlaying)
                InputManager.SettingFileLoad();

            //GUI
            EditorGUILayout.LabelField("오브젝트 리스트");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("안전 삭제 모드 (삭제 할 리스트가 빈 값이 아니면 삭제 금지)", GUILayout.Width(330));
            deleteSafety = EditorGUILayout.Toggle(deleteSafety);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //GUI
            EditorGUILayout.BeginHorizontal();

            if (InputManager.controlSettingList.ContainsKey(""))
                GUI.enabled = false;

            if (GUILayout.Button("추가", GUILayout.Width(40)))
                InputManager.controlSettingList.Add("", KeyCode.None);

            GUI.enabled = true;

            if (InputManager.controlSettingList.Count <= 0 || ((InputManager.controlSettingList.Keys.ToList()[InputManager.controlSettingList.Count - 1] != "" || InputManager.controlSettingList.Values.ToList()[InputManager.controlSettingList.Count - 1] != KeyCode.None) && deleteSafety))
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
                {
                    if (!InputManager.controlSettingList.ContainsKey(""))
                        InputManager.controlSettingList.Add("", KeyCode.None);
                    else
                        count--;
                }
            }
            else if (count < InputManager.controlSettingList.Count)
            {
                for (int i = InputManager.controlSettingList.Count - 1; i >= count; i--)
                {
                    if ((InputManager.controlSettingList.Keys.ToList()[InputManager.controlSettingList.Count - 1] == "" && InputManager.controlSettingList.Values.ToList()[InputManager.controlSettingList.Count - 1] == KeyCode.None) || !deleteSafety)
                        InputManager.controlSettingList.Remove(InputManager.controlSettingList.ToList()[InputManager.controlSettingList.Count - 1].Key);
                    else
                        count++;
                }
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

            //플레이 모드가 아니면 변경한 리스트의 데이터를 잃어버리지 않게 파일로 저장
            if (GUI.changed && !Application.isPlaying)
                InputManager.SettingFileSave();
        }
    }
}