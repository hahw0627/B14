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
        rewardText.text = $"��ġ ���� : {reward:F0} ���";
        if (timeAway.TotalHours > 12)
        {
            timeAwayText.text = $"���� �ð� : 12�ð� (�ִ�) / ���� ���� �ð� : {FormatTimeSpan(timeAway)}";
        }
        else
        {
            timeAwayText.text = $"���� �ð� : {FormatTimeSpan(timeAway)}";
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
            return $"{timeSpan.Days}�� {timeSpan.Hours}�ð�";
        else if (timeSpan.TotalHours >= 1)
            return $"{timeSpan.Hours}�ð� {timeSpan.Minutes}��";
        else
            return $"{timeSpan.Minutes}�� {timeSpan.Seconds}��";
    }

    private void OnConfirmButtonClicked()
    {
        idleRewardManager.ClaimReward();
        int rewardGold = Mathf.RoundToInt(currentReward);
        DataManager.Instance.AddGold(rewardGold);
        goldAcquireEffect.PlayGoldAcquireEffect(confirmButton.transform.position, rewardGold);
        Debug.Log($"{rewardGold} ��带 �����߽��ϴ�.");
        UIManager.Instance.UpdateCurrencyUI();
        rewardPanel.SetActive(false);
        mainSceneRewardButton.SetActive(false);
        currentReward = 0;
    }
}
