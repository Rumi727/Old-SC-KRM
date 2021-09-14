using SCKRM.Input;
using SCKRM.InspectorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Resources.UI
{
    [AddComponentMenu("커널/Resources/리소스팩 리스트/리소스팩 리스트 설정", 0)]
    public class ResourcePackListManager : MonoBehaviour
    {
        [SerializeField, SetName("Exit 키를 눌렀을때 보여질 오브젝트")]
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