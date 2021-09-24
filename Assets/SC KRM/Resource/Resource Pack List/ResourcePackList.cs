using SCKRM.Input;
using SCKRM.InspectorEditor;
using SCKRM.Json;
using SCKRM.Object;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace SCKRM.Resources.UI
{
    [AddComponentMenu("커널/Resources/리소스팩 리스트/리소스팩 리스트", 1)]
    public class ResourcePackList : MonoBehaviour
    {
        IEnumerator reloadCoroutine;

        [SerializeField] internal RectTransform listRectTransform;
        [SerializeField] internal RectTransform viewportRectTransform;
        
        public static ResourcePackList instance;
        public static ResourcePackList instanceSelected;

        [SerializeField, SetName("적용된 리소스팩을 보여주는 리스트이면 활성화")] internal bool selected = false;

        void OnEnable() => Reload();

        public void Reload()
        {
            if (reloadCoroutine != null)
                StopCoroutine(reloadCoroutine);
            
            reloadCoroutine = reload();
            StartCoroutine(reloadCoroutine);
        }

        IEnumerator reload()
        {
            ResourcePackButton[] child = GetComponentsInChildren<ResourcePackButton>();
            for (int i = 0; i < child.Length; i++)
            {
                ResourcePackButton item = child[i];
                ObjectPoolingSystem.ObjectRemove("resource_pack_list.resource_pack_button", item.gameObject, item.OnDestroy);
            }

            if (!selected)
            {
                instance = this;
                
                string[] directory = Directory.GetDirectories(Path.Combine(Kernel.persistentDataPath, "Resource Pack"));
                for (int i = 0; i < directory.Length; i++)
                {
                    string item = directory[i].Replace("\\", "/");

                    bool forContinue = false;

                    for (int j = 0; j < ResourcesManager.ResourcePacks.Count; j++)
                    {
                        if (ResourcesManager.ResourcePacks[j].Path == item)
                        {
                            forContinue = true;
                            break;
                        }
                    }

                    if (forContinue)
                        continue;

                    ResourcePackButton button = ObjectPoolingSystem.ObjectCreate("resource_pack_list.resource_pack_button", transform).GetComponent<ResourcePackButton>();
                    button.resourcePackList = this;
                    button.resourcePack = new ResourcePack();

                    button.resourcePack.Path = item;

                    if (Directory.Exists(Path.Combine(item, "assets")))
                    {
                        List<string> nameSpace = new List<string>();
                        string[] di = Directory.GetDirectories(Path.Combine(item, "assets"));
                        for (int j = 0; j < di.Length; j++)
                        {
                            string item2 = di[j].Replace("\\", "/");
                            nameSpace.Add(item2.Remove(0, item2.LastIndexOf("/") + 1));
                        }

                        button.resourcePack.NameSpace = nameSpace.ToArray();
                    }

                    button.icon.sprite = ResourcesManager.Search<Sprite>(Path.Combine(item, "pack"), "", false);

                    if (JsonManager.JsonReadDictionary("name", Path.Combine(item, "pack.json"), out string name, false))
                        button.resourcePack.Name = name;

                    if (JsonManager.JsonReadDictionary("description", Path.Combine(item, "pack.json"), out string description, false))
                        button.resourcePack.Description = description;

                    button.gameObject.name = button.resourcePack.Name.EnvironmentVariable();

                    button.name.text = button.resourcePack.Name.EnvironmentVariable();
                    button.description.text = button.resourcePack.Description.EnvironmentVariable();

                    if (selected)
                    {
                        button.rightButton.SetActive(false);

                        if (button.index != ResourcesManager.ResourcePacks.Count - 1)
                        {
                            button.leftButton.SetActive(true);

                            if (button.index != 0)
                                button.upButton.SetActive(true);

                            if (button.index < ResourcesManager.ResourcePacks.Count - 2)
                                button.downButton.SetActive(true);
                        }
                    }
                    else
                    {
                        button.rightButton.SetActive(true);
                        button.leftButton.SetActive(false);
                        button.upButton.SetActive(false);
                        button.downButton.SetActive(false);
                    }

                    if (InputManager.mousePosition.x >= transform.position.x && InputManager.mousePosition.y >= transform.position.y && InputManager.mousePosition.x <= transform.position.x + button.rectTransform.sizeDelta.x && InputManager.mousePosition.y <= transform.position.y + button.rectTransform.sizeDelta.y)
                    {
                        if (!button.button.activeSelf)
                            button.button.SetActive(true);
                    }
                    else
                    {
                        if (button.button.activeSelf)
                            button.button.SetActive(false);
                    }

                    yield return null;
                }
            }
            else
            {
                instanceSelected = this;

                for (int i = 0; i < ResourcesManager.ResourcePacks.Count; i++)
                {
                    ResourcePack item = ResourcesManager.ResourcePacks[i];

                    ResourcePackButton button = ObjectPoolingSystem.ObjectCreate("resource_pack_list.resource_pack_button", transform).GetComponent<ResourcePackButton>();
                    button.resourcePackList = this;
                    button.resourcePack = item;

                    button.index = i;

                    button.gameObject.name = item.Name.EnvironmentVariable();

                    button.icon.sprite = ResourcesManager.Search<Sprite>(Path.Combine(item.Path, "pack"), "", false);
                    button.name.text = item.Name.EnvironmentVariable();
                    button.description.text = item.Description.EnvironmentVariable();

                    if (selected)
                    {
                        button.rightButton.SetActive(false);

                        if (button.index != ResourcesManager.ResourcePacks.Count - 1)
                        {
                            button.leftButton.SetActive(true);

                            if (button.index != 0)
                                button.upButton.SetActive(true);

                            if (button.index < ResourcesManager.ResourcePacks.Count - 2)
                                button.downButton.SetActive(true);
                        }
                    }
                    else
                    {
                        button.rightButton.SetActive(true);
                        button.leftButton.SetActive(false);
                        button.upButton.SetActive(false);
                        button.downButton.SetActive(false);
                    }

                    if (InputManager.mousePosition.x >= transform.position.x && InputManager.mousePosition.y >= transform.position.y && InputManager.mousePosition.x <= transform.position.x + button.rectTransform.sizeDelta.x && InputManager.mousePosition.y <= transform.position.y + button.rectTransform.sizeDelta.y)
                    {
                        if (!button.button.activeSelf)
                            button.button.SetActive(true);
                    }
                    else
                    {
                        if (button.button.activeSelf)
                            button.button.SetActive(false);
                    }

                    yield return null;
                }
            }
        }
    }
}