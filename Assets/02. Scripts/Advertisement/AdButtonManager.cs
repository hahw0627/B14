using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;
using System.Globalization;

public class AdButtonManager : MonoBehaviour
{
    public TextMeshProUGUI CooldownText;

    public const float COOLDOWN_DURATION = 180f;
    private Button _rewardedAdButton;

    private const string COOLDOWN_KEY = "AdCooldownEndTime";
    private DateTime _cooldownEndTime;

    private void Start()
    {
        _rewardedAdButton = gameObject.GetComponent<Button>();
        CooldownText.gameObject.SetActive(false);

        LoadCooldownTime();
    }

    public void StartCooldown(float cooldownTime)
    {
        _cooldownEndTime = DateTime.Now.AddSeconds(cooldownTime);
        SaveCooldownTime();

        CooldownText.gameObject.SetActive(true);
        _rewardedAdButton.interactable = false;
        StartCoroutine(CooldownTimer(cooldownTime));
    }

    private IEnumerator CooldownTimer(float cooldownTime)
    {
        var remainingTime = cooldownTime;

        while (remainingTime > 0)
        {
            var minutes = (int)(remainingTime / 60); // 남은 분 계산
            var seconds = (int)(remainingTime % 60); // 남은 초 계산
            CooldownText.text = $"{minutes:D2}:{seconds:D2}"; // 분:초 형식으로 표시
            yield return new WaitForSeconds(1);
            remainingTime--;
        }

        _rewardedAdButton.interactable = true;
        CooldownText.gameObject.SetActive(false);
    }

    private void LoadCooldownTime()
    {
        if (!PlayerPrefs.HasKey(COOLDOWN_KEY)) return;
        var storedTime = PlayerPrefs.GetString(COOLDOWN_KEY);

        if (!DateTime.TryParse(storedTime, out _cooldownEndTime)) return;
        var remainingTime = (_cooldownEndTime - DateTime.Now).TotalSeconds;
        if (remainingTime > 0)
        {
            _rewardedAdButton.interactable = false;
            StartCooldown((float)remainingTime); // 남은 시간이 있다면 쿨다운 시작
        }
        else
        {
            _rewardedAdButton.interactable = true; // 광고 보상 버튼 활성화
            CooldownText.gameObject.SetActive(false); // 쿨다운이 끝났다면 텍스트 비활성화
        }
    }

    private void SaveCooldownTime()
    {
        PlayerPrefs.SetString(COOLDOWN_KEY,
            _cooldownEndTime.ToString(CultureInfo.InvariantCulture)); // DateTime 문자열로 저장
        PlayerPrefs.Save();
    }
}