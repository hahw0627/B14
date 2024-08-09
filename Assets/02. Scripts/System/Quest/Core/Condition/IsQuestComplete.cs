using UnityEngine;

namespace Quest.Core.Condition
{
    [CreateAssetMenu(menuName = "Quest/Condition/IsQuestComplete", fileName = "IsQuestComplete_")]
    public class IsQuestComplete : Base.Condition
    {
        [SerializeField]
        private Quest _target;

        public override bool IsPass(Quest quest)
            => QuestSystem.Instance.ContainsInCompleteQuests(_target);
    }
}