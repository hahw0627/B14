using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour
{
    public Image timerBackground;
    public Image timerImage;
    public TextMeshProUGUI timerText;

    public float time = 90f;
    public float currentTime;

    private void OnEnable()
    {
        currentTime = time;
        StartCoroutine(TimerCountdown());
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:D2}:{seconds:D2}";

        timerImage.fillAmount = currentTime / time;
    }

    private IEnumerator TimerCountdown()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        // ���� �ð��� 0 ���ϰ� �Ǹ�
        DeactivateBoss();
        DeactivateTimer();
    }

    private void DeactivateBoss()
    {
        // ���� ��Ȱ��ȭ
        gameObject.SetActive(false);

        // Player�� StageReset ȣ��
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            StageManager.StageReset();
        }
        DeactivateTimer();
    }

    public void ActivateTimer()
    {
        timerBackground.gameObject.SetActive(true);
        timerImage.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    public void DeactivateTimer()
    {
        timerBackground.gameObject.SetActive(false);
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }
}
