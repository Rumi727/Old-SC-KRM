using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SCKRM.Object;
using System.Threading.Tasks;
using System.Linq;

namespace SCKRM.InspectorEditor
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ObjectPoolingSystem), true)]
    public class ObjectPoolingSystemEditor : Editor
    {
        ObjectPoolingSystem _editor;

        int showLength = 33;
        int showPos = 0;

        bool repaint = false;

        void OnEnable()
        {
            _editor = target as ObjectPoolingSystem;

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
            EditorGUILayout.LabelField("오브젝트 리스트");

            //GUI
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("추가", GUILayout.Width(40)))
            {
                _editor._PrefabObject.ObjectName.Add("");
                _editor._PrefabObject.Object.Add(null);
            }

            if (_editor._PrefabObject.ObjectName.Count <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("삭제", GUILayout.Width(40)) && _editor._PrefabObject.ObjectName.Count > 0)
            {
                _editor._PrefabObject.ObjectName.RemoveAt(_editor._PrefabObject.ObjectName.Count - 1);
                _editor._PrefabObject.Object.RemoveAt(_editor._PrefabObject.Object.Count - 1);
            }

            GUI.enabled = true;

            EditorGUILayout.Space();

            int count = EditorGUILayout.IntField("리스트 길이", _editor._PrefabObject.ObjectName.Count, GUILayout.Height(21));

            EditorGUILayout.Space();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("위로", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= _editor._PrefabObject.ObjectName.Count - showLength)
                GUI.enabled = false;

            if (GUILayout.Button("아래로", GUILayout.Width(50)) && showPos < _editor._PrefabObject.ObjectName.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //변수 설정
            while (_editor._PrefabObject.Object.Count > _editor._PrefabObject.ObjectName.Count)
                _editor._PrefabObject.Object.RemoveAt(_editor._PrefabObject.ObjectName.Count);

            while (_editor._PrefabObject.Object.Count < _editor._PrefabObject.ObjectName.Count)
                _editor._PrefabObject.Object.Add(null);

            if (count < 0)
                count = 0;

            if (count > _editor._PrefabObject.ObjectName.Count)
            {
                for (int i = _editor._PrefabObject.ObjectName.Count; i < count; i++)
                {
                    _editor._PrefabObject.ObjectName.Add("");
                    _editor._PrefabObject.Object.Add(null);
                }
            }
            else if (count < _editor._PrefabObject.ObjectName.Count)
            {
                for (int i = _editor._PrefabObject.ObjectName.Count - 1; i >= count; i--)
                {
                    _editor._PrefabObject.ObjectName.RemoveAt(i);
                    _editor._PrefabObject.Object.RemoveAt(i);
                }
            }

            if (showLength <= _editor._PrefabObject.ObjectName.Count && showPos > _editor._PrefabObject.ObjectName.Count - showLength)
                showPos = _editor._PrefabObject.ObjectName.Count - showLength;

            if (showLength < 33)
                showLength = 33;
            if (showLength + showPos > _editor._PrefabObject.ObjectName.Count)
                showLength = _editor._PrefabObject.ObjectName.Count - showPos;

            for (int i = showPos; i < showPos + showLength; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("프리팹 이름", GUILayout.Width(66));
                _editor._PrefabObject.ObjectName[i] = EditorGUILayout.TextField(_editor._PrefabObject.ObjectName[i]);
                EditorGUILayout.LabelField("프리팹", GUILayout.Width(38));
                _editor._PrefabObject.Object[i] = (GameObject)EditorGUILayout.ObjectField("", _editor._PrefabObject.Object[i], typeof(GameObject), true);
                EditorGUILayout.EndHorizontal();
            }

            bool overlap = _editor._PrefabObject.ObjectName.Count != _editor._PrefabObject.ObjectName.Distinct().Count();

            if (GUI.changed && Application.isPlaying && !overlap)
                ObjectPoolingSystem.PrefabObject = _editor._PrefabObject;

            if (overlap && !Application.isPlaying)
                EditorGUILayout.HelpBox("중복된 프리팹 이름이 있는 칸은 제거됩니다", MessageType.Warning);
            else if (overlap && Application.isPlaying)
                EditorGUILayout.HelpBox("맨 위에 있는 칸을 제외한 중복된 프리팹 이름이 있는 칸은 무시됩니다", MessageType.Warning);

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}