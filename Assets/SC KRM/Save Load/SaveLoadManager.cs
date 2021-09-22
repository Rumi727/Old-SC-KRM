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

        static IEnumerator AllRefresh()
        {
            yield return new WaitForEndOfFrame();
            Kernel.AllRefresh(true);
        }

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
            kernelSetting.Controls = new Dictionary<string, KeyCode>(InputManager.controlSettingList);
            foreach (var item in InputManager.controlSettingList)
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

                InputManager.SettingFileLoad();
                foreach (var item in kernelSetting.Controls)
                {
                    string key = item.Key;
                    if (InputManager.controlSettingList.ContainsKey(key))
                        InputManager.controlSettingList[key] = kernelSetting.Controls[key];
                }
            }

            instance.StartCoroutine(AllRefresh());
        }
    }

    class KernelSetting
    {
        [JsonProperty("Main Volume")] public int MainVolume = 100;
        public string Language = "en_us";
        [JsonProperty("Resource Pack")] public List<ResourcePack> ResourcePack = new List<ResourcePack>();
        public Dictionary<string, KeyCode> Controls = new Dictionary<string, KeyCode>();
    }
}