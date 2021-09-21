using SCKRM.Json;
using SCKRM.Language;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [ExecuteAlways]
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("커널/Text/Text", 0)]
    public class CustomText : MonoBehaviour
    {
        Text text;

        [SerializeField] string _jsonKey = "";
        public string jsonKey { get => _jsonKey; set => _jsonKey = value; }

        public virtual void Rerender()
        {
            if (text == null)
                text = GetComponent<Text>();

            if (text == null)
                return;

            text.text = LanguageManager.LanguageLoad(jsonKey).EnvironmentVariable();
        }
    }
}