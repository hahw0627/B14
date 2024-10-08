using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataSO_Test", menuName = "ScriptableObjects/MonsterDataSO_Test", order = 2)]
public class MonsterDataSO : ScriptableObject
{
    public string monsterName;
    public int MaxHp;
    public int Damage;
    public float AttackSpeed;
}
