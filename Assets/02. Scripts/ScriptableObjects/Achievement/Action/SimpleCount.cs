using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Achievement/Action/SimpleCount")]
public class SimpleCount : BaseAchievementAction
{
    public override int Run(AchievementDataSO achievementDataSO, int currentCountOfAchievements,
        int countOfAchievements)
    {
        return currentCountOfAchievements + countOfAchievements;
    }
}