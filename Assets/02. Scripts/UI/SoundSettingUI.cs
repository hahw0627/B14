using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    public Slider bgmSlider, sfxSldier, masterSlider;

    private void Awake()
    {
        bgmSlider.value = SoundManager.Instance.GetAudioSource(Define.Sound.Bgm).volume;
        sfxSldier.value = SoundManager.Instance.GetAudioSource(Define.Sound.Effect).volume;
        
    }

    public void BGM_Volume()
    {
        if (bgmSlider.value < bgmSlider.minValue)
        {
            bgmSlider.value = bgmSlider.minValue;
        }
        SoundManager.Instance.SetVolume(Define.Sound.Bgm,bgmSlider.value);
    }
    public void SFX_Volume()
    {
        if (sfxSldier.value < sfxSldier.minValue)
        {
            sfxSldier.value = sfxSldier.minValue;
        }
        SoundManager.Instance.SetVolume(Define.Sound.Effect, sfxSldier.value);
    }
    public void MasterVolume()
    {
        if (masterSlider.value < masterSlider.minValue)
        {
            masterSlider.value = masterSlider.minValue;
        }
        SoundManager.Instance.SetVolume(Define.Sound.Master, masterSlider.value);
    }

}
