using SCKRM.Input;
using SCKRM.InspectorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Language.UI
{
    [AddComponentMenu("Ŀ��/Language/��� ����Ʈ/��� ����Ʈ ����", 0)]
    public class LanguageListManager : MonoBehaviour
    {
        [SerializeField, SetName("Exit Ű�� �������� ������ ������Ʈ")]
        GameObject visibleGameObject;

        void Update()
        {
            if (InputManager.GetKeyDown("Exit"))
                Exit();
        }

        public void Exit()
        {
            gameObject.SetActive(false);
            visibleGameObject.SetActive(true);

            Kernel.AllRefresh(true, true);
        }
    }
}