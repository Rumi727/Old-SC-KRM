using SCKRM.Json;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SCKRM.Language
{
    [AddComponentMenu("Ŀ��/Language/��� �Ŵ���", 0)]
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
        /// ���ҽ��ѿ� �ִ� ��� ������ �ҷ��ɴϴ�
        /// </summary>
        /// <param name="key">�ҷ��� Ű</param>
        /// <param name="language">�ҷ��� ��� (���������쿣, ���� ���õ� ���� �ҷ���)</param>
        /// <returns></returns>
        public static string LangLoad(string key, string language = "")
        {
            if (!LangList.ContainsKey(key))
            {
                if (language == "")
                    language = currentLanguage;

                if (JsonManager.JsonRead(key, ResourcePack.LanguagePath + language, out string value))
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