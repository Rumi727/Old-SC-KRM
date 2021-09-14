using SCKRM.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Loading
{
    [AddComponentMenu("커널/로딩/로딩 바", 1)]
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField] Image image;
        public Text text;

        [Range(0, 1)] public float lerpT = 1;
        [HideInInspector] public float value = 0;

        void Update()
        {
            if (lerpT != 0 && lerpT != 1)
            {
                image.fillAmount = Mathf.Lerp(image.fillAmount, value, lerpT * Kernel.FPSDeltaTime);
                if (image.fillAmount > 0.999f)
                    ObjectPoolingSystem.ObjectRemove("Loading Bar", gameObject, OnDestroy);
            }
            else
            {
                image.fillAmount = value;
                if (image.fillAmount >= 1)
                    ObjectPoolingSystem.ObjectRemove("Loading Bar", gameObject, OnDestroy);
            }
        }

        public void OnDestroy()
        {
            value = 0;
            image.fillAmount = 0;
        }
    }
}