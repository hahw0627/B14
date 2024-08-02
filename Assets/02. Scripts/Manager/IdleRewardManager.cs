using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRewardManager : MonoBehaviour
{
    public float goldPerSecond = 1.0f;
    private const string LastPlayTimeKey = "LastPlayTime";
    public IdleRewardUI rewardUI;
    private const int MaxIdleHours = 12;

    private void Start()
    {
        CheckIdReward();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(LastPlayTimeKey, DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void CheckIdReward()
    {
        if (PlayerPrefs.HasKey(LastPlayTimeKey))
        {
            long lastPlayTimeBinary = Convert.ToInt64(PlayerPrefs.GetString(LastPlayTimeKey));
            DateTime lastPlayTime = DateTime.FromBinary(lastPlayTimeBinary);
            TimeSpan idleTime = DateTime.Now - lastPlayTime;

            TimeSpan cappedIdleTime = TimeSpan.FromHours(Math.Min(idleTime.TotalHours, MaxIdleHours));

            float reward = (float)idleTime.TotalSeconds *goldPerSecond;

            if(reward > 0)
            {
                ShowRewardUI(reward, idleTime);
            }
        }
    }

    private void ShowRewardUI(float reward, TimeSpan timeAway)
    {
        rewardUI.ShowReward(reward, timeAway);
    }
}
