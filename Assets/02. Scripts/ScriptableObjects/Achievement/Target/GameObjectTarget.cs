using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Achievement/Target/GameObject")]
public class GameObjectTarget : AchievementTarget
{
    [SerializeField]
    private GameObject _value;

    public override object Value => _value;

    public override bool IsEqual(object target)
    {
        return target is GameObject targetAsGameObject && targetAsGameObject.name.Contains(_value.name);
    }
}