using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRewardManager : MonoBehaviour
{
    public float goldPerSecond = 1.0f;
    private const string LastRewardTimeKey = "LastRewardTime";
    private const string PendingRewardKey = "PendingReward";
    private const string PendingRewardTimeKey = "PendingRewardTime";
    public IdleRewardUI rewardUI;
    private const int MaxIdleHours = 12;

    private float pendingReward = 0f;
    private DateTime lastRewardTime;
    private DateTime pendingRewardTime;

    public FirstRunCheck firstRunCheck;

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
            // 첫 실행인 경우 현재 시간을 마지막 보상 시간으로 설정
            lastRewardTime = DateTime.Now;
            SaveLastRewardTime();
        }
    }

    private void OnApplicationQuit()
    {
        SavePendingReward();
    }

    public void CheckIdleReward()
    {
        if (pendingReward <= 0)
        {
            DateTime now = DateTime.Now;
            TimeSpan idleTime = now - lastRewardTime;
            TimeSpan cappedIdleTime = TimeSpan.FromHours(Math.Min(idleTime.TotalHours, MaxIdleHours));
            pendingReward = (float)cappedIdleTime.TotalSeconds * goldPerSecond;
            pendingRewardTime = now;
        }

        if (pendingReward > 0)
        {
            TimeSpan timeAway = pendingRewardTime - lastRewardTime;
            ShowRewardUI(pendingReward, timeAway);
            if (rewardUI.rewardPanel != null && !rewardUI.rewardPanel.activeSelf)
            {
                rewardUI.rewardPanel.SetActive(true);
            }
        }
    }

    private void ShowRewardUI(float reward, TimeSpan timeAway)
    {
        rewardUI.ShowReward(reward, timeAway);
    }

    public void ClaimReward()
    {
        int rewardGold = Mathf.RoundToInt(pendingReward);
        DataManager.Instance.AddGold(rewardGold);
        lastRewardTime = pendingRewardTime;
        SaveLastRewardTime();
        pendingReward = 0f;
        SavePendingReward();
    }

    private void SaveLastRewardTime()
    {
        PlayerPrefs.SetString(LastRewardTimeKey, lastRewardTime.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void LoadLastRewardTime()
    {
        if (PlayerPrefs.HasKey(LastRewardTimeKey))
        {
            long lastRewardTimeBinary = Convert.ToInt64(PlayerPrefs.GetString(LastRewardTimeKey));
            lastRewardTime = DateTime.FromBinary(lastRewardTimeBinary);
        }
        else
        {
            lastRewardTime = DateTime.Now;
            SaveLastRewardTime();
        }
    }

    private void SavePendingReward()
    {
        PlayerPrefs.SetFloat(PendingRewardKey, pendingReward);
        PlayerPrefs.SetString(PendingRewardTimeKey, pendingRewardTime.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void LoadPendingReward()
    {
        if (PlayerPrefs.HasKey(PendingRewardKey))
        {
            pendingReward = PlayerPrefs.GetFloat(PendingRewardKey);
            long pendingRewardTimeBinary = Convert.ToInt64(PlayerPrefs.GetString(PendingRewardTimeKey));
            pendingRewardTime = DateTime.FromBinary(pendingRewardTimeBinary);
        }
    }

    public void ActivateIdleRewardSystem()
    {
        if (FirstRunCheck.IsFirstRun)
        {
            FirstRunCheck.IsFirstRun = false;
            FirstRunCheck.SaveKeyOfFirstRun();
            LoadLastRewardTime();
            LoadPendingReward();
            CheckIdleReward();
        }
    }
}
