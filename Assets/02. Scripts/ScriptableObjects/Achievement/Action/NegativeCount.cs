using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Achievement/Action/NegativeCount")]
public class NegativeCount : AchievementAction
{
    public override int Run(AchievementDataSO achievementDataSO, int currentCountOfAchievements,
        int countOfAchievements)
    {
        return countOfAchievements < 0 ? currentCountOfAchievements + countOfAchievements : currentCountOfAchievements;
    }
}