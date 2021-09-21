using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCKRM.Input.UI
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [AddComponentMenu("커널/Input/입력 리스트/입력 설정 버튼", 0)]
    public class ControlsSettingButton : MonoBehaviour
    {
        [SerializeField] internal ControlsButton controlsButton;

        public void InputSet() => StartCoroutine(inputSet());

        IEnumerator inputSet()
        {
            InputManager.defaultInputLock = true;
            controlsButton.inputLockObject.SetActive(true);

            controlsButton.valueText.text = ">   <";
            controlsButton.valueText.color = Color.yellow;

            yield return new WaitUntil(() => UnityEngine.Input.anyKeyDown);

            controlsButton.valueText.color = Color.white;

            for (int i = 0; i < InputManager.unityKeyCodeList.Length; i++)
            {
                KeyCode keyCode = InputManager.unityKeyCodeList[i];
                if (UnityEngine.Input.GetKeyDown(keyCode))
                {
                    if (keyCode == KeyCode.Escape)
                        InputManager.instance._keyList[InputManager.keyList.Keys.ToList().IndexOf(controlsButton.key)].value = KeyCode.None;
                    else
                        InputManager.instance._keyList[InputManager.keyList.Keys.ToList().IndexOf(controlsButton.key)].value = keyCode;

                    InputManager._KeyListSaveChanges();

                    controlsButton.valueText.text = keyCode.KeyCodeToString();
                }
            }

            InputManager.defaultInputLock = false;
            controlsButton.inputLockObject.SetActive(false);
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}