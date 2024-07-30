using UnityEngine;

[CreateAssetMenu(fileName = "AchievementDataSO", menuName = "ScriptableObjects/Achievement/AchievementDataSO",
    order = 7)]
public class AchievementDataSO : ScriptableObject
{
    [Header("Text")]
    [SerializeField]
    private string _description;

    [Header("Setting")]
    [SerializeField]
    private int _countRequiredToAchieve;

    [Header("Action")]
    [SerializeField]
    private AchievementAction _AchievementAction;

    public int CurrentCountOfAchievements { get; private set; }
    public string Description => _description;
    public int CountRequiredToAchieve => _countRequiredToAchieve;

    public void ReceiveReport(int countOfAchievements)
    {
        CurrentCountOfAchievements = _AchievementAction.Run(this, CurrentCountOfAchievements, countOfAchievements);
    }
}





