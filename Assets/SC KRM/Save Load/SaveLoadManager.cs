using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.Language;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SCKRM.SaveData
{
    [AddComponentMenu("커널/세이브 로드/세이브 로드 매니저", 0)]
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager instance;

        void Awake()
        {
            if (instance != null)
                Destroy(gameObject);

            instance = this;

            LoadData();
        }

        void OnApplicationQuit() => SaveData();

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        public static void SaveData()
        {
            if (!Directory.Exists(Path.Combine(Kernel.persistentDataPath, "Save Data")))
                Directory.CreateDirectory(Path.Combine(Kernel.persistentDataPath, "Save Data"));

            KernelSetting kernelSetting = new KernelSetting();
            string path = Path.Combine(Kernel.persistentDataPath, "Save Data", "Kernel Setting.json");



            kernelSetting.MainVolume = Kernel.MainVolume;
            kernelSetting.Language = LanguageManager.currentLanguage;
            kernelSetting.ResourcePack = new List<ResourcePack>(ResourcesManager.ResourcePacks);
            kernelSetting.ResourcePack.RemoveAt(kernelSetting.ResourcePack.Count - 1);
            kernelSetting.Controls = new Dictionary<string, KeyCode>(InputManager.keyList);
            foreach (var item in InputManager.keyList)
            {
                if (item.Value == KeyCode.Escape)
                    kernelSetting.Controls.Remove(item.Key);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(kernelSetting, Formatting.Indented));
        }

        public static void LoadData()
        {
            if (!Directory.Exists(Path.Combine(Kernel.persistentDataPath, "Save Data")))
                return;

            KernelSetting kernelSetting = new KernelSetting();
            string path = Path.Combine(Kernel.persistentDataPath, "Save Data", "Kernel Setting.json");
            if (File.Exists(path))
                kernelSetting = JsonConvert.DeserializeObject<KernelSetting>(File.ReadAllText(path));

            if (kernelSetting != null)
            {
                Kernel.MainVolume = kernelSetting.MainVolume;
                LanguageManager.currentLanguage = kernelSetting.Language;

                List<ResourcePack> resourcePacks = kernelSetting.ResourcePack;
                ResourcesManager.ResourcePacks.Clear();
                for (int i = 0; i < resourcePacks.Count; i++)
                    ResourcesManager.ResourcePacks.Add(resourcePacks[i]);
                ResourcesManager.ResourcePacks.Add(ResourcePack.Default);

                List<KeyValuePair<string, KeyCode>> list = kernelSetting.Controls.ToList();
                int ii = 0;
                for (int i = 0; i < InputManager.instance._keyList.Count; i++)
                {
                    if (ii >= list.Count)
                        break;

                    KeyValuePair<string, KeyCode> item = list[ii];
                    StringKeyCode stringKeyCode = InputManager.instance._keyList[i];
                    
                    if (item.Key == stringKeyCode.key)
                    {
                        stringKeyCode.value = item.Value;
                        ii++;
                    }
                }

                InputManager._KeyListSaveChanges();
            }

            Kernel.AllRefresh(true);
        }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    }

    class KernelSetting
    {
        [JsonProperty("Main Volume")] public int MainVolume = 100;
        public string Language = "en_us";
        [JsonProperty("Resource Pack")] public List<ResourcePack> ResourcePack = new List<ResourcePack>();
        public Dictionary<string, KeyCode> Controls = new Dictionary<string, KeyCode>();
    }
}