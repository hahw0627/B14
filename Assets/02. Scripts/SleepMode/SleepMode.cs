using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SleepMode : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject sleepModeUI; // 절전 모드 UI 패널 (검은 배경 + 시간 표시)
    public TextMeshProUGUI timeText; // 시간 표시 텍스트

    private bool isSleepModeActive = false;
    bool  isPressing;
    float pressStartTime;
    [SerializeField]
    float requirePressTime = 3.0f; //3초 눌러야 절전모드 해제

    private float lastInputTime;
    [SerializeField]
    private float idleTimeLimit = 1800f; // 30분 = 1800초

    private void Start()
    {
        lastInputTime = Time.time;
    }
    void Update()
    {
        if (isSleepModeActive)
        {
            timeText.text = System.DateTime.Now.ToString("HH:mm:ss");

            if (isPressing && (Time.time - pressStartTime >= requirePressTime))
            {
                DeactivateSleepMode();
            }
        }
        else
        {
            if (Time.time - lastInputTime > idleTimeLimit)
            {
                ActivateSleepMode();
            }
        }
    }

    public void ActivateSleepMode()
    {
        isSleepModeActive = true;
        sleepModeUI.SetActive(true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;  //화면꺼짐 방지
        Screen.brightness = 0.1f;
        Application.targetFrameRate = 15;

    }

    public void DeactivateSleepMode()
    {
        isSleepModeActive = false;
        sleepModeUI.SetActive(false);
        Screen.sleepTimeout = SleepTimeout.SystemSetting; //시스템에 맞춰 설정.
        Screen.brightness = 1f;
        Application.targetFrameRate = 60;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSleepModeActive)
        {
            isPressing = true;
            pressStartTime = Time.time;
        }
        else
        {
            //입력이 있을때마다 갱신.
            lastInputTime = Time.time;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressing)
        {
            isPressing = false;
        }
        else
        {
            lastInputTime = Time.time;
        }
    }
    //exit 은 손가락을 누른채로 화면에서 벗어났을때 작동할수있다. 일반적으로 두가지 방법을 동시에 사용하는게
    //예외처리에 좋다.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPressing)
        {
            isPressing = false;
        }
         else
        {
            lastInputTime = Time.time;
        }
    }
    
    
}
