using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject _introCanvas;
    
    private void Start()
    {
        if (!_introCanvas.activeSelf)
        {
            SoundManager.Instance.Play("MainBackground", type: Define.Sound.Bgm);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}