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
        public static float FPS { get; private set; } = 60;

        public static float DeltaTime { get; private set; } = FPS60second;
        public static float FPSDeltaTime { get; private set; } = 1;
        public static float UnscaledDeltaTime { get; private set; } = FPS60second;
        public static float FPSUnscaledDeltaTime { get; private set; } = 1;

        public const float FPS60second = 1f / 60f;

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
                Destroy(gameObject);
        }

        void Update()
        {
            FPS = 1f / DeltaTime;
            DeltaTime = Time.deltaTime;
            FPSDeltaTime = DeltaTime * 60;
            UnscaledDeltaTime = Time.unscaledDeltaTime;
            FPSUnscaledDeltaTime = UnscaledDeltaTime * 60;

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
            value = value.Replace("%ProductName%", Application.productName);
            value = value.Replace("%Version%", Application.version);

            return value;
        }
    }
}