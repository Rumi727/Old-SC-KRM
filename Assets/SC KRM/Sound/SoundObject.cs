using SCKRM.Object;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Sound
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("커널/Audio/오디오 재생 오브젝트", 0)]
    public class SoundObject : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        AudioSource _audioSource;
        public AudioSource audioSource { get => _audioSource; }

        /// <summary>
        /// This variable is managed by the Sound Manager script, do not touch it if possible.
        /// </summary>
        #region Variables managed by scripts
        public SoundType soundType { get; set; } = SoundType.All;

        public string nameSpace { get; set; } = "";
        public string path { get; set; } = "";
        public AudioClip clip { get; set; }

        public bool bgm { get; set; }


        [SerializeField, Range(0, 1)] float _volume = 1;
        [SerializeField, Range(0, 3)] float _pitch = 1;
        public float volume { get => _volume; set => _volume = value; }
        public float pitch { get => _pitch; set => _pitch = value; }
        #endregion

        float previousFrameTimer = 0;
        public bool isLooped { get; private set; } = false;
        public bool isEnded { get; private set; } = false;
        public float time { get; private set; } = 0;

        public void Reload()
        {
            if (audioSource == null)
                GetComponent<AudioSource>();

            if (audioSource == null)
                return;

            OnDestroy();
            if (bgm)
            {
                if (clip != null)
                    audioSource.clip = clip;
                else
                    audioSource.clip = ResourcesManager.Search<AudioClip>(ResourcePack.BGMPath + path, nameSpace);
                SoundManager.BGMList.Add(this);
            }
            else
            {
                if (clip != null)
                    audioSource.clip = clip;
                else
                    audioSource.clip = ResourcesManager.Search<AudioClip>(ResourcePack.SoundPath + path, nameSpace);
                SoundManager.SoundList.Add(this);
            }

            SoundManager.AllList.Add(this);
            audioSource.Play();
        }

        void Update()
        {
            if (audioSource == null)
                GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Remove();
                return;
            }

            audioSource.volume = volume * (Kernel.MainVolume * 0.01f);
            audioSource.pitch = pitch * Kernel.gameSpeed;
            time = audioSource.time;

            if (isEnded && !audioSource.loop)
                Remove();
            else if (audioSource.loop)
            {
                isLooped = false;

                if (audioSource.time - previousFrameTimer < 0)
                    isLooped = true;

                previousFrameTimer = audioSource.time;
            }

            if (!audioSource.isPlaying && !audioSource.loop)
                isEnded = true;
        }

        public void Remove() => ObjectPoolingSystem.ObjectRemove("sound_manager.sound_object", gameObject, OnDestroy);

        public void OnDestroy()
        {
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;

            previousFrameTimer = 0;
            isLooped = false;
            isEnded = false;

            if (bgm)
                SoundManager.BGMList.Remove(this);
            else
                SoundManager.SoundList.Remove(this);

            SoundManager.AllList.Remove(this);

            audioSource.Stop();

            if (audioSource.clip != null) //&& !ResourcesManager.AudioList.ContainsValue(audioSource.clip))
                Destroy(audioSource.clip);

            audioSource.clip = null;
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

    [System.Flags]
    public enum SoundType
    {
        All = 0,
        BGM = 1 << 1,
        Sound = 1 << 2
    }
}