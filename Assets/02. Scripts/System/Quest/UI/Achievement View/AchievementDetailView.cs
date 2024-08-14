using Quest.Core.Task;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quest.UI.Achievement_View
{
    public class AchievementDetailView : MonoBehaviour
    {
        [FormerlySerializedAs("achievementIcon")]
        [SerializeField]
        private Image _achievementIcon;

        [FormerlySerializedAs("titleText")]
        [SerializeField]
        private TextMeshProUGUI _titleText;

        [FormerlySerializedAs("description")]
        [SerializeField]
        private TextMeshProUGUI _description;

        [FormerlySerializedAs("rewardIcon")]
        [SerializeField]
        private Image _rewardIcon;

        [FormerlySerializedAs("rewardText")]
        [SerializeField]
        private TextMeshProUGUI _rewardText;

        [FormerlySerializedAs("completionScreen")]
        [SerializeField]
        private GameObject _completionScreen;

        private Core.Quest _target;

        private void OnDestroy()
        {
            if (_target == null) return;
            _target.onTaskSuccessChanged -= UpdateDescription;
            _target.onCompleted -= ShowCompletionScreen;
        }

        public void Setup(Core.Quest achievement)
        {
            _target = achievement;

            _achievementIcon.sprite = achievement.Icon;
            _titleText.text = achievement.DisplayName;

            var task = achievement.CurrentTaskGroup.Tasks[0];
            _description.text = BuildTaskDescription(task);

            var reward = achievement.Rewards[0];
            _rewardIcon.sprite = reward.Icon;
            _rewardText.text = $"{reward.Description} +{reward.Quantity}";

            if (achievement.IsComplete)
                _completionScreen.SetActive(true);
            else
            {
                _completionScreen.SetActive(false);
                achievement.onTaskSuccessChanged += UpdateDescription;
                achievement.onCompleted += ShowCompletionScreen;
            }
        }

        private void UpdateDescription(Core.Quest achievement, Task task, int currentSuccess, int prevSuccess)
            => _description.text = BuildTaskDescription(task);

        private void ShowCompletionScreen(Core.Quest achievement)
            => _completionScreen.SetActive(true);

        private static string BuildTaskDescription(Task task) =>
            $"{task.Description} {task.CurrentSuccess}/{task.NeedSuccessToComplete}";
    }
}