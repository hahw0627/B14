using Quest.Core.Task.Target.Base;
using UnityEngine;

namespace Quest.Core.Task.Target
{
    [CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Target_")]
    public class GameObjectTarget : TaskTarget
    {
        [SerializeField]
        private GameObject _value;

        public override object Value => _value;

        public override bool IsEqual(object target)
        {
            var targetAsGameObject = target as GameObject;
            return targetAsGameObject && targetAsGameObject.name.Contains(_value.name);
        }
    }
}
