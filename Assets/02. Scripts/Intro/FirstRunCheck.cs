using System;
using UnityEngine;

public class FirstRunCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject _introCanvas;

    [NonSerialized]
    public static bool IsFirstRun;

    private const string FIRST_RUN_KEY = "FirstRun";

    private void Awake()
    {
        if (StageManager.Instance.StageDataSO.Stage == 1 && StageManager.Instance.StageDataSO.StagePage == 0)
        {
            FirstRun();
            return;
        }

        // PlayerPrefs에서 'FirstRun' 키 확인
        if (!PlayerPrefs.HasKey(FIRST_RUN_KEY))
        {
            FirstRun();
        }
        else
        {
            // 이미 실행된 경우
            IsFirstRun = false;
            _introCanvas.SetActive(false);
            Debug.Log("<color=white>인트로: 게임을 전에 실행했었습니다.</color>");
        }
    }

    private void FirstRun()
    {
        IsFirstRun = true;
        Time.timeScale = 0f;
        _introCanvas.SetActive(true);
        Debug.Log("<color=white>인트로: 게임을 처음 실행합니다!</color>");
    }

    public static void SaveKeyOfFirstRun()
    {
        PlayerPrefs.SetInt(FIRST_RUN_KEY, 1);
        PlayerPrefs.Save();
    }

    public void InitFirstRun()
    {
        PlayerPrefs.DeleteKey(FIRST_RUN_KEY);
        _introCanvas.SetActive(true);
    }
}