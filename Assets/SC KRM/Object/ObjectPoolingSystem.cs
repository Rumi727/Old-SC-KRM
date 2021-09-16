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

#pragma warning disable CS0618 // ���� �Ǵ� ����� ������ �ʽ��ϴ�.
    [AddComponentMenu("Ŀ��/Object/������Ʈ Ǯ�� ����", 0)]
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
        /// ������Ʈ�� �����մϴ�
        /// </summary>
        /// <param name="ObjectKey">������ ������Ʈ Ű</param>
        /// <returns></returns>
        public static GameObject ObjectCreate(string ObjectKey) => ObjectCreate(ObjectKey, null);

        /// <summary>
        /// ������Ʈ�� �����մϴ�
        /// </summary>
        /// <param name="ObjectKey">������ ������Ʈ Ű</param>
        /// <param name="Parent">������ ������Ʈ�� �ڽ����ΰ� ������Ʈ</param>
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
        /// ������Ʈ�� �����մϴ�
        /// </summary>
        /// <param name="ObjectKey">���� ������Ʈ Ű</param>
        /// <param name="gameObject">���� ������Ʈ</param>
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
#pragma warning restore CS0618 // ���� �Ǵ� ����� ������ �ʽ��ϴ�.
}