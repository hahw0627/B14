using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IdleRewardUI : MonoBehaviour
{
    [FormerlySerializedAs("rewardPanel")]
    public GameObject RewardPanel;

    [FormerlySerializedAs("rewardText")]
    public TextMeshProUGUI RewardText;

    [FormerlySerializedAs("timeAwayText")]
    public TextMeshProUGUI TimeAwayText;

    [FormerlySerializedAs("confirmButton")]
    public Button ConfirmButton;

    [FormerlySerializedAs("closeButton")]
    public Button CloseButton;

    [FormerlySerializedAs("goldAcquireEffect")]
    public GoldAcquireEffect GoldAcquireEffect;

    [FormerlySerializedAs("mainSceneRewardButton")]
    public GameObject MainSceneRewardButton;

    private TimeSpan _timeAway;
    private float _currentReward;

    [FormerlySerializedAs("idleRewardManager")]
    public IdleRewardManager IdleRewardManager;

    private void Start()
    {
        ConfirmButton.onClick.AddListener(OnConfirmButtonClicked);
        CloseButton.onClick.AddListener(OnCloseButtonClicked);
        MainSceneRewardButton.SetActive(false);
    }

    private void Update()
    {
        MainSceneRewardButton.SetActive(_currentReward > 0 && !RewardPanel.activeSelf);
    }

    public void ShowReward(float reward, TimeSpan timeAway)
    {
        _currentReward = reward;
        RewardText.text = $"{reward:F0} 골드";
        TimeAwayText.text = timeAway.TotalHours > 12
            ? $"12시간 (최대) / 실제 부재 시간: {FormatTimeSpan(timeAway)}"
            : $"{FormatTimeSpan(timeAway)}";
        RewardPanel.SetActive(true);
        MainSceneRewardButton.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        RewardPanel.SetActive(false);
        MainSceneRewardButton.SetActive(true);
    }

    private static string FormatTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.TotalDays >= 1)
            return $"{timeSpan.Days}일 {timeSpan.Hours}시간";

        return timeSpan.TotalHours >= 1
            ? $"{timeSpan.Hours}시간 {timeSpan.Minutes}분"
            : $"{timeSpan.Minutes}분 {timeSpan.Seconds}초";
    }

    private void OnConfirmButtonClicked()
    {
        var rewardGold = Mathf.RoundToInt(_currentReward);

        GoldAcquireEffect.PlayGoldAcquireEffect(ConfirmButton.transform.position, rewardGold);

        // 이펙트가 완료된 후에 실행될 콜백 등록
        GoldAcquireEffect.OnEffectCompleted += OnGoldEffectCompleted;

        RewardPanel.SetActive(false);
        MainSceneRewardButton.SetActive(false);
    }

    private void OnGoldEffectCompleted()
    {
        // 이펙트 완료 후 실행될 로직
        IdleRewardManager.ClaimReward();

        var rewardGold = Mathf.RoundToInt(_currentReward);
        Debug.Log($"<color=yellow>{rewardGold} 골드를 지급했습니다.</color>");

        UIManager.Instance.UpdateCurrencyUI();

        _currentReward = 0;

        // 이벤트 구독 해제
        GoldAcquireEffect.OnEffectCompleted -= OnGoldEffectCompleted;
    }
}