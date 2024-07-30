using UnityEngine;

public abstract class AchievementAction : ScriptableObject
{
    public abstract int Run(AchievementDataSO achievementDataSO, int currentCountOfAchievements,
        int countOfAchievements);
}