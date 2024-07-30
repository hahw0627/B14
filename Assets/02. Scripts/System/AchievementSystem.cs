using UnityEngine;
 
public class AchievementSystem : MonoBehaviour
{
    [System.Serializable]
    public class Achievement
    {
        public string Name;
        public string Description;
        public bool IsUnlocked;
        public bool ShowAlert;  // 알람을 띄울지 여부
 
        public void Unlock()
        {
            if (IsUnlocked) return;
            IsUnlocked = true;
            Debug.Log($"업적 달성: {Name}");
 
            if (ShowAlert)
            {
                ShowAchievementAlert();
            }
        }
 
        private void ShowAchievementAlert()
        {
            // 알람을 띄우는 로직 추가 (예: UI 알람, 팝업 등)
            Debug.Log($"알람: {Name} 달성!");
        }
    }
 
    public Achievement[] Achievements;

    private void Start()
    {
        // 업적 초기화
        foreach (Achievement achievement in Achievements)
        {
            achievement.IsUnlocked = false;
        }
    }
 
    public void CheckAchievements(string condition)
    {
        // 특정 조건을 만족하는 경우 해당 업적 달성
        foreach (Achievement achievement in Achievements)
        {
            if (!achievement.IsUnlocked && condition == achievement.Name)
            {
                achievement.Unlock();
            }
        }
    }
}