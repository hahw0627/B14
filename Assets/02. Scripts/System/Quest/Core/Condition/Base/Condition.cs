using UnityEngine;

namespace Quest.Core.Condition.Base
{
    public abstract class Condition : ScriptableObject
    {
        [SerializeField]
        private string _description;

        public abstract bool IsPass(Quest quest);
    }
}
