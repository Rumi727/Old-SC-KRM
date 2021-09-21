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
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            InputManager.instance._keyList[InputManager.keyList.Keys.ToList().IndexOf(key)].value = InputManager.defaultKeyList[key];
            InputManager._KeyListSaveChanges();

            valueText.text = InputManager.defaultKeyList[key].KeyCodeToString();
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
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