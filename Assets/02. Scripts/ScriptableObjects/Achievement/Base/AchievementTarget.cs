using UnityEngine;

public abstract class AchievementTarget : ScriptableObject
{
    public abstract object Value { get; }

    public abstract bool IsEqual(object target);
}