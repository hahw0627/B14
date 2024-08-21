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

        if (currentTime <= 0)
        {
            DeactivateBoss();
        }
    }

    private void DeactivateBoss()
    {
        Player player = FindObjectOfType<Player>();
        player.CurrentHp = 0;
        gameObject.SetActive(false);
        DeactivateTimer();
        StartCoroutine(player.DeathWithDelay());
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
