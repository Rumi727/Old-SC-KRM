using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using SCKRM.Input;
using System.Linq;

namespace SCKRM.InspectorEditor
{
#pragma warning disable CS0618 // ���� �Ǵ� ����� ������ �ʽ��ϴ�.
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
            EditorGUILayout.LabelField("����� ���� Ű");
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("�߰�", GUILayout.Width(40)))
                _editor._keyList.Add(new StringKeyCode());

            if (_editor._keyList.Count <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("����", GUILayout.Width(40)) && _editor._keyList.Count > 0)
                _editor._keyList.RemoveAt(_editor._keyList.Count - 1);

            GUI.enabled = true;

            EditorGUILayout.Space();

            int count = EditorGUILayout.IntField("����Ʈ ����", _editor._keyList.Count, GUILayout.Height(21));

            EditorGUILayout.Space();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("����", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= _editor._keyList.Count - showLength)
                GUI.enabled = false;

            if (GUILayout.Button("�Ʒ���", GUILayout.Width(50)) && showPos < _editor._keyList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            

            //���� ����
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
                EditorGUILayout.LabelField("Ű �̸�", GUILayout.Width(41));
                _editor._keyList[i].key = EditorGUILayout.TextField(_editor._keyList[i].key);
                EditorGUILayout.LabelField("Ű �ڵ�", GUILayout.Width(41));
                _editor._keyList[i].value = (KeyCode)EditorGUILayout.EnumPopup("", _editor._keyList[i].value);
                EditorGUILayout.EndHorizontal();
            }

            bool overlap = _editor._keyList.GroupBy(x => x.key).Where(x => x.Skip(1).Any()).Any();

            if (GUI.changed && Application.isPlaying && !overlap)
                InputManager.keyList = _editor._keyList.ToDictionary(t => t.key, t => t.value);
            
            if (overlap && !Application.isPlaying)
                EditorGUILayout.HelpBox("�ߺ��� Ű �̸��� �ִ� ĭ�� ���ŵ˴ϴ�", MessageType.Warning);
            else if (overlap && Application.isPlaying)
                EditorGUILayout.HelpBox("�ߺ��� Ű �̸��� �ִ� ĭ�� ������ ��� ���� ������ ������� �ʽ��ϴ�", MessageType.Error);

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
#pragma warning restore CS0618 // ���� �Ǵ� ����� ������ �ʽ��ϴ�.
}