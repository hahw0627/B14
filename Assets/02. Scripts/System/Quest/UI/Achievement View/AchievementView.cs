using System.Collections.Generic;
using Quest.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.UI.Achievement_View
{
    public class AchievementView : MonoBehaviour
    {
        [FormerlySerializedAs("achievementGroup")]
        [SerializeField]
        private RectTransform _achievementGroup;
        [FormerlySerializedAs("achievementDetailViewPrefab")]
        [SerializeField]
        private AchievementDetailView _achievementDetailViewPrefab;

        private void Start()
        {
            var questSystem = QuestSystem.Instance;
            CreateDetailViews(questSystem.ActiveAchievements);
            CreateDetailViews(questSystem.CompletedAchievements);

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                gameObject.SetActive(false);
        }

        private void CreateDetailViews(IReadOnlyList<Core.Quest> achievements)
        {
            foreach (var achievement in achievements)
                Instantiate(_achievementDetailViewPrefab, _achievementGroup).Setup(achievement);
        }
    }
}
