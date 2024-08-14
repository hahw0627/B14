using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quest.UI.Quest_View
{
    public class QuestListViewController : MonoBehaviour
    {
        [FormerlySerializedAs("tabGroup")]
        [SerializeField]
        private ToggleGroup _tabGroup;

        [FormerlySerializedAs("activeQuestListView")]
        [SerializeField]
        private QuestListView _activeQuestListView;

        [FormerlySerializedAs("completedQuestListView")]
        [SerializeField]
        private QuestListView _completedQuestListView;

        public IEnumerable<Toggle> Tabs => _tabGroup.ActiveToggles();

        public void AddQuestToActiveListView(Core.Quest quest, UnityAction<bool> onClicked)
            => _activeQuestListView.AddElement(quest, onClicked);

        public void RemoveQuestFromActiveListView(Core.Quest quest)
            => _activeQuestListView.RemoveElement(quest);

        public void AddQuestToCompletedListView(Core.Quest quest, UnityAction<bool> onClicked)
            => _completedQuestListView.AddElement(quest, onClicked);
    }
}