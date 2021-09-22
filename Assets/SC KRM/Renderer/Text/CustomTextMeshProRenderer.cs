using SCKRM.Language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SCKRM.Renderer
{
    [RequireComponent(typeof(TextMeshPro))]
    [AddComponentMenu("커널/Renderer/Text/TextMeshPro - Text", 0)]
    public class CustomTextMeshProRenderer : CustomText
    {
        TextMeshPro textMeshPro;

        public override void Rerender()
        {
            if (textMeshPro == null)
                textMeshPro = GetComponent<TextMeshPro>();

            if (textMeshPro == null)
                return;

            textMeshPro.text = LanguageManager.LanguageLoad(jsonKey).EnvironmentVariable();
        }
    }
}