using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Achievement/Action/SimpleCount")]
public class SimpleCount : AchievementAction
{
    public override int Run(AchievementDataSO achievementDataSO, int currentCountOfAchievements,
        int countOfAchievements)
    {
        return currentCountOfAchievements + countOfAchievements;
    }
}