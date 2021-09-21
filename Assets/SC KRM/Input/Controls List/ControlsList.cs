using SCKRM.Language;
using SCKRM.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Input.UI
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [AddComponentMenu("커널/Input/조작 설정 리스트/조작 설정 리스트", 0)]
    public class ControlsList : MonoBehaviour
    {
        [SerializeField] GameObject inputLockObject;

        void OnEnable()
        {
            ControlsButton[] child = GetComponentsInChildren<ControlsButton>();
            for (int i = 0; i < child.Length; i++)
            {
                ControlsButton item = child[i];
                ObjectPoolingSystem.ObjectRemove("controls_list.controls_button", item.gameObject, item.OnDestroy);
            }

            foreach (var item in InputManager.keyList)
            {
                if (item.Value == KeyCode.Escape)
                    continue;

                ControlsButton controlsButton = ObjectPoolingSystem.ObjectCreate("controls_list.controls_button", transform).GetComponent<ControlsButton>();
                controlsButton.key = item.Key;
                controlsButton.inputLockObject = inputLockObject;

                string keyLanguage = LanguageManager.LanguageLoad(controlsButton.key);
                if (keyLanguage != "null")
                    controlsButton.keyText.text = keyLanguage;
                else
                    controlsButton.keyText.text = controlsButton.key;

                controlsButton.valueText.text = item.Value.KeyCodeToString();
            }
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}