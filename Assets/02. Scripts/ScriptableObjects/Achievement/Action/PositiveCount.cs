using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Achievement/Action/PositiveCount")]
public class PositiveCount : BaseAchievementAction
{
    public override int Run(AchievementDataSO achievementDataSO, int currentCountOfAchievements,
        int countOfAchievements)
    {
        return countOfAchievements > 0 ? currentCountOfAchievements + countOfAchievements : currentCountOfAchievements;
    }
}