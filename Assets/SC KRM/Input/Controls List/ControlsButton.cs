using SCKRM.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/입력 버튼", 0)]
    public class ControlsButton : MonoBehaviour
    {
        internal string key = "";

        [SerializeField] internal Text keyText;
        [SerializeField] internal Text valueText;

        internal GameObject inputLockObject;

        public void KeyReset()
        {
            if (JsonManager.JsonRead(key, InputManager.settingFilePath, out KeyCode keyCode, false))
            {
                InputManager.controlSettingList[key] = keyCode;
                valueText.text = keyCode.KeyCodeToString();
            }
        }

        public void OnDestroy()
        {
            key = "";
            keyText.text = "";
            valueText.text = "";
            valueText.color = Color.white;
        }
    }
}