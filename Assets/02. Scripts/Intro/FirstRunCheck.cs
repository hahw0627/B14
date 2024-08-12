using System;
using UnityEngine;

public class FirstRunCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject _introCanvas;
    
    private const string FIRST_RUN_KEY = "FirstRun";

    [NonSerialized]
    public static bool IsFirstRun;
    
    private void Awake()
    {
        Debug.Log($"최초 실행 여부: {!PlayerPrefs.HasKey(FIRST_RUN_KEY)}");
        // PlayerPrefs에서 'FirstRun' 키 확인
        if (!PlayerPrefs.HasKey(FIRST_RUN_KEY))
        {
            // 처음 실행인 경우
            IsFirstRun = true;
            Time.timeScale = 0f;
            _introCanvas.SetActive(true);
            Debug.Log("인트로: 게임을 처음 실행합니다!");
        }
        else
        {
            // 이미 실행된 경우
            IsFirstRun = false;
            Destroy(_introCanvas);
            Debug.Log("인트로: 게임을 이미 실행했습니다.");   
        }
    }

    public static void SaveKeyOfFirstRun()
    {
        PlayerPrefs.SetInt(FIRST_RUN_KEY, 1);
        PlayerPrefs.Save();
    }
    
    public void InitFirstRun()
    {
        PlayerPrefs.DeleteKey(FIRST_RUN_KEY);
    }
}