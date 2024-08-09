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

        // 남은 시간이 0 이하가 되면
        DeactivateBoss();
        DeactivateTimer();
    }

    private void DeactivateBoss()
    {
        // 보스 비활성화
        gameObject.SetActive(false);

        // Player의 StageReset 호출
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.StageReset();
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
