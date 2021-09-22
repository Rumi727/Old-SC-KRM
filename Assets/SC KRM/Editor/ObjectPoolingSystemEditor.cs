using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SCKRM.Object;
using System.Threading.Tasks;
using System.Linq;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ObjectPoolingSystem), true)]
    public class ObjectPoolingSystemEditor : Editor
    {
        int showLength = 33;
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
            ObjectPoolingSystem.SettingFileLoad();

            //GUI
            EditorGUILayout.LabelField("오브젝트 리스트");

            //GUI
            EditorGUILayout.BeginHorizontal();

            if (ObjectPoolingSystem.PrefabList.ContainsKey(""))
                GUI.enabled = false;

            if (GUILayout.Button("추가", GUILayout.Width(40)))
                    ObjectPoolingSystem.PrefabList.Add("", "");

            GUI.enabled = true;

            if (ObjectPoolingSystem.PrefabList.Count <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("삭제", GUILayout.Width(40)) && ObjectPoolingSystem.PrefabList.Count > 0)
                ObjectPoolingSystem.PrefabList.Remove(ObjectPoolingSystem.PrefabList.ToList()[ObjectPoolingSystem.PrefabList.Count - 1].Key);

            GUI.enabled = true;

            EditorGUILayout.Space();

            int count = EditorGUILayout.IntField("리스트 길이", ObjectPoolingSystem.PrefabList.Count, GUILayout.Height(21));

            EditorGUILayout.Space();

            if (showPos <= 0)
                GUI.enabled = false;

            if (GUILayout.Button("위로", GUILayout.Width(40)) && showPos > 0)
                showPos--;

            GUI.enabled = true;

            if (showPos >= ObjectPoolingSystem.PrefabList.Count - showLength)
                GUI.enabled = false;

            if (GUILayout.Button("아래로", GUILayout.Width(50)) && showPos < ObjectPoolingSystem.PrefabList.Count - showLength)
                showPos++;

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();



            //변수 설정
            if (count < 0)
                count = 0;

            if (count > ObjectPoolingSystem.PrefabList.Count)
            {
                for (int i = ObjectPoolingSystem.PrefabList.Count; i < count; i++)
                    ObjectPoolingSystem.PrefabList.Add("", "");
            }
            else if (count < ObjectPoolingSystem.PrefabList.Count)
            {
                for (int i = ObjectPoolingSystem.PrefabList.Count - 1; i >= count; i--)
                    ObjectPoolingSystem.PrefabList.Remove(ObjectPoolingSystem.PrefabList.ToList()[ObjectPoolingSystem.PrefabList.Count - 1].Key);
            }

            if (showLength <= ObjectPoolingSystem.PrefabList.Count && showPos > ObjectPoolingSystem.PrefabList.Count - showLength)
                showPos = ObjectPoolingSystem.PrefabList.Count - showLength;

            if (showLength < 33)
                showLength = 33;
            if (showLength + showPos > ObjectPoolingSystem.PrefabList.Count)
                showLength = ObjectPoolingSystem.PrefabList.Count - showPos;



            //PrefabObject의 <string, string>를 <string, GameObject>로 바꿔서 인스펙터에 보여주고 인스펙터에서 변경한걸 <string, string>로 다시 바꿔서 PrefabObject에 저장
            /*
             * 왜 이렇게 변환하냐면 JSON에 오브젝트를 저장할려면 우선적으로 string 값같은 경로가 있어야하고
               인스펙터에서 쉽게 드래그로 오브젝트를 바꾸기 위해선
               GameObject 형식이여야해서 이런 소용돌이가 나오게 된것
            */
            List<KeyValuePair<string, string>> prefabObject = ObjectPoolingSystem.PrefabList.ToList();

            //딕셔너리는 키를 수정할수 없기때문에, 리스트로 분활해줘야함
            List<string> keyList = new List<string>();
            List<string> valueList = new List<string>();

            for (int i = showPos; i < showPos + showLength; i++)
            {
                KeyValuePair<string, string> item = prefabObject[i];

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("프리팹 키", GUILayout.Width(53));
                string key = EditorGUILayout.TextField(item.Key);

                EditorGUILayout.LabelField("프리팹", GUILayout.Width(38));
                //문자열(경로)을 프리팹으로 변환
                GameObject gameObject = (GameObject)EditorGUILayout.ObjectField("", UnityEngine.Resources.Load<GameObject>(item.Value), typeof(GameObject), true);

                /*
                 * 변경한 프리팹이 리소스 폴더에 있지 않은경우
                   저장은 되지만 프리팹을 감지할수 없기때문에
                   조건문으로 경고를 표시해주고
                   경로가 중첩되는 현상을 대비하기 위해 경로를 빈 문자열로 변경해줌
                 */
                string assetsPath = AssetDatabase.GetAssetPath(gameObject);
                if (assetsPath.Contains("Resources"))
                {
                    keyList.Add(key);

                    assetsPath = assetsPath.Substring(assetsPath.IndexOf("Resources") + 10);
                    assetsPath = assetsPath.Remove(assetsPath.LastIndexOf("."));

                    valueList.Add(assetsPath);

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    keyList.Add(key);
                    valueList.Add("");

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.HelpBox("'Resources' 폴더에 있는 오브젝트를 넣어주세요", MessageType.Info);
                }
            }

            //키 중복 감지
            bool overlap = keyList.Count != keyList.Distinct().Count();
            if (!overlap)
            {
                //리스트 2개를 딕셔너리로 변환
                ObjectPoolingSystem.PrefabList = keyList.Zip(valueList, (key, value) => new { key, value }).ToDictionary(a => a.key, a => a.value);
            }

            //JSON으로 저장
            if (GUI.changed)
                ObjectPoolingSystem.SettingFileSave();
        }
    }
}