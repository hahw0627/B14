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
