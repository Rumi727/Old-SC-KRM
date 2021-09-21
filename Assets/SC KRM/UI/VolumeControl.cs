using SCKRM.Input;
using SCKRM.InspectorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [RequireComponent(typeof(RectTransform)), RequireComponent(typeof(Image))]
    [AddComponentMenu("커널/UI/볼륨 컨트롤 바 설정", 0)]
    public class VolumeControl : MonoBehaviour
    {
        [HideInInspector] RectTransform rectTransform;
        [SerializeField] RectTransform sliderRectTransform;
        [SerializeField] Slider slider;
        [SerializeField, SetName("볼륨 퍼센트를 표시할 텍스트")] Text text;

        [Range(0, 1)]
        [SerializeField] float lerpT = 0.15f;
        [Range(0, 100)]
        [SerializeField] int addValue = 10;
        [Range(0, 10)]
        [SerializeField] float hideTimer = 1.5f;

        float timer = float.MaxValue;
        [HideInInspector] float value = 0;

        bool timerReset = false;
        bool sliderOnStay = false;
        bool sliderOnDragStay = false;

        void Update()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            if (sliderOnDragStay && UnityEngine.Input.GetMouseButtonUp(0))
                sliderOnDragStay = false;

            if (!sliderOnDragStay)
            {
                if (InputManager.GetKeyDown("volume_control.volume_down"))
                {
                    Kernel.MainVolume -= addValue;
                    timer = 0;
                }
                else if (InputManager.GetKeyDown("volume_control.volume_up"))
                {
                    Kernel.MainVolume += addValue;
                    timer = 0;
                }
            }

            if (Kernel.MainVolume > 200)
                Kernel.MainVolume = 200;
            else if (Kernel.MainVolume < 0)
                Kernel.MainVolume = 0;

            if (lerpT != 1)
                value = Mathf.Lerp(value, Kernel.MainVolume, lerpT * Kernel.fpsDeltaTime);
            else
                value = Kernel.MainVolume;

            if (!sliderOnDragStay)
                slider.value = value;

            if (timerReset || sliderOnDragStay)
                timer = 0;

            text.text = Mathf.Round(Kernel.MainVolume) + "%";

            if (timer < hideTimer)
            {
                if (!timerReset && !sliderOnDragStay)
                    timer += Kernel.deltaTime;

                if (lerpT != 1)
                    rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(0, -16), lerpT * Kernel.fpsDeltaTime);
                else
                    rectTransform.anchoredPosition = new Vector2(0, -16);
            }
            else
            {
                if (lerpT != 1)
                    rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(0, rectTransform.sizeDelta.y + 1), lerpT * Kernel.fpsDeltaTime);
                else
                    rectTransform.anchoredPosition = new Vector2(0, rectTransform.sizeDelta.y + 1);
            }
        }

        public void SetVolume()
        {
            if (!sliderOnDragStay)
                sliderOnDragStay = sliderOnStay && UnityEngine.Input.GetMouseButton(0);

            if (sliderOnDragStay)
            {
                sliderOnDragStay = true;
                value = slider.value;
                Kernel.MainVolume = (int)slider.value;
            }
        }
        
        public void SliderOnEnter() => sliderOnStay = true;
        public void SliderOnExit() => sliderOnStay = false;

        public void TimerResetOn() => timerReset = true;
        public void TimerResetOff() => timerReset = false;
    }
}