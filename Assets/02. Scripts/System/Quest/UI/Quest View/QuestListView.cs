using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quest.UI.Quest_View
{
    public class QuestListView : MonoBehaviour
    {
        [FormerlySerializedAs("elementTextPrefab")]
        [SerializeField]
        private TextMeshProUGUI _elementTextPrefab;

        private readonly Dictionary<Core.Quest, GameObject> _elementsByQuest = new();
        private ToggleGroup _toggleGroup;

        private void Awake()
        {
            _toggleGroup = GetComponent<ToggleGroup>();
        }

        public void AddElement(Core.Quest quest, UnityAction<bool> onClicked)
        {
            var element = Instantiate(_elementTextPrefab, transform);
            element.text = quest.DisplayName;

            var toggle = element.GetComponent<Toggle>();
            toggle.group = _toggleGroup;
            toggle.onValueChanged.AddListener(onClicked);

            _elementsByQuest.Add(quest, element.gameObject);
        }

        public void RemoveElement(Core.Quest quest)
        {
            Destroy(_elementsByQuest[quest]);
            _elementsByQuest.Remove(quest);
        }
    }
}
