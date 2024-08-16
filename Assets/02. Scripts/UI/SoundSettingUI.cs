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
        masterSlider.value = 0.4f;
    }

    public void BGM_Volume()
    {
        SoundManager.Instance.SetVolume(Define.Sound.Bgm,bgmSlider.value);
    }
    public void SFX_Volume()
    {
        SoundManager.Instance.SetVolume(Define.Sound.Effect, sfxSldier.value);
    }
    public void MasterVolume()
    {
        SoundManager.Instance.SetVolume(Define.Sound.Master, masterSlider.value);
    }

}
