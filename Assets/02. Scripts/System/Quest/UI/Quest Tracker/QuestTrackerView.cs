using System.Linq;
using Quest.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.UI.Quest_Tracker
{
    public class QuestTrackerView : MonoBehaviour
    {
        [FormerlySerializedAs("questTrackerPrefab")]
        [SerializeField]
        private QuestTracker _questTrackerPrefab;

        [FormerlySerializedAs("categoryColors")]
        [SerializeField]
        private CategoryColor[] _categoryColors;

        private void Start()
        {
            QuestSystem.Instance.onQuestRegistered += CreateQuestTracker;

            foreach (var quest in QuestSystem.Instance.ActiveQuests)
                CreateQuestTracker(quest);
        }

        private void OnDestroy()
        {
            if (QuestSystem.Instance)
                QuestSystem.Instance.onQuestRegistered -= CreateQuestTracker;
        }

        private void CreateQuestTracker(Quest.Core.Quest quest)
        {
            var categoryColor = _categoryColors.FirstOrDefault(x => x.Category == quest.Category);
            var color = categoryColor.Category == null ? Color.white : categoryColor.Color;
            Instantiate(_questTrackerPrefab, transform).Setup(quest, color);
        }

        [System.Serializable]
        private struct CategoryColor
        {
            [FormerlySerializedAs("category")]
            public Category Category;

            [FormerlySerializedAs("color")]
            public Color Color;
        }
    }
}