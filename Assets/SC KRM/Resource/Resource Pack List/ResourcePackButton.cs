using SCKRM.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Resources.UI
{
    [AddComponentMenu("커널/Resources/리소스팩 리스트/리소스팩 설정 버튼", 2)]
    public class ResourcePackButton : MonoBehaviour
    {
        internal ResourcePack resourcePack;

        internal ResourcePackList resourcePackList;

        internal int index = 0;

        [SerializeField] internal RectTransform rectTransform;

        [SerializeField] internal Image icon;
        [SerializeField] new internal Text name;
        [SerializeField] internal Text description;

        [SerializeField] internal GameObject button;

        [SerializeField] internal GameObject rightButton;
        [SerializeField] internal GameObject leftButton;
        [SerializeField] internal GameObject upButton;
        [SerializeField] internal GameObject downButton;

        void Update()
        {
            rectTransform.sizeDelta = new Vector2(resourcePackList.listRectTransform.sizeDelta.x + resourcePackList.viewportRectTransform.sizeDelta.x - 4, rectTransform.sizeDelta.y);

            if (InputManager.mousePosition.x >= transform.position.x + 2 && InputManager.mousePosition.y >= transform.position.y + 2 && InputManager.mousePosition.x <= transform.position.x + rectTransform.sizeDelta.x - 2 && InputManager.mousePosition.y <= transform.position.y + rectTransform.sizeDelta.y - 2)
            {
                if (!button.activeSelf)
                    button.SetActive(true);
            }
            else
            {
                if (button.activeSelf)
                    button.SetActive(false);
            }

            if (resourcePackList.selected)
            {
                rightButton.SetActive(false);

                if (index != ResourcesManager.ResourcePacks.Count - 1)
                {
                    leftButton.SetActive(true);

                    if (index != 0)
                        upButton.SetActive(true);

                    if (index < ResourcesManager.ResourcePacks.Count - 2)
                        downButton.SetActive(true);
                }
            }
            else
            {
                rightButton.SetActive(true);
                leftButton.SetActive(false);
                upButton.SetActive(false);
                downButton.SetActive(false);
            }

            if (InputManager.mousePosition.x >= transform.position.x && InputManager.mousePosition.y >= transform.position.y && InputManager.mousePosition.x <= transform.position.x + rectTransform.sizeDelta.x && InputManager.mousePosition.y <= transform.position.y + rectTransform.sizeDelta.y)
            {
                if (!button.activeSelf)
                    button.SetActive(true);
            }
            else
            {
                if (button.activeSelf)
                    button.SetActive(false);
            }
        }

        public void OnDestroy()
        {
            gameObject.name = "Default";

            resourcePackList = null;
            resourcePack = null;
            index = 0;
        }

        public void ListMove(bool up)
        {
            if (up)
                Kernel.ListMove(ResourcesManager.ResourcePacks, index, index - 1);
            else
                Kernel.ListMove(ResourcesManager.ResourcePacks, index, index + 1);

            resourcePackList.Reload();
        }

        public void Add()
        {
            ResourcesManager.ResourcePacks.Insert(0, resourcePack);

            ResourcePackList.instance.Reload();
            ResourcePackList.instanceSelected.Reload();
        }

        public void Remove()
        {
            ResourcesManager.ResourcePacks.Remove(resourcePack);

            ResourcePackList.instance.Reload();
            ResourcePackList.instanceSelected.Reload();
        }
    }
}