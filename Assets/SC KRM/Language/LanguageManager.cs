using SCKRM.Json;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SCKRM.Language
{
    [AddComponentMenu("커널/Language/언어 매니저", 0)]
    public class LanguageManager : MonoBehaviour
    {
        public static LanguageManager instance { get; private set; }

        public static string currentLanguage { get; set; } = "en_us";
        public static Dictionary<string, string> LangList { get; } = new Dictionary<string, string>();

        void Awake()
        {
            if (instance != null)
                Destroy(gameObject);

            instance = this;
        }

        /// <summary>
        /// 리소스팩에 있는 언어 파일을 불러옵니다
        /// </summary>
        /// <param name="key">불러올 키</param>
        /// <param name="language">불러올 언어 (비어있을경우엔, 현재 선택된 언어로 불러옴)</param>
        /// <returns></returns>
        public static string LanguageLoad(string key, string language = "")
        {
            if (!LangList.ContainsKey(key))
            {
                if (language == "")
                    language = currentLanguage;

                if (JsonManager.JsonReadDictionary(key, ResourcePack.LanguagePath + language, out string value))
                    LangList.Add(key, value);
                else
                    value = "null";

                return value;
            }
            else
                return LangList[key];
        }
    }
}