using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Language.UI
{
    [AddComponentMenu("Ŀ��/Language/��� ���� ��ư", 2)]
    public class LanguageButton : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] internal Text text;
        
        internal string language = "";

        void Update()
        {
            if (LanguageManager.currentLanguage == language)
                image.color = Color.white;
            else
                image.color = Color.clear;
        }

        public void ChangeLanguage()
        {
            LanguageManager.currentLanguage = language;
            image.color = Color.white;
        }

        public void OnDestroy() => language = "";
    }
}