using System;
using UnityEngine;
using UnityEngine.Serialization;

public class IdleRewardManager : MonoBehaviour
{
    [FormerlySerializedAs("goldPerSecond")]
    public float GoldPerSecond = 1.0f;
    private const string LAST_REWARD_TIME_KEY = "LastRewardTime";
    private const string PENDING_REWARD_KEY = "PendingReward";
    private const string PENDING_REWARD_TIME_KEY = "PendingRewardTime";
    [FormerlySerializedAs("rewardUI")]
    public IdleRewardUI RewardUI;
    private const int MAX_IDLE_HOURS = 12;

    private float _pendingReward;
    private DateTime _lastRewardTime;
    private DateTime _pendingRewardTime;

    private void Start()
    {
        if (!FirstRunCheck.IsFirstRun)
        {
            LoadLastRewardTime();
            LoadPendingReward();
            CheckIdleReward();
        }
        else
        {
            // ù ������ ��� ���� �ð��� ������ ���� �ð����� ����
            _lastRewardTime = DateTime.Now;
            SaveLastRewardTime();
        }
    }

    private void OnApplicationQuit()
    {
        SavePendingReward();
    }

    public void CheckIdleReward()
    {
        if (_pendingReward <= 0)
        {
            var now = DateTime.Now;
            var idleTime = now - _lastRewardTime;
            var cappedIdleTime = TimeSpan.FromHours(Math.Min(idleTime.TotalHours, MAX_IDLE_HOURS));
            _pendingReward = (float)cappedIdleTime.TotalSeconds * GoldPerSecond;
            _pendingRewardTime = now;
        }

        if (!(_pendingReward > 0)) return;
        var timeAway = _pendingRewardTime - _lastRewardTime;
        ShowRewardUI(_pendingReward, timeAway);
        if (RewardUI.RewardPanel != null && !RewardUI.RewardPanel.activeSelf)
        {
            RewardUI.RewardPanel.SetActive(true);
        }
    }

    private void ShowRewardUI(float reward, TimeSpan timeAway)
    {
        RewardUI.ShowReward(reward, timeAway);
    }

    public void ClaimReward()
    {
        var rewardGold = Mathf.RoundToInt(_pendingReward);
        DataManager.Instance.AddGold(rewardGold);
        _lastRewardTime = _pendingRewardTime;
        SaveLastRewardTime();
        _pendingReward = 0f;
        SavePendingReward();
    }

    private void SaveLastRewardTime()
    {
        PlayerPrefs.SetString(LAST_REWARD_TIME_KEY, _lastRewardTime.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void LoadLastRewardTime()
    {
        if (PlayerPrefs.HasKey(LAST_REWARD_TIME_KEY))
        {
            var lastRewardTimeBinary = Convert.ToInt64(PlayerPrefs.GetString(LAST_REWARD_TIME_KEY));
            _lastRewardTime = DateTime.FromBinary(lastRewardTimeBinary);
        }
        else
        {
            _lastRewardTime = DateTime.Now;
            SaveLastRewardTime();
        }
    }

    private void SavePendingReward()
    {
        PlayerPrefs.SetFloat(PENDING_REWARD_KEY, _pendingReward);
        PlayerPrefs.SetString(PENDING_REWARD_TIME_KEY, _pendingRewardTime.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void LoadPendingReward()
    {
        if (!PlayerPrefs.HasKey(PENDING_REWARD_KEY)) return;
        _pendingReward = PlayerPrefs.GetFloat(PENDING_REWARD_KEY);
        var pendingRewardTimeBinary = Convert.ToInt64(PlayerPrefs.GetString(PENDING_REWARD_TIME_KEY));
        _pendingRewardTime = DateTime.FromBinary(pendingRewardTimeBinary);
    }

    public void ActivateIdleRewardSystem()
    {
        if (!FirstRunCheck.IsFirstRun) return;
        FirstRunCheck.IsFirstRun = false;
        FirstRunCheck.SaveKeyOfFirstRun();
        LoadLastRewardTime();
        LoadPendingReward();
        CheckIdleReward();
    }
}
