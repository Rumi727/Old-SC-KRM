using SCKRM.Language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SCKRM.Renderer
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [AddComponentMenu("커널/Renderer/Text/TextMeshPro - Text (UI)", 0)]
    public class CustomTextMeshProUGUIRenderer : CustomText
    {
        TextMeshProUGUI textMeshProUGUI;

        public override void Rerender()
        {
            if (textMeshProUGUI == null)
                textMeshProUGUI = GetComponent<TextMeshProUGUI>();

            if (textMeshProUGUI == null)
                return;

            textMeshProUGUI.text = LanguageManager.LanguageLoad(jsonKey).EnvironmentVariable();
        }
    }
}