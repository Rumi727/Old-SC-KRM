using SCKRM.Input;
using SCKRM.Language;
using SCKRM.Loading;
using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.Resources;
using SCKRM.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    [AddComponentMenu("커널/커널", 0)]
    public class Kernel : MonoBehaviour
    {
        static IEnumerator allRefreshCoroutine;
        static LoadingBar loadingBar;

        public static Kernel instance;
        public static float fps { get; private set; } = 60;

        public static float deltaTime { get; private set; } = fps60second;
        public static float fpsDeltaTime { get; private set; } = 1;
        public static float unscaledDeltaTime { get; private set; } = fps60second;
        public static float fpsUnscaledDeltaTime { get; private set; } = 1;

        public const float fps60second = 1f / 60f;



        public static string dataPath { get; private set; }
        public static string streamingAssetsPath { get; } = Application.streamingAssetsPath;
        public static string persistentDataPath { get; private set; }

        public static string companyName { get; private set; }
        public static string productName { get; private set; }
        public static string version { get; private set; }

        public static RuntimePlatform platform { get; } = Application.platform;



        public static bool isAFK { get; private set; } = false;



        public const int fpsLimit = 300;
        public const int notFocusFpsLimit = 30;
        public const int afkFpsLimit = 15;



        public static float afkTimer { get; private set; } = 0;
        public const float afkTimerLimit = 60;



        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
                Destroy(gameObject);

            dataPath = Application.dataPath;
            persistentDataPath = Application.persistentDataPath;

            companyName = Application.companyName;
            productName = Application.productName;
            version = Application.version;
        }

        void Update()
        {
            fps = 1f / deltaTime;
            deltaTime = Time.deltaTime;
            fpsDeltaTime = deltaTime * 60;
            unscaledDeltaTime = Time.unscaledDeltaTime;
            fpsUnscaledDeltaTime = unscaledDeltaTime * 60;

            //FPS Limit
            if (!isAFK && Application.isFocused)
                Application.targetFrameRate = fpsLimit;
            else if (!isAFK && !Application.isFocused)
                Application.targetFrameRate = notFocusFpsLimit;
            else
                Application.targetFrameRate = afkFpsLimit;



            //AFK
            if (InputManager.anyKeyDown)
                afkTimer = 0;

            if (afkTimer >= afkTimerLimit)
                isAFK = true;
            else
            {
                isAFK = false;
                afkTimer += deltaTime;
            }



#if !UNITY_EDITOR
            if (InputManager.GetKeyDown("Full Screen"))
            {
                if (Screen.fullScreen)
                    Screen.SetResolution((int)(Screen.currentResolution.width / 1.5f), (int)(Screen.currentResolution.height / 1.5f), false);
                else
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
#endif
        }

        public static void ListMove<T>(List<T> ts, int index, int moveIndex)
        {
            T temp = ts[moveIndex];
            ts[moveIndex] = ts[index];
            ts[index] = temp;
        }

        public static void AllRefresh(bool coroutine = true, bool onlyText = false)
        {
            if (allRefreshCoroutine != null)
                instance.StopCoroutine(allRefreshCoroutine);

            if (loadingBar != null)
                ObjectPoolingSystem.ObjectRemove("Loading Bar", loadingBar.gameObject, loadingBar.OnDestroy);

            allRefreshCoroutine = allRefresh(coroutine, onlyText);
            instance.StartCoroutine(allRefreshCoroutine);
        }

        static IEnumerator allRefresh(bool coroutine, bool onlyText)
        {
            if (coroutine)
            {
                loadingBar = LoadingBarManager.Create();
                yield return null;
            }

            if (onlyText)
            {
                LanguageManager.LangList.Clear();

                CustomText[] customTextRenderers = FindObjectsOfType<CustomText>(true);
                for (int i = 0; i < customTextRenderers.Length; i++)
                {
                    customTextRenderers[i].Rerender();

                    if (coroutine)
                    {
                        loadingBar.text.text = $"Text Reload... ({i + 1}/{customTextRenderers.Length})";
                        loadingBar.value = i / (float)(customTextRenderers.Length - 1);
                        yield return null;
                    }
                }
            }
            else
            {
                //ResourcesManager.AudioList.Clear();
                LanguageManager.LangList.Clear();

                CustomRenderer[] customRenderers = FindObjectsOfType<CustomRenderer>(true);
                CustomText[] customTextRenderers = FindObjectsOfType<CustomText>(true);
                SoundObject[] soundObjects = SoundManager.instance.GetComponentsInChildren<SoundObject>(true);
                int i;
                int ii;
                for (i = 0; i < customRenderers.Length; i++)
                {
                    customRenderers[i].Rerender();

                    if (coroutine)
                    {
                        loadingBar.text.text = $"Texture Reload... ({i + 1}/{customRenderers.Length})";
                        loadingBar.value = i / (float)(customRenderers.Length - 1 + customTextRenderers.Length - 1 + soundObjects.Length - 1);
                        yield return null;
                    }
                }

                for (ii = 0; ii < customTextRenderers.Length; ii++)
                {
                    customTextRenderers[ii].Rerender();

                    if (coroutine)
                    {
                        loadingBar.text.text = $"Text Reload... ({ii + 1}/{customTextRenderers.Length})";
                        loadingBar.value = (i + ii) / (float)(customRenderers.Length - 1 + customTextRenderers.Length - 1 + soundObjects.Length - 1);
                        yield return null;
                    }
                }

                for (int iii = 0; iii < soundObjects.Length; iii++)
                {
                    soundObjects[iii].Reload();

                    if (coroutine)
                    {
                        loadingBar.text.text = $"Sound Reload... ({iii + 1}/{soundObjects.Length})";
                        loadingBar.value = (i + ii + iii) / (float)(customRenderers.Length - 1 + customTextRenderers.Length - 1 + soundObjects.Length - 1);
                        yield return null;
                    }
                }
            }
        }
    }

    public static class KernelExtensionMethod
    {
        public static string EnvironmentVariable(this string value)
        {
            value = value.Replace("%DataPath%", Kernel.dataPath);
            value = value.Replace("%StreamingAssetsPath%", Kernel.streamingAssetsPath);
            value = value.Replace("%PersistentDataPath%", Kernel.persistentDataPath);

            value = value.Replace("%CompanyName%", Kernel.companyName);
            value = value.Replace("%ProductName%", Kernel.productName);
            value = value.Replace("%Version%", Kernel.version);

            value = value.Replace("%Platform%", Kernel.platform.ToString());

            return value;
        }
    }
}