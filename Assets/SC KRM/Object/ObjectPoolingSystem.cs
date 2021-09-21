using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SCKRM.Object
{
    [System.Serializable]
    public class ObjectList
    {
        public List<string> ObjectKey = new List<string>();
        public List<GameObject> Object = new List<GameObject>();
    }

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [AddComponentMenu("커널/Object/오브젝트 풀링 설정", 0)]
    public class ObjectPoolingSystem : MonoBehaviour
    {
        #region variable
        /// <summary>
        /// If you don't know what you're doing, don't modify this variable in your script
        /// </summary>
        [SerializeField, Obsolete("If you don't know exactly what you're doing, don't touch this variable")] public ObjectList _PrefabObject = new ObjectList();
        /// <summary>
        /// If you don't know what you're doing, don't modify this variable in your script
        /// </summary>
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

            PrefabObject.ObjectKey = PrefabObject.ObjectKey.Distinct().ToList();
            while (PrefabObject.Object.Count > PrefabObject.ObjectKey.Count)
                PrefabObject.Object.RemoveAt(PrefabObject.ObjectKey.Count);
        }

        void Update()
        {
            while (PrefabObject.Object.Count > PrefabObject.ObjectKey.Count)
                PrefabObject.Object.RemoveAt(PrefabObject.ObjectKey.Count);

            while (PrefabObject.Object.Count < PrefabObject.ObjectKey.Count)
                PrefabObject.Object.Add(null);
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
            else if (PrefabObject.ObjectKey.Contains(ObjectKey))
            {
                GameObject gameObject = Instantiate(PrefabObject.Object[PrefabObject.ObjectKey.IndexOf(ObjectKey)], Parent);
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
            ObjectList.Object.Add(gameObject.gameObject);
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}