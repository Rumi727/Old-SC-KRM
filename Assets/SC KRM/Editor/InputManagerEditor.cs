using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Input;
using System.Linq;

namespace SCKRM.InspectorEditor
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [CanEditMultipleObjects]
    [CustomEditor(typeof(InputManager), true)]
    public class InputManagerEditor : Editor
    {
        InputManager _editor;

        int showLength = 8;
        int showPos = 0;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as InputManager;

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
            EditorGUILayout.LabelField("사용자 지정 키");
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("추가", GUILayout.Width(40)))
                _editor._keyList.Add(new StringKeyCode());

            if (_editor._keyList.Count <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("삭제", GUILayout.Width(40)) && _editor._keyList.Count > 0)
                _editor._keyList.RemoveAt(_editor._keyList.Count - 1);

            GUI.enabled = true;

            EditorGUILayout.Space();

            int count = EditorGUILayout.IntField("리스트 길이", _editor._keyList.Count, GUILayout.Height(21));

            EditorGUILayout.Space();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("위로", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= _editor._keyList.Count - showLength)
                GUI.enabled = false;

            if (GUILayout.Button("아래로", GUILayout.Width(50)) && showPos < _editor._keyList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            

            //변수 설정
            if (count < 0)
                count = 0;

            if (count > _editor._keyList.Count)
            {
                for (int i = _editor._keyList.Count; i < count; i++)
                    _editor._keyList.Add(new StringKeyCode());
            }
            else if (count < _editor._keyList.Count)
            {
                for (int i = _editor._keyList.Count - 1; i >= count; i--)
                    _editor._keyList.RemoveAt(i);
            }

            if (showLength <= _editor._keyList.Count && showPos > _editor._keyList.Count - showLength)
                showPos = _editor._keyList.Count - showLength;

            if (showLength < 33)
                showLength = 33;
            if (showLength + showPos > _editor._keyList.Count)
                showLength = _editor._keyList.Count - showPos;

            for (int i = showPos; i < showPos + showLength; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("키 이름", GUILayout.Width(41));
                _editor._keyList[i].key = EditorGUILayout.TextField(_editor._keyList[i].key);
                EditorGUILayout.LabelField("키 코드", GUILayout.Width(41));
                _editor._keyList[i].value = (KeyCode)EditorGUILayout.EnumPopup("", _editor._keyList[i].value);
                EditorGUILayout.EndHorizontal();
            }

            bool overlap = _editor._keyList.GroupBy(x => x.key).Where(x => x.Skip(1).Any()).Any();

            if (GUI.changed && Application.isPlaying && !overlap)
                InputManager._KeyListSaveChanges();
            
            if (overlap && !Application.isPlaying)
                EditorGUILayout.HelpBox("중복된 키 이름이 있는 칸은 제거됩니다", MessageType.Warning);
            else if (overlap && Application.isPlaying)
                EditorGUILayout.HelpBox("중복된 키 이름이 있는 칸이 있으면 모든 변경 사항이 저장되지 않습니다", MessageType.Error);

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}