using Newtonsoft.Json;
using SCKRM.Json;
using SCKRM.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [AddComponentMenu("커널/Input/키 입력 값 설정", 0)]
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance { get; private set; }
        public static string settingFilePath { get; } = Path.Combine(ResourcePack.Default.Path, ResourcePack.SettingsPath, "inputManager.controlsSettingList.json");


        public static Dictionary<string, KeyCode> controlSettingList { get; set; } = new Dictionary<string, KeyCode>();



        public static bool defaultInputLock { get; set; }

        public static KeyCode[] unityKeyCodeList { get; } = Enum.GetValues(typeof(KeyCode)) as KeyCode[];



        void Awake()
        {
            if (instance != null)
                Destroy(this);

            instance = this;
        }

        void Update()
        {
            mousePosition = UnityEngine.Input.mousePosition;
            _mousePresent = UnityEngine.Input.mousePresent;
            _mouseScrollDelta = UnityEngine.Input.mouseScrollDelta;

            anyKeyDown = UnityEngine.Input.anyKeyDown;
            anyKey = UnityEngine.Input.anyKey;
        }



        public static void SettingFileSave()
        {
            string json = JsonConvert.SerializeObject(controlSettingList, Formatting.Indented);
            File.WriteAllText(settingFilePath, json);
        }

        public static void SettingFileLoad()
        {
            if (!File.Exists(settingFilePath))
                SettingFileSave();

            if (JsonManager.JsonRead(settingFilePath, out Dictionary<string, KeyCode> value, false))
                controlSettingList = value;
        }

        public static Dictionary<string, KeyCode> SettingFileRead()
        {
            if (!File.Exists(settingFilePath))
                SettingFileSave();

            if (JsonManager.JsonRead(settingFilePath, out Dictionary<string, KeyCode> value, false))
                return value;
            else
                return null;
        }



        public static bool GetKeyDown(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.GetKeyDown(keyCode);
        public static bool GetKeyDown(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && controlSettingList.ContainsKey(keyCode) && UnityEngine.Input.GetKeyDown(controlSettingList[keyCode]);

        public static bool GetKey(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.GetKey(keyCode);
        public static bool GetKey(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && controlSettingList.ContainsKey(keyCode) && UnityEngine.Input.GetKey(controlSettingList[keyCode]);

        public static bool GetKeyUp(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.GetKeyUp(keyCode);
        public static bool GetKeyUp(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && controlSettingList.ContainsKey(keyCode) && UnityEngine.Input.GetKeyUp(controlSettingList[keyCode]);



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
    }

    [Flags]
    public enum InputLockDeny
    {
        None = 0,
        All = 1 << 1,
        Default = 1 << 2,
    }
}