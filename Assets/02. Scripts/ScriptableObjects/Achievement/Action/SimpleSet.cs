using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Achievement/Action/SimpleSet")]
public class SimpleSet : BaseAchievementAction
{
    public override int Run(AchievementDataSO achievementDataSO, int currentCountOfAchievements,
        int countOfAchievements)
    {
        return countOfAchievements;
    }
}
