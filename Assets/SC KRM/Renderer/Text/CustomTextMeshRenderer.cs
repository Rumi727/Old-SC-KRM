using SCKRM.Language;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Renderer
{
    [RequireComponent(typeof(TextMesh))]
    [AddComponentMenu("커널/Renderer/Text/텍스트 메시 렌더러", 0)]
    public class CustomTextMeshRenderer : CustomText
    {
        TextMesh textMesh;

        public override void Rerender()
        {
            if (textMesh == null)
                textMesh = GetComponent<TextMesh>();

            if (textMesh == null)
                return;

            textMesh.text = LanguageManager.LanguageLoad(jsonKey).EnvironmentVariable();
        }
    }
}