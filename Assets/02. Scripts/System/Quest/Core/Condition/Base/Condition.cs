using UnityEngine;

public abstract class Condition : ScriptableObject
{
    [SerializeField]
    private string _description;

    public abstract bool IsPass(Quest quest);
}
