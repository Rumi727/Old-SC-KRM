using Newtonsoft.Json;
using SCKRM.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SCKRM.Object
{
    class ObjectList
    {
        public List<string> ObjectKey = new List<string>();
        public List<GameObject> Object = new List<GameObject>();
    }

    [AddComponentMenu("커널/Object/오브젝트 풀링 설정", 0)]
    public class ObjectPoolingSystem : MonoBehaviour
    {
        #region variable
        public static Dictionary<string, string> PrefabList { get; set; } = new Dictionary<string, string>();

        static ObjectList ObjectList { get; } = new ObjectList();

        public static ObjectPoolingSystem instance { get; private set; }
        public static Transform thisTransform { get; private set; }
        #endregion

        void Awake()
        {
            if (instance != null)
                Destroy(gameObject);

            instance = this;

            thisTransform = transform;

            SettingFileLoad();
        }

        public static void SettingFileSave()
        {
            string path = Path.Combine(ResourcePack.Default.Path, ResourcePack.SettingsPath, "objectPoolingSystem.prefabList.json");
            string json = JsonConvert.SerializeObject(PrefabList, Formatting.Indented);

            File.WriteAllText(path, json);
        }

        public static void SettingFileLoad()
        {
            string path = Path.Combine(ResourcePack.Default.Path, ResourcePack.SettingsPath, "objectPoolingSystem.prefabList.json");
            string json = File.ReadAllText(path);

            PrefabList = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="ObjectKey">생성할 오브젝트 키</param>
        /// <returns></returns>
        public static GameObject ObjectCreate(string ObjectKey) => ObjectCreate(ObjectKey, null);

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="ObjectKey">생성할 오브젝트 키</param>
        /// <param name="Parent">생성할 오브젝트가 자식으로갈 오브젝트</param>
        /// <returns></returns>
        public static GameObject ObjectCreate(string ObjectKey, Transform Parent)
        {
            if (ObjectList.ObjectKey.Contains(ObjectKey))
            {
                GameObject gameObject = ObjectList.Object[ObjectList.ObjectKey.IndexOf(ObjectKey)];
                gameObject.transform.SetParent(Parent);
                gameObject.SetActive(true);

                int i = ObjectList.ObjectKey.IndexOf(ObjectKey);
                ObjectList.ObjectKey.RemoveAt(i);
                ObjectList.Object.RemoveAt(i);

                return gameObject;
            }
            else if (PrefabList.ContainsKey(ObjectKey))
            {
                GameObject gameObject = Instantiate(UnityEngine.Resources.Load<GameObject>(PrefabList[ObjectKey]), Parent);
                gameObject.name = ObjectKey;

                return gameObject;
            }

            return null;
        }

        /// <summary>
        /// 오브젝트를 삭제합니다
        /// </summary>
        /// <param name="ObjectKey">지울 오브젝트 키</param>
        /// <param name="gameObject">지울 오브젝트</param>
        /// <param name="onDestroy"></param>
        public static void ObjectRemove(string ObjectKey, GameObject gameObject, Action onDestroy)
        {
            onDestroy.Invoke();
            gameObject.SetActive(false);
            gameObject.transform.SetParent(thisTransform);

            ObjectList.ObjectKey.Add(ObjectKey);
            ObjectList.Object.Add(gameObject);
        }
    }
}