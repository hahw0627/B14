using UnityEngine;

namespace Quest.Core
{
    [CreateAssetMenu(menuName = "Quest/Achievement", fileName = "Achievement_")]
    public class Achievement : Quest
    {
        protected override bool IsCancelable => false;
        public override bool IsSavable => true;

        public override void Cancel()
        {
            Debug.LogAssertion("Achievement can't be canceled");
        }
    }
}
