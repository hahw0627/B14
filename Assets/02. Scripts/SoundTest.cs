using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{

    public void Click()
    {

        SoundManager.Instance.Play("background1", Define.Sound.Bgm);
        SoundManager.Instance.Play("boss_dies");
    }
}
