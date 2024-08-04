using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleRewardUI : MonoBehaviour
{
    public GameObject rewardPanel;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI timeAwayText;
    public Button confirmButton;
    public Button closeButton;
    public GoldAcquireEffect goldAcquireEffect;
    public GameObject mainSceneRewardButton;

    private TimeSpan timeAway;
    private float currentReward;
    public IdleRewardManager idleRewardManager;

    private void Start()
    {
        rewardPanel.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
        mainSceneRewardButton.SetActive(false);
    }

    public void ShowReward(float reward, TimeSpan timeAway)
    {
        currentReward = reward;
        rewardText.text = $"방치 보상 : {reward:F0} 골드";
        if (timeAway.TotalHours > 12)
        {
            timeAwayText.text = $"부재 시간 : 12시간 (최대) / 실제 부재 시간 : {FormatTimeSpan(timeAway)}";
        }
        else
        {
            timeAwayText.text = $"부재 시간 : {FormatTimeSpan(timeAway)}";
        }
        rewardPanel.SetActive(true);
        mainSceneRewardButton.SetActive(false);
    }
    private void OnCloseButtonClicked()
    {
        rewardPanel.SetActive(false);
        mainSceneRewardButton.SetActive(true);
    }
    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.TotalDays >= 1)
            return $"{timeSpan.Days}일 {timeSpan.Hours}시간";
        else if (timeSpan.TotalHours >= 1)
            return $"{timeSpan.Hours}시간 {timeSpan.Minutes}분";
        else
            return $"{timeSpan.Minutes}분 {timeSpan.Seconds}초";
    }

    private void OnConfirmButtonClicked()
    {
        idleRewardManager.ClaimReward();
        int rewardGold = Mathf.RoundToInt(currentReward);
        DataManager.Instance.AddGold(rewardGold);
        goldAcquireEffect.PlayGoldAcquireEffect(confirmButton.transform.position, rewardGold);
        Debug.Log($"{rewardGold} 골드를 지급했습니다.");
        UIManager.Instance.UpdateCurrencyUI();
        rewardPanel.SetActive(false);
        mainSceneRewardButton.SetActive(false);
        currentReward = 0;
    }
}
