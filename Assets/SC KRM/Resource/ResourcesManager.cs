using Newtonsoft.Json;
using SCKRM.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SCKRM.Resources
{
    [AddComponentMenu("커널/Resources/리소스 매니저", 0)]
    public class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager instance { get; private set; }

        void Awake()
        {
            if (instance != null)
                Destroy(gameObject);

            instance = this;

            if (!Directory.Exists(Path.Combine(Kernel.persistentDataPath, "Resource Pack")))
                Directory.CreateDirectory(Path.Combine(Kernel.persistentDataPath, "Resource Pack"));
        }

        /// <summary>
        /// If you don't know what you're doing, don't modify this variable in your script
        /// </summary>
        public static List<ResourcePack> ResourcePacks { get; } = new List<ResourcePack>() { ResourcePack.Default };

        //public static Dictionary<string, AudioClip> AudioList { get; } = new Dictionary<string, AudioClip>();

        /// <summary>
        /// 리소스팩에서 리소스를 검색하고 반환합니다
        /// </summary>
        /// <typeparam name="ResourceType">리소스 타입</typeparam>
        /// <param name="path">경로</param>
        /// <param name="nameSpace"></param>
        /// <param name="resourcePackPath"></param>
        /// <returns></returns>
        public static ResourceType Search<ResourceType>(string path, string nameSpace = "", bool resourcePackPath = true)
        {
            if (resourcePackPath)
            {
                NameSpaceAndPath nameSpaceAndPath = GetNameSpaceAndPath(path);
                string selectedNameSpace;
                if (nameSpace == "")
                    selectedNameSpace = nameSpaceAndPath.NameSpace;
                else
                    selectedNameSpace = nameSpace;
                path = nameSpaceAndPath.Path;

                string allPath;

                for (int i = 0; i < ResourcePacks.Count; i++)
                {
                    ResourcePack resourcePack = ResourcePacks[i];

                    if (resourcePack.NameSpace.Contains(selectedNameSpace))
                    {
                        allPath = Path.Combine(resourcePack.Path, path).Replace("\\", "/").Replace("%NameSpace%", selectedNameSpace);

                        if (typeof(ResourceType) == typeof(Texture2D) || typeof(ResourceType) == typeof(Sprite))
                        {
                            allPath += ".png";

                            if (File.Exists(allPath))
                            {
                                byte[] bytes = File.ReadAllBytes(allPath);

                                if (bytes.Length > 0)
                                {
                                    Texture2D texture = new Texture2D(0, 0);
                                    texture.LoadImage(bytes);

                                    SpriteJsonSetting spriteJsonSetting = new SpriteJsonSetting();
                                    if (File.Exists(allPath + ".json"))
                                    {
                                        string json = File.ReadAllText(allPath + ".json");
                                        spriteJsonSetting = JsonConvert.DeserializeObject<SpriteJsonSetting>(json);
                                    }

                                    Rect rect = JRect.JRectToRect(spriteJsonSetting.rect);
                                    Vector2 pivot = JVector2.JVector3ToVector3(spriteJsonSetting.pivot);
                                    Vector4 border = JVector4.JVector4ToVector4(spriteJsonSetting.border);

                                    texture.filterMode = spriteJsonSetting.filterMode;

                                    if (typeof(ResourceType) == typeof(Texture2D))
                                        return (ResourceType)Convert.ChangeType(texture, typeof(ResourceType));
                                    else if (typeof(ResourceType) == typeof(Sprite))
                                    {
                                        if (rect.x == -1)
                                            rect = new Rect(0, 0, texture.width, texture.height);
                                        if (pivot.x == -1)
                                            pivot = new Vector2(0.5f, 0.5f);

                                        if (rect.width < 1)
                                            rect.width = 1;
                                        if (rect.width > texture.width)
                                            rect.width = texture.width;
                                        if (rect.height < 1)
                                            rect.height = 1;
                                        if (rect.height > texture.height)
                                            rect.height = texture.height;

                                        if (rect.x < 0)
                                            rect.x = 0;
                                        if (rect.x > texture.width - rect.width)
                                            rect.x = texture.width - rect.width;
                                        if (rect.y < 0)
                                            rect.y = 0;
                                        if (rect.y > texture.height - rect.height)
                                            rect.y = texture.height - rect.height;

                                        if (spriteJsonSetting.pivot.x < 0)
                                            spriteJsonSetting.pivot.x = 0;
                                        if (spriteJsonSetting.pivot.y < 0)
                                            spriteJsonSetting.pivot.y = 0;
                                        if (spriteJsonSetting.pivot.x > 1)
                                            spriteJsonSetting.pivot.x = 1;
                                        if (spriteJsonSetting.pivot.y > 1)
                                            spriteJsonSetting.pivot.y = 1;

                                        Sprite sprite = Sprite.Create(texture, rect, pivot, spriteJsonSetting.pixelsPerUnit, 1, SpriteMeshType.Tight, border);
                                        return (ResourceType)Convert.ChangeType(sprite, typeof(ResourceType));
                                    }
                                }
                            }
                        }
                        else if (typeof(ResourceType) == typeof(AudioClip))
                        {
                            AudioClip audioClip = null;

                            /*if (AudioList.ContainsKey(path.Replace("%NameSpace%", selectedNameSpace)))
                                audioClip = AudioList[path.Replace("%NameSpace%", selectedNameSpace)];
                            else
                            {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                                if (File.Exists(allPath + ".ogg"))
                                    audioClip = new WWW(allPath + ".ogg").GetAudioClip(false, true, AudioType.OGGVORBIS);
                                else if (File.Exists(allPath + ".mp3"))
                                    audioClip = new WWW(allPath + ".mp3").GetAudioClip(false, true, AudioType.MPEG);
                                else if (File.Exists(allPath + ".wav"))
                                    audioClip = new WWW(allPath + ".wav").GetAudioClip(false, true, AudioType.WAV);
                                else if (File.Exists(allPath + ".aif"))
                                    audioClip = new WWW(allPath + ".aif").GetAudioClip(false, true, AudioType.AIFF);
                                else if (File.Exists(allPath + ".xm"))
                                    audioClip = new WWW(allPath + ".xm").GetAudioClip(false, true, AudioType.XM);
                                else if (File.Exists(allPath + ".mod"))
                                    audioClip = new WWW(allPath + ".mod").GetAudioClip(false, true, AudioType.MOD);
                                else if (File.Exists(allPath + ".it"))
                                    audioClip = new WWW(allPath + ".it").GetAudioClip(false, true, AudioType.IT);
                                else if (File.Exists(allPath + ".s3m"))
                                    audioClip = new WWW(allPath + ".s3m").GetAudioClip(false, true, AudioType.S3M);

                                if (audioClip != null)
                                    AudioList.Add(path.Replace("%NameSpace%", selectedNameSpace), audioClip);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                            }*/

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                            if (File.Exists(allPath + ".ogg"))
                                audioClip = new WWW(allPath + ".ogg").GetAudioClip(false, true, AudioType.OGGVORBIS);
                            else if (File.Exists(allPath + ".mp3"))
                                audioClip = new WWW(allPath + ".mp3").GetAudioClip(false, true, AudioType.MPEG);
                            else if (File.Exists(allPath + ".wav"))
                                audioClip = new WWW(allPath + ".wav").GetAudioClip(false, true, AudioType.WAV);
                            else if (File.Exists(allPath + ".aif"))
                                audioClip = new WWW(allPath + ".aif").GetAudioClip(false, true, AudioType.AIFF);
                            else if (File.Exists(allPath + ".xm"))
                                audioClip = new WWW(allPath + ".xm").GetAudioClip(false, true, AudioType.XM);
                            else if (File.Exists(allPath + ".mod"))
                                audioClip = new WWW(allPath + ".mod").GetAudioClip(false, true, AudioType.MOD);
                            else if (File.Exists(allPath + ".it"))
                                audioClip = new WWW(allPath + ".it").GetAudioClip(false, true, AudioType.IT);
                            else if (File.Exists(allPath + ".s3m"))
                                audioClip = new WWW(allPath + ".s3m").GetAudioClip(false, true, AudioType.S3M);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                            if (audioClip != null)
                                return (ResourceType)Convert.ChangeType(audioClip, typeof(ResourceType));
                        }
                        else if (typeof(ResourceType) == typeof(string))
                        {
                            allPath += ".json";
                            if (File.Exists(allPath))
                            {
                                StreamReader sr = new StreamReader(allPath);
                                string text = sr.ReadToEnd();
                                sr.Close();

                                return (ResourceType)Convert.ChangeType(text, typeof(string));
                            }
                        }
                    }
                }
            }
            else
            {
                if (typeof(ResourceType) == typeof(Texture2D) || typeof(ResourceType) == typeof(Sprite))
                {
                    path = path.Replace("\\", "/");
                    path += ".png";

                    if (File.Exists(path))
                    {
                        byte[] bytes = File.ReadAllBytes(path);

                        if (bytes.Length > 0)
                        {
                            Texture2D texture = new Texture2D(0, 0);
                            texture.LoadImage(bytes);

                            SpriteJsonSetting spriteJsonSetting = new SpriteJsonSetting();
                            if (File.Exists(path + ".json"))
                            {
                                string json = File.ReadAllText(path + ".json");
                                spriteJsonSetting = JsonConvert.DeserializeObject<SpriteJsonSetting>(json);
                            }

                            Rect rect = JRect.JRectToRect(spriteJsonSetting.rect);
                            Vector2 pivot = JVector2.JVector3ToVector3(spriteJsonSetting.pivot);
                            Vector4 border = JVector4.JVector4ToVector4(spriteJsonSetting.border);

                            texture.filterMode = spriteJsonSetting.filterMode;

                            if (typeof(ResourceType) == typeof(Texture2D))
                                return (ResourceType)Convert.ChangeType(texture, typeof(ResourceType));
                            else if (typeof(ResourceType) == typeof(Sprite))
                            {
                                if (rect.x == -1)
                                    rect = new Rect(0, 0, texture.width, texture.height);
                                if (pivot.x == -1)
                                    pivot = new Vector2(0.5f, 0.5f);

                                if (rect.width < 1)
                                    rect.width = 1;
                                if (rect.width > texture.width)
                                    rect.width = texture.width;
                                if (rect.height < 1)
                                    rect.height = 1;
                                if (rect.height > texture.height)
                                    rect.height = texture.height;

                                if (rect.x < 0)
                                    rect.x = 0;
                                if (rect.x > texture.width - rect.width)
                                    rect.x = texture.width - rect.width;
                                if (rect.y < 0)
                                    rect.y = 0;
                                if (rect.y > texture.height - rect.height)
                                    rect.y = texture.height - rect.height;

                                if (spriteJsonSetting.pivot.x < 0)
                                    spriteJsonSetting.pivot.x = 0;
                                if (spriteJsonSetting.pivot.y < 0)
                                    spriteJsonSetting.pivot.y = 0;
                                if (spriteJsonSetting.pivot.x > 1)
                                    spriteJsonSetting.pivot.x = 1;
                                if (spriteJsonSetting.pivot.y > 1)
                                    spriteJsonSetting.pivot.y = 1;

                                Sprite sprite = Sprite.Create(texture, rect, pivot, spriteJsonSetting.pixelsPerUnit, 1, SpriteMeshType.Tight, border);
                                return (ResourceType)Convert.ChangeType(sprite, typeof(ResourceType));
                            }
                        }
                    }
                }
                else if (typeof(ResourceType) == typeof(AudioClip))
                {
                    AudioClip audioClip = null;

                    /*if (AudioList.ContainsKey(path.Replace("%NameSpace%", nameSpace)))
                        audioClip = AudioList[path.Replace("%NameSpace%", nameSpace)];
                    else
                    {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                        if (File.Exists(path + ".ogg"))
                            audioClip = new WWW(path + ".ogg").GetAudioClip(false, true, AudioType.OGGVORBIS);
                        else if (File.Exists(path + ".mp3"))
                            audioClip = new WWW(path + ".mp3").GetAudioClip(false, true, AudioType.MPEG);
                        else if (File.Exists(path + ".wav"))
                            audioClip = new WWW(path + ".wav").GetAudioClip(false, true, AudioType.WAV);
                        else if (File.Exists(path + ".aif"))
                            audioClip = new WWW(path + ".aif").GetAudioClip(false, true, AudioType.AIFF);
                        else if (File.Exists(path + ".xm"))
                            audioClip = new WWW(path + ".xm").GetAudioClip(false, true, AudioType.XM);
                        else if (File.Exists(path + ".mod"))
                            audioClip = new WWW(path + ".mod").GetAudioClip(false, true, AudioType.MOD);
                        else if (File.Exists(path + ".it"))
                            audioClip = new WWW(path + ".it").GetAudioClip(false, true, AudioType.IT);
                        else if (File.Exists(path + ".s3m"))
                            audioClip = new WWW(path + ".s3m").GetAudioClip(false, true, AudioType.S3M);

                        if (audioClip != null)
                            AudioList.Add(path.Replace("%NameSpace%", nameSpace), audioClip);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                    }*/

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                    if (File.Exists(path + ".ogg"))
                        audioClip = new WWW(path + ".ogg").GetAudioClip(false, true, AudioType.OGGVORBIS);
                    else if (File.Exists(path + ".mp3"))
                        audioClip = new WWW(path + ".mp3").GetAudioClip(false, true, AudioType.MPEG);
                    else if (File.Exists(path + ".wav"))
                        audioClip = new WWW(path + ".wav").GetAudioClip(false, true, AudioType.WAV);
                    else if (File.Exists(path + ".aif"))
                        audioClip = new WWW(path + ".aif").GetAudioClip(false, true, AudioType.AIFF);
                    else if (File.Exists(path + ".xm"))
                        audioClip = new WWW(path + ".xm").GetAudioClip(false, true, AudioType.XM);
                    else if (File.Exists(path + ".mod"))
                        audioClip = new WWW(path + ".mod").GetAudioClip(false, true, AudioType.MOD);
                    else if (File.Exists(path + ".it"))
                        audioClip = new WWW(path + ".it").GetAudioClip(false, true, AudioType.IT);
                    else if (File.Exists(path + ".s3m"))
                        audioClip = new WWW(path + ".s3m").GetAudioClip(false, true, AudioType.S3M);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                    if (audioClip != null)
                        return (ResourceType)Convert.ChangeType(audioClip, typeof(ResourceType));
                }
                else if (typeof(ResourceType) == typeof(string))
                {
                    path += ".json";
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path);
                        string text = sr.ReadToEnd();
                        sr.Close();

                        return (ResourceType)Convert.ChangeType(text, typeof(string));
                    }
                }
            }

            return default(ResourceType);
        }

        public static NameSpaceAndPath GetNameSpaceAndPath(string value)
        {
            if (value.Contains(":"))
                return new NameSpaceAndPath() { NameSpace = value.Substring(0, value.IndexOf(":")), Path = value.Remove(0, value.IndexOf(":") + 1) };
            else
                return new NameSpaceAndPath() { NameSpace = ResourcePack.DefaultNameSpace, Path = value };
        }

        public static void OpenResourcePackFolder() => Process.Start("explorer.exe", Path.Combine(Kernel.persistentDataPath, "Resource Pack").Replace("/", "\\"));
    }

    public class SpriteJsonSetting
    {
        [JsonProperty("Filter Mode")] public FilterMode filterMode = FilterMode.Bilinear;
        [JsonProperty("Rect")] public JRect rect = new JRect(-1);
        [JsonProperty("Pivot")] public JVector2 pivot = new JVector2(-1);
        [JsonProperty("Pixels Per Unit")] public int pixelsPerUnit = 100;
        [JsonProperty("Border")] public JVector4 border = new JVector4(-1);
    }

    [System.Serializable]
    public class ResourcePack
    {
        public static ResourcePack Default { get; } = new ResourcePack() { NameSpace = new string[] { "sc-krm" }, Name = "Default", Description = "The default look and feel of %ProductName%" };
        public const string DefaultNameSpace = "sc-krm";

        public const string TexturePath = "assets/%NameSpace%/textures/";
        public const string GuiPath = "assets/%NameSpace%/textures/gui/";

        public const string BGMPath = "assets/%NameSpace%/bgm/";
        public const string SoundPath = "assets/%NameSpace%/sound/";

        public const string MaterialPath = "assets/%NameSpace%/material/";

        public const string LanguagePath = "assets/%NameSpace%/lang/";

        public string[] NameSpace = { "sc-krm" };
        public string Path = Kernel.streamingAssetsPath;
        
        public string Name = "";
        public string Description = "";
    }

    public class NameSpaceAndPath
    {
        public string NameSpace = ResourcePack.DefaultNameSpace;
        public string Path = "";
    }
}
