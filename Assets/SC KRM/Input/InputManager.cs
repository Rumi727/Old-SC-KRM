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

        public static bool defaultInputLock { get; set; }

        public static KeyCode[] unityKeyCodeList { get; } = Enum.GetValues(typeof(KeyCode)) as KeyCode[];


        /// <summary>
        /// The system that manages this variable is serious spaghetti code! If you don't know exactly what you're doing, please stop doing what you're doing.
        /// </summary>
		[Obsolete("The system that manages this variable is serious spaghetti code! If you don't know exactly what you're doing, please stop doing what you're doing.")] public List<StringKeyCode> _keyList = new List<StringKeyCode>();
        /// <summary>
        /// The system that manages this variable is serious spaghetti code! If you don't know exactly what you're doing, please stop doing what you're doing.
        /// </summary>
        [Obsolete("The system that manages this variable is serious spaghetti code! If you don't know exactly what you're doing, please stop doing what you're doing.")] public static Dictionary<string, KeyCode> keyList { get; set; }
        /// <summary>
        /// The system that manages this variable is serious spaghetti code! If you don't know exactly what you're doing, please stop doing what you're doing.
        /// </summary>
        [Obsolete("The system that manages this variable is serious spaghetti code! If you don't know exactly what you're doing, please stop doing what you're doing.")] public static Dictionary<string, KeyCode> defaultKeyList { get; private set; }



        public static bool GetKeyDown(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.GetKeyDown(keyCode);
        public static bool GetKeyDown(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && keyList.ContainsKey(keyCode) && UnityEngine.Input.GetKeyDown(keyList[keyCode]);

        public static bool GetKey(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.GetKey(keyCode);
        public static bool GetKey(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && keyList.ContainsKey(keyCode) && UnityEngine.Input.GetKey(keyList[keyCode]);

        public static bool GetKeyUp(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.GetKeyUp(keyCode);
        public static bool GetKeyUp(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && keyList.ContainsKey(keyCode) && UnityEngine.Input.GetKeyUp(keyList[keyCode]);



        public static Vector2 mousePosition { get; private set; }

        static bool _mousePresent;
        public static bool GetMousePresent(InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && _mousePresent;

        static Vector2 _mouseScrollDelta;
        public static Vector2 GetMouseScrollDelta(InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (!InputLockCheck(inputLockDeny))
                return _mouseScrollDelta;
            else
                return Vector2.zero;
        }



        static bool anyKeyDown;
        public static bool GetAnyKeyDown(InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && anyKeyDown;

        static bool anyKey;
        public static bool GetAnyKey(InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && anyKey;



        public static bool InputLockCheck(InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if ((inputLockDeny & InputLockDeny.All) != 0)
                return false;

            bool inputLock = false;

            if (defaultInputLock && (inputLockDeny & InputLockDeny.Default) == 0)
                inputLock = true;

            return inputLock;
        }

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

            _KeyListSaveChanges();
            defaultKeyList = new Dictionary<string, KeyCode>(keyList);
        }

        void Update()
        {
            mousePosition = UnityEngine.Input.mousePosition;
            _mousePresent = UnityEngine.Input.mousePresent;
            _mouseScrollDelta = UnityEngine.Input.mouseScrollDelta;

            anyKeyDown = UnityEngine.Input.anyKeyDown;
            anyKey = UnityEngine.Input.anyKey;
        }

        public static void _KeyListSaveChanges() => keyList = instance._keyList.ToDictionary(t => t.key, t => t.value);
    }

    [Flags]
    public enum InputLockDeny
    {
        None = 0,
        All = 1 << 1,
        Default = 1 << 2,
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}