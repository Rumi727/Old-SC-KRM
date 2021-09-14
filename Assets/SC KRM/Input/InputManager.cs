using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SCKRM.Input
{
    [System.Serializable]
    public class StringKeyCode
    {
        public string key = "";
        public KeyCode value = KeyCode.None;
    }

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [AddComponentMenu("커널/Input/키 입력 값 설정", 0)]
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance { get; private set; }

        [Obsolete("If you don't know exactly what you're doing, don't touch this variable")] public List<StringKeyCode> _keyList = new List<StringKeyCode>();
        [Obsolete("If you don't know exactly what you're doing, don't touch this variable")] public static Dictionary<string, KeyCode> keyList { get; set; }

        public static bool GetKeyDown(KeyCode keyCode) => UnityEngine.Input.GetKeyDown(keyCode);
        public static bool GetKeyDown(string keyCode) => keyList.ContainsKey(keyCode) && UnityEngine.Input.GetKeyDown(keyList[keyCode]);
        public static bool GetKey(KeyCode keyCode) => UnityEngine.Input.GetKey(keyCode);
        public static bool GetKey(string keyCode) => keyList.ContainsKey(keyCode) && UnityEngine.Input.GetKey(keyList[keyCode]);
        public static bool GetKeyUp(KeyCode keyCode) => UnityEngine.Input.GetKeyUp(keyCode);
        public static bool GetKeyUp(string keyCode) => keyList.ContainsKey(keyCode) && UnityEngine.Input.GetKeyUp(keyList[keyCode]);

        public static Vector2 mousePosition { get; private set; }
        public static bool mousePresent { get; private set; }
        public static Vector2 mouseScrollDelta { get; private set; }

        void Awake()
        {
            if (instance != null)
                Destroy(this);

            instance = this;

            List<StringKeyCode> deduplicationKeyList = _keyList;
            for (int i = 0; i < _keyList.Count; i++)
            {
                StringKeyCode item = _keyList[i];
                if (i + 1 < _keyList.Count && item.key == _keyList[i + 1].key)
                {
                    deduplicationKeyList.Remove(item);
                    i--;
                }
            }

            keyList = _keyList.ToDictionary(t => t.key, t => t.value);
        }

        void Update()
        {
            mousePosition = UnityEngine.Input.mousePosition;
            mousePresent = UnityEngine.Input.mousePresent;
            mouseScrollDelta = UnityEngine.Input.mouseScrollDelta;
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}