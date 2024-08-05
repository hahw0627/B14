using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Achievement/Target/String")]
public class StringTarget : AchievementTarget
{
    [SerializeField]
    private string _value;

    public override object Value => _value;

    public override bool IsEqual(object target)
    {
        if (target is not string targetAsString)
        {
            return false;
        }
        return _value == targetAsString;
    }
}