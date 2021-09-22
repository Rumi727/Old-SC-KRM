using SCKRM.Language;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("커널/Renderer/Text/텍스트 렌더러", 0)]
    public class CustomTextRenderer : CustomText
    {
        Text text;

        public override void Rerender()
        {
            if (text == null)
                text = GetComponent<Text>();

            if (text == null)
                return;

            text.text = LanguageManager.LanguageLoad(jsonKey).EnvironmentVariable();
        }
    }
}