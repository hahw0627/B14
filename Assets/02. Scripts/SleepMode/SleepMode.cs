using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SleepMode : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject sleepModeUI; // ���� ��� UI �г� (���� ��� + �ð� ǥ��)
    public TextMeshProUGUI timeText; // �ð� ǥ�� �ؽ�Ʈ

    private bool isSleepModeActive = false;
    bool  isPressing;
    float pressStartTime;
    [SerializeField]
    float requirePressTime = 3.0f; //3�� ������ ������� ����

    private float lastInputTime;
    [SerializeField]
    private float idleTimeLimit = 1800f; // 30�� = 1800��

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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;  //ȭ�鲨�� ����
        Screen.brightness = 0.1f;
        Application.targetFrameRate = 15;

    }

    public void DeactivateSleepMode()
    {
        isSleepModeActive = false;
        sleepModeUI.SetActive(false);
        Screen.sleepTimeout = SleepTimeout.SystemSetting; //�ý��ۿ� ���� ����.
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
            //�Է��� ���������� ����.
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
    //exit �� �հ����� ����ä�� ȭ�鿡�� ������� �۵��Ҽ��ִ�. �Ϲ������� �ΰ��� ����� ���ÿ� ����ϴ°�
    //����ó���� ����.
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
