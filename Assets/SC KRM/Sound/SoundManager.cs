using SCKRM.Object;
using SCKRM.Resources;
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

        /// <summary>
        /// BGM을 재생합니다
        /// </summary>
        /// <param name="soundType">타입</param>
        /// <param name="path">경로</param>
        /// <param name="volume">볼륨</param>
        /// <param name="loop">루프</param>
        /// <param name="pitch">피치</param>
        /// <param name="rhythmPitchUse">리듬 매니저에서 사용하는 피치와 연동</param>
        /// <returns></returns>
        public static SoundObject PlayBGM(SoundType soundType, string path, float volume = 1, bool loop = false, float pitch = 1, bool autoStart = true)
        {
            if (BGMList.Count >= MaxBGMCount)
                return null;

            SoundObject soundObject = ObjectPoolingSystem.ObjectCreate("sound_manager.sound_object", instance.BGM).GetComponent<SoundObject>();
            BGMList.Add(soundObject);

            soundObject.gameObject.name = path;

            NameSpaceAndPath nameSpaceAndPath = ResourcesManager.GetNameSpaceAndPath(path);
            soundObject.nameSpace = nameSpaceAndPath.NameSpace;
            soundObject.path = nameSpaceAndPath.Path;

            soundObject.soundType = soundType | SoundType.BGM;
            soundObject.bgm = true;
            soundObject.volume = volume;
            soundObject.pitch = pitch;
            soundObject.audioSource.loop = loop;

            if (autoStart)
                soundObject.Reload();

            return soundObject;
        }

        /// <summary>
        /// BGM을 재생합니다
        /// </summary>
        /// <param name="soundType">타입</param>
        /// <param name="clip">클립</param>
        /// <param name="volume">볼륨</param>
        /// <param name="loop">루프</param>
        /// <param name="pitch">피치</param>
        /// <param name="rhythmPitchUse">리듬 매니저에서 사용하는 피치와 연동</param>
        /// <returns></returns>
        public static SoundObject PlayBGM(SoundType soundType, AudioClip clip, float volume = 1, bool loop = false, float pitch = 1, bool autoStart = true)
        {
            if (BGMList.Count >= MaxBGMCount)
                return null;

            if (clip == null)
                return null;

            SoundObject soundObject = ObjectPoolingSystem.ObjectCreate("sound_manager.sound_object", instance.BGM).GetComponent<SoundObject>();
            BGMList.Add(soundObject);

            soundObject.gameObject.name = clip.ToString();
            soundObject.clip = clip;

            soundObject.soundType = soundType | SoundType.BGM;
            soundObject.bgm = true;
            soundObject.volume = volume;
            soundObject.pitch = pitch;
            soundObject.audioSource.loop = loop;

            if (autoStart)
                soundObject.Reload();

            return soundObject;
        }

        /// <summary>
        /// 효과음을 재생합니다
        /// </summary>
        /// <param name="soundType">타입</param>
        /// <param name="path">경로</param>
        /// <param name="volume">볼륨</param>
        /// <param name="pitch">피치</param>
        /// <returns></returns>
        public static SoundObject PlaySound(SoundType soundType, string path, float volume = 1, float pitch = 1, bool autoStart = true) => PlaySound(soundType, new string[] { path }, volume, pitch, autoStart);

        /// <summary>
        /// 효과음을 재생합니다
        /// </summary>
        /// <param name="soundType">타입</param>
        /// <param name="path">경로 (랜덤 선택)</param>
        /// <param name="volume">볼륨</param>
        /// <param name="pitch">피치</param>
        /// <returns></returns>
        static SoundObject PlaySound(SoundType soundType, string[] path, float volume = 1, float pitch = 1, bool autoStart = true)
        {
            if (BGMList.Count >= MaxBGMCount)
                return null;

            int random = UnityEngine.Random.Range(0, path.Length);

            SoundObject soundObject = ObjectPoolingSystem.ObjectCreate("sound_manager.sound_object", instance.Sound).GetComponent<SoundObject>();
            soundObject.gameObject.name = path[random];

            NameSpaceAndPath nameSpaceAndPath = ResourcesManager.GetNameSpaceAndPath(path[random]);
            soundObject.nameSpace = nameSpaceAndPath.NameSpace;
            soundObject.path = nameSpaceAndPath.Path;

            soundObject.soundType = soundType | SoundType.Sound;
            soundObject.bgm = false;
            soundObject.volume = volume;
            soundObject.pitch = pitch;
            soundObject.audioSource.loop = false;

            if (autoStart)
                soundObject.Reload();

            return soundObject;
        }

        /// <summary>
        /// 효과음을 재생합니다
        /// </summary>
        /// <param name="soundType">타입</param>
        /// <param name="clip">클립</param>
        /// <param name="volume">볼륨</param>
        /// <param name="pitch">피치</param>
        /// <returns></returns>
        public static SoundObject PlaySound(SoundType soundType, AudioClip clip, float volume = 1, float pitch = 1, bool autoStart = true) => PlaySound(soundType, new AudioClip[] { clip }, volume, pitch, autoStart);

        /// <summary>
        /// 효과음을 재생합니다
        /// </summary>
        /// <param name="soundType">타입</param>
        /// <param name="clip">클립 (랜덤 선택)</param>
        /// <param name="volume">볼륨</param>
        /// <param name="pitch">피치</param>
        /// <returns></returns>
        static SoundObject PlaySound(SoundType soundType, AudioClip[] clip, float volume = 1, float pitch = 1, bool autoStart = true)
        {
            if (BGMList.Count >= MaxBGMCount)
                return null;

            int random = UnityEngine.Random.Range(0, clip.Length);

            if (clip[random] == null)
                return null;

            SoundObject soundObject = ObjectPoolingSystem.ObjectCreate("sound_manager.sound_object", instance.Sound).GetComponent<SoundObject>();
            soundObject.gameObject.name = clip[random].ToString();
            soundObject.clip = clip[random];

            soundObject.soundType = soundType | SoundType.Sound;
            soundObject.bgm = false;
            soundObject.volume = volume;
            soundObject.pitch = pitch;
            soundObject.audioSource.loop = false;

            if (autoStart)
                soundObject.Reload();

            return soundObject;
        }

        /// <summary>
        /// BGM을 중지합니다
        /// </summary>
        /// <param name="path">경로</param>
        /// <param name="all">모두 중지</param>
        public static void StopBGM(string path, bool all = false)
        {
            for (int i = 0; i < BGMList.Count; i++)
            {
                SoundObject soundObject = BGMList[i];

                if (soundObject.path == path)
                {
                    soundObject.Remove();
                    if (!all)
                        return;
                }
            }
        }

        /// <summary>
        /// 효과음을 중지합니다
        /// </summary>
        /// <param name="soundObject">중지할 오브젝트</param>
        public static void StopSound(SoundObject soundObject) => soundObject.Remove();

        /// <summary>
        /// 효과음을 중지합니다
        /// </summary>
        /// <param name="path">경로</param>
        /// <param name="all">모두 중지</param>
        public static void StopSound(string path, bool all = false)
        {
            for (int i = 0; i < SoundList.Count; i++)
            {
                SoundObject soundObject = SoundList[i];

                if (soundObject.path == path)
                {
                    soundObject.Remove();
                    if (!all)
                        return;
                }
            }
        }


        /// <summary>
        /// 모든 효과음을 중지합니다
        /// </summary>
        /// <param name="soundType">타입</param>
        public static void StopAll(SoundType soundType)
        {
            if ((soundType & SoundType.BGM) != 0)
            {
                for (int i = 0; i < BGMList.Count; i++)
                {
                    BGMList[i].Remove();
                    i--;
                }
            }
            else if ((soundType & SoundType.Sound) != 0)
            {
                for (int i = 0; i < SoundList.Count; i++)
                {
                    SoundList[i].Remove();
                    i--;
                }
            }
            else
            {
                for (int i = 0; i < AllList.Count; i++)
                {
                    AllList[i].Remove();
                    i--;
                }
            }
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}