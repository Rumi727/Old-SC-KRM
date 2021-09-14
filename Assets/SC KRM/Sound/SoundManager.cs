using SCKRM.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SCKRM.Sound
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    [AddComponentMenu("커널/Audio/오디오 매니저", 0)]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance { get; private set; }

        #region Transform
        [SerializeField] internal Transform _BGM;
        [SerializeField] internal Transform _Sound;

        public Transform BGM { get => _BGM; }
        public Transform Sound { get => _Sound; }
        #endregion

        /// <summary>
        /// This variable is managed by the Sound Manager script, do not touch it if possible.
        /// </summary>
        #region List (Variables managed by scripts)
        [Obsolete("This variable is managed by the Sound Manager script, do not touch it if possible.")]
        public static List<SoundObject> AllList { get; } = new List<SoundObject>();
        [Obsolete("This variable is managed by the Sound Manager script, do not touch it if possible.")]
        public static List<SoundObject> BGMList { get; } = new List<SoundObject>();
        [Obsolete("This variable is managed by the Sound Manager script, do not touch it if possible.")]
        public static List<SoundObject> SoundList { get; } = new List<SoundObject>();
        #endregion

        /// <summary>
        /// This is the only variable that can be touched without any knowledge
        /// </summary>
        #region Max
        public const int MaxBGMCount = 16;
        public const int MaxSoundCount = 32;
        #endregion

        void Awake() => instance = this;

        public static SoundObject PlayBGM(SoundType soundType, string path, float volume = 1, bool loop = false, float pitch = 1, bool rhythmPitchUse = false)
        {
            if (BGMList.Count >= MaxBGMCount)
                return null;

            SoundObject soundObject = ObjectPoolingSystem.ObjectCreate("Sound Object", instance.BGM).GetComponent<SoundObject>();
            BGMList.Add(soundObject);

            soundObject.gameObject.name = path;

            soundObject.path = path;

            soundObject.soundType = soundType | SoundType.BGM;
            soundObject.bgm = true;
            soundObject.volume = volume;
            soundObject.pitch = pitch;
            soundObject.audioSource.loop = loop;
            soundObject.rhythmPitchUse = rhythmPitchUse;

            soundObject.Reload();

            return soundObject;
        }

        public static SoundObject PlaySound(SoundType soundType, string path, float volume = 1, float pitch = 1) => PlaySound(soundType, new string[] { path }, volume, pitch);

        static SoundObject PlaySound(SoundType soundType, string[] path, float volume = 1, float pitch = 1)
        {
            if (BGMList.Count >= MaxBGMCount)
                return null;

            int random = UnityEngine.Random.Range(0, path.Length);

            SoundObject soundObject = ObjectPoolingSystem.ObjectCreate("Sound Object", instance.Sound).GetComponent<SoundObject>();
            soundObject.gameObject.name = path[random];

            soundObject.path = path[random];

            soundObject.soundType = soundType | SoundType.Sound;
            soundObject.bgm = false;
            soundObject.volume = volume;
            soundObject.pitch = pitch;
            soundObject.audioSource.loop = false;
            soundObject.rhythmPitchUse = false;

            soundObject.Reload();

            return soundObject;
        }
        
        public static void StopBGM(string path, bool all = false)
        {
            for (int i = 0; i < BGMList.Count; i++)
            {
                SoundObject soundObject = BGMList[i];

                if (soundObject.path == path)
                {
                    ObjectPoolingSystem.ObjectRemove("Sound Object", soundObject.gameObject, soundObject.OnDestroy);
                    if (!all)
                        return;
                }
            }
        }

        public static void StopSound(SoundObject soundObject) => ObjectPoolingSystem.ObjectRemove("Sound Object", soundObject.gameObject, soundObject.OnDestroy);

        public static void StopSound(string path, bool all = false)
        {
            for (int i = 0; i < SoundList.Count; i++)
            {
                SoundObject soundObject = SoundList[i];

                if (soundObject.path == path)
                {
                    ObjectPoolingSystem.ObjectRemove("Sound Object", soundObject.gameObject, soundObject.OnDestroy);
                    if (!all)
                        return;
                }
            }
        }

        public static void StopAll(SoundType soundType)
        {
            if ((soundType & SoundType.BGM) != 0)
            {
                for (int i = 0; i < BGMList.Count; i++)
                {
                    SoundObject soundObject = BGMList[i];
                    ObjectPoolingSystem.ObjectRemove("Sound Object", soundObject.gameObject, soundObject.OnDestroy);
                    i--;
                }
            }
            else if ((soundType & SoundType.Sound) != 0)
            {
                for (int i = 0; i < SoundList.Count; i++)
                {
                    SoundObject soundObject = SoundList[i];
                    ObjectPoolingSystem.ObjectRemove("Sound Object", soundObject.gameObject, soundObject.OnDestroy);
                    i--;
                }
            }
            else
            {
                for (int i = 0; i < AllList.Count; i++)
                {
                    SoundObject soundObject = AllList[i];
                    ObjectPoolingSystem.ObjectRemove("Sound Object", soundObject.gameObject, soundObject.OnDestroy);
                    i--;
                }
            }
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}