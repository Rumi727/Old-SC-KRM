using SCKRM.Input;
using SCKRM.InspectorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Resources.UI
{
    [AddComponentMenu("Ŀ��/Resources/���ҽ��� ����Ʈ/���ҽ��� ����Ʈ ����", 0)]
    public class ResourcePackListManager : MonoBehaviour
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

            Kernel.AllRefresh(true);
        }
    }
}