using SCKRM.Input;
using SCKRM.InspectorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("커널/UI/볼륨 컨트롤 바 설정", 0)]
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField, HideInInspector] RectTransform rectTransform;
        [SerializeField, SetName("채우기 양을 바꿀 이미지")] Image image;
        [SerializeField, SetName("볼륨 퍼센트를 표시할 텍스트")] Text text;

        [Range(0, 1)]
        [SerializeField] float lerpT = 0.15f;
        [Range(0, 100)]
        [SerializeField] float addValue = 10f;
        [Range(0, 10)]
        [SerializeField] float hideTimer = 1.5f;

        float timer = float.MaxValue;

        void Update()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            if (InputManager.GetKeyDown("Volume Down"))
            {
                AudioListener.volume -= addValue * 0.01f;
                timer = 0;
            }
            else if (InputManager.GetKeyDown("Volume Up") || InputManager.GetKeyDown("Volume Up2"))
            {
                AudioListener.volume += addValue * 0.01f;
                timer = 0;
            }

            if (AudioListener.volume > 1)
                AudioListener.volume = 1;
            else if (AudioListener.volume < 0)
                AudioListener.volume = 0;

            image.fillAmount = Mathf.Lerp(image.fillAmount, AudioListener.volume, lerpT * Kernel.fpsDeltaTime);
            text.text = Mathf.Round(AudioListener.volume * 100) + "%";

            if (timer < hideTimer)
            {
                timer += Kernel.deltaTime;
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, Vector2.zero, lerpT * Kernel.fpsDeltaTime);
            }
            else
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(rectTransform.sizeDelta.x, 0), lerpT * Kernel.fpsDeltaTime);
        }
    }
}