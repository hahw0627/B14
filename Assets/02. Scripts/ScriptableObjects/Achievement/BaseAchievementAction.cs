using UnityEngine;

public abstract class BaseAchievementAction : ScriptableObject
{
    public abstract int Run(AchievementDataSO achievementDataSO, int currentCountOfAchievements,
        int countOfAchievements);
}