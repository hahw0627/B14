using UnityEngine;

[CreateAssetMenu(fileName = "AchievementDataSO", menuName = "ScriptableObjects/AchievementDataSO", order = 7)]
public class AchievementDataSO : ScriptableObject
{
    public string Title;
    public string Content;
    public int RewardGold;
}
