using Quest.Core.Task.Target.Base;
using UnityEngine;

namespace Quest.Core.Task.Target
{
    [CreateAssetMenu(menuName = "Quest/Task/Target/String", fileName = "Target_")]
    public class StringTarget : TaskTarget
    {
        [SerializeField]
        private string _value;

        public override object Value => _value;

        public override bool IsEqual(object target)
        {
            if (target is not string targetAsString)
                return false;
            return _value == targetAsString;
        }
    }
}