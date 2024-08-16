using UnityEngine;

public class LoginScene : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.Play("LoginBackground", Define.Sound.Bgm);
    }
}