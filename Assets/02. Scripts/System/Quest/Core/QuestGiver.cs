using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.Core
{
    public class QuestGiver : MonoBehaviour
    {
        [FormerlySerializedAs("quests")]
        [SerializeField]
        private Quest[] _quests;

        private void Start()
        {
            foreach (var quest in _quests)
            {
                if (quest.IsAcceptable && !QuestSystem.Instance.ContainsInCompleteQuests(quest))
                    QuestSystem.Instance.Register(quest);
            }
        }
    }
}
