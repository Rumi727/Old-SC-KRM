using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCKRM.Object
{
    [System.Serializable]
    public class ObjectList
    {
        public List<string> ObjectName = new List<string>();
        public List<GameObject> Object = new List<GameObject>();
    }

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [AddComponentMenu("커널/Object/오브젝트 풀링 설정", 0)]
    public class ObjectPoolingSystem : MonoBehaviour
    {
        /// <summary>
        /// If you don't know what you're doing, don't modify this variable in your script
        /// </summary>
        #region variable
        [SerializeField, Obsolete("If you don't know exactly what you're doing, don't touch this variable")] public ObjectList _PrefabObject = new ObjectList();
        [Obsolete("If you don't know exactly what you're doing, don't touch this variable")] public static ObjectList PrefabObject { get; set; }

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
            PrefabObject = _PrefabObject;

            PrefabObject.ObjectName = PrefabObject.ObjectName.Distinct().ToList();
            while (PrefabObject.Object.Count > PrefabObject.ObjectName.Count)
                PrefabObject.Object.RemoveAt(PrefabObject.ObjectName.Count);
        }

        void Update()
        {
            while (PrefabObject.Object.Count > PrefabObject.ObjectName.Count)
                PrefabObject.Object.RemoveAt(PrefabObject.ObjectName.Count);

            while (PrefabObject.Object.Count < PrefabObject.ObjectName.Count)
                PrefabObject.Object.Add(null);
        }

        public static GameObject ObjectCreate(string ObjectName) => ObjectCreate(ObjectName, null);

        public static GameObject ObjectCreate(string ObjectName, Transform Parent)
        {
            if (ObjectList.ObjectName.Contains(ObjectName))
            {
                GameObject gameObject = ObjectList.Object[ObjectList.ObjectName.IndexOf(ObjectName)];
                gameObject.transform.SetParent(Parent);
                gameObject.SetActive(true);

                int i = ObjectList.ObjectName.IndexOf(ObjectName);
                ObjectList.ObjectName.RemoveAt(i);
                ObjectList.Object.RemoveAt(i);

                return gameObject;
            }
            else if (PrefabObject.ObjectName.Contains(ObjectName))
            {
                GameObject gameObject = Instantiate(PrefabObject.Object[PrefabObject.ObjectName.IndexOf(ObjectName)], Parent);
                gameObject.name = ObjectName;

                return gameObject;
            }

            return null;
        }

        public static void ObjectRemove(string ObjectName, GameObject gameObject, Action onDestroy)
        {
            onDestroy.Invoke();
            gameObject.SetActive(false);
            gameObject.transform.SetParent(thisTransform);
            ObjectList.ObjectName.Add(ObjectName);
            ObjectList.Object.Add(gameObject.gameObject);
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}