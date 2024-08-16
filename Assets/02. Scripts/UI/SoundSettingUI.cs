using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    public Slider bgmSlider, sfxSldier, masterSlider;

    void Start()
    {
        bgmSlider.value = SoundManager.Instance.GetAudioSource(Define.Sound.Bgm).volume;
        sfxSldier.value = SoundManager.Instance.GetAudioSource(Define.Sound.Effect).volume;
    }

    public void BGM_Volume()
    {
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
    }
    public void SFX_Volume()
    {
        SoundManager.Instance.SetSFXVolume(sfxSldier.value);
    }
    public void MasterVolume()
    {
        SoundManager.Instance.SetMasterVolume(masterSlider.value);
    }

}
