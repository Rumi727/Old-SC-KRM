using SCKRM.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBGM : MonoBehaviour
{
    public InputField playBGMInputField;
    public InputField playSoundInputField;

    public void playBGM() => SoundManager.PlayBGM(SoundType.All, playBGMInputField.text, 0.5f, true);
    public void playSound() => SoundManager.PlaySound(SoundType.All, playSoundInputField.text, 0.5f);
    public void stopAll() => SoundManager.StopAll(SoundType.All);
}
