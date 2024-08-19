using Quest.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.UI.Quest_View
{
    public class QuestView : MonoBehaviour
    {
        [FormerlySerializedAs("questListViewController")]
        [SerializeField]
        private QuestListViewController _questListViewController;
        [FormerlySerializedAs("questDetailView")]
        [SerializeField]
        private QuestDetailView _questDetailView;

        private void Start()
        {
            var questSystem = QuestSystem.Instance;

            foreach (var quest in questSystem.ActiveQuests)
                AddQuestToActiveListView(quest);

            foreach (var quest in questSystem.CompletedQuests)
                AddQuestToCompletedListView(quest);

            questSystem.onQuestRegistered += AddQuestToActiveListView;
            questSystem.onQuestCompleted += RemoveQuestFromActiveListView;
            questSystem.onQuestCompleted += AddQuestToCompletedListView;
            questSystem.onQuestCompleted += HideDetailIfQuestCanceled;
            questSystem.onQuestCanceled += HideDetailIfQuestCanceled;
            questSystem.onQuestCanceled += RemoveQuestFromActiveListView;

                foreach (var tab in _questListViewController.Tabs)
                tab.onValueChanged.AddListener(HideDetail);

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            var questSystem = QuestSystem.Instance;
            if (questSystem)
            {
                questSystem.onQuestRegistered -= AddQuestToActiveListView;
                questSystem.onQuestCompleted -= RemoveQuestFromActiveListView;
                questSystem.onQuestCompleted -= AddQuestToCompletedListView;
                questSystem.onQuestCompleted -= HideDetailIfQuestCanceled;
                questSystem.onQuestCanceled -= HideDetailIfQuestCanceled;
                questSystem.onQuestCanceled -= RemoveQuestFromActiveListView;
            }
        }

        private void OnEnable()
        {
            if (_questDetailView.Target != null)
                _questDetailView.Show(_questDetailView.Target);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                gameObject.SetActive(false);
        }

        private void ShowDetail(bool isOn, Core.Quest quest)
        {
            if (isOn)
                _questDetailView.Show(quest);
        }

        private void HideDetail(bool isOn)
        {
            _questDetailView.Hide();
        }

        private void AddQuestToActiveListView(Core.Quest quest)
            => _questListViewController.AddQuestToActiveListView(quest, isOn => ShowDetail(isOn, quest));

        private void AddQuestToCompletedListView(Core.Quest quest)
            => _questListViewController.AddQuestToCompletedListView(quest, isOn => ShowDetail(isOn, quest));

        private void HideDetailIfQuestCanceled(Core.Quest quest)
        {
            if (_questDetailView.Target == quest)
                _questDetailView.Hide();
        }

        private void RemoveQuestFromActiveListView(Core.Quest quest)
        {
            _questListViewController.RemoveQuestFromActiveListView(quest);
            if (_questDetailView.Target == quest)
                _questDetailView.Hide();
        }
    }
}
