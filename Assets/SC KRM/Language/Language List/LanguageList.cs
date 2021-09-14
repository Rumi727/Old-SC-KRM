using SCKRM.Json;
using SCKRM.Object;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SCKRM.Language.UI
{
    [AddComponentMenu("커널/Language/언어 리스트/언어 리스트", 1)]
    public class LanguageList : MonoBehaviour
    {
        IEnumerator reloadCoroutine;

        public static LanguageList instance;

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
            LanguageButton[] child = GetComponentsInChildren<LanguageButton>();
            for (int i = 0; i < child.Length; i++)
            {
                LanguageButton item = child[i];
                ObjectPoolingSystem.ObjectRemove("Language Button", item.gameObject, item.OnDestroy);
            }

            instance = this;

            List<string> languageList = new List<string>();
            for (int packIndex = 0; packIndex < ResourcesManager.ResourcePacks.Count; packIndex++)
            {
                ResourcePack resourcePack = ResourcesManager.ResourcePacks[packIndex];

                for (int nameSpaceIndex = 0; nameSpaceIndex < resourcePack.NameSpace.Length; nameSpaceIndex++)
                {
                    string nameSpace = resourcePack.NameSpace[nameSpaceIndex];
                    
                    if (Directory.Exists(Path.Combine(resourcePack.Path, ResourcePack.LanguagePath).Replace("%NameSpace%", nameSpace)))
                    {
                        string[] directorys = Directory.GetFiles(Path.Combine(resourcePack.Path, ResourcePack.LanguagePath).Replace("%NameSpace%", nameSpace), "*.json");

                        for (int languageIndex = 0; languageIndex < directorys.Length; languageIndex++)
                        {
                            string path = directorys[languageIndex].Replace("\\", "/");
                            string language = path.Substring(path.LastIndexOf("/") + 1, path.LastIndexOf(".") - path.LastIndexOf("/") - 1);

                            if (languageList.Contains(language))
                                continue;

                            LanguageButton button = ObjectPoolingSystem.ObjectCreate("Language Button", transform).GetComponent<LanguageButton>();
                            button.gameObject.name = language;
                            button.language = language;

                            languageList.Add(language);

                            if (JsonManager.JsonRead("language.name", path, out string name, false))
                                button.text.text = name;
                            if (JsonManager.JsonRead("language.region", path, out string region, false))
                                button.text.text += $" ({region})";

                            if (button.text.text == "")
                                button.text.text = language;

                            yield return null;
                        }
                    }
                }
            }
        }
    }
}