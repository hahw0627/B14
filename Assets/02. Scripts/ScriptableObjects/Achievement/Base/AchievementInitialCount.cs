using UnityEngine;

public abstract class AchievementInitialCount : ScriptableObject
{
    public abstract int GetCount(AchievementDataSO achievementDataSO);
}
