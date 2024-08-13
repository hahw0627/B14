using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstRunCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject _introCanvas;
    
    private const string FIRST_RUN_KEY = "FirstRun";

    [NonSerialized]
    public static bool IsFirstRun;
    
    private void Awake()
    {
        // PlayerPrefs에서 'FirstRun' 키 확인
        if (!PlayerPrefs.HasKey(FIRST_RUN_KEY))
        {
            // 처음 실행인 경우
            IsFirstRun = true;
            Time.timeScale = 0f;
            _introCanvas.SetActive(true);
            Debug.Log("<color=white>인트로: 게임을 처음 실행합니다!</color>");
        }
        else
        {
            // 이미 실행된 경우
            IsFirstRun = false;
            Destroy(_introCanvas);
            Debug.Log("<color=white>인트로: 게임을 전에 실행했었습니다.</color>");   
        }
    }

    public static void SaveKeyOfFirstRun()
    {
        PlayerPrefs.SetInt(FIRST_RUN_KEY, 1);
        PlayerPrefs.Save();
    }
    
    public void InitFirstRun()
    {
        DebugConsole.ClearLog();
        PlayerPrefs.DeleteKey(FIRST_RUN_KEY);
        SceneLoader.LoadScene( SceneManager.GetActiveScene().name );
    }
}