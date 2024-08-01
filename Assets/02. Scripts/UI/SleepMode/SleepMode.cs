using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SleepMode : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject sleepModeUI; // ���� ��� UI �г� (���� ��� + �ð� ǥ��)
    public TextMeshProUGUI timeText; // �ð� ǥ�� �ؽ�Ʈ
    
    Image blackImage; // �������� ����ȭ��

    bool isSleepModeActive = false;
    bool  isPressing;
    float pressStartTime;

    [SerializeField]
    float requirePressTime = 3.0f;      //3�� ������ ������� ����
    float lastInputTime;
    [SerializeField]
    float idleTimeLimit = 1800f; // 30�� = 1800��

    Color originColor;
    Coroutine fadeCoroutine;

    private void Start()
    {
        lastInputTime = Time.time;
        blackImage = sleepModeUI.GetComponent<Image>();
        originColor = blackImage.color;
        StartCoroutine(CheckForSleepMode());

    }

    IEnumerator CheckForSleepMode()
    {
        while (true)
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
            yield return 1f;
        }
    }

    public void ActivateSleepMode()
    {
        isSleepModeActive = true;
        sleepModeUI.SetActive(true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;  //ȭ�鲨�� ���� //�����Ҷ� 
        Screen.brightness = 0.1f;
        Application.targetFrameRate = 15;

    }

    public void DeactivateSleepMode()
    {
        isSleepModeActive = false;
        sleepModeUI.SetActive(false);
        Screen.sleepTimeout = SleepTimeout.SystemSetting; //����� �ý��ۿ� ���� ����.
        Screen.brightness = 1f;
        Application.targetFrameRate = 60;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (isSleepModeActive)
        {
            isPressing = true;
            pressStartTime = Time.time;
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeIn());

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
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOut());
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

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;             //��� �ð�
        Color startColor = blackImage.color;
        Color endColor = originColor;

        while (elapsedTime < requirePressTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / requirePressTime);
            blackImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        blackImage.color = endColor;
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = blackImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        while (elapsedTime < requirePressTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsedTime / requirePressTime);
            blackImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        blackImage.color = endColor;
    }

}
