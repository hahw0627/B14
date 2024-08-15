using UnityEngine;


public enum MonsterType
{
    Easy,
    Normal,
    Hard,
    Boss
}

[CreateAssetMenu(fileName = "MonsterStatistics", menuName = "ScriptableObjects/MonsterStatistics", order = 1)]
public class MonsterStatistics : ScriptableObject
{
    public string Name;
    public MonsterType Type;
    public int MaxHp;
    public int Attack;
    public int AttackDelay;
}