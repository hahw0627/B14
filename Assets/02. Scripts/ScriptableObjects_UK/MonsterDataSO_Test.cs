using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataSO_Test", menuName = "ScriptableObjects/MonsterDataSO_Test", order = 2)]
public class MonsterDataSO_Test : ScriptableObject
{
    public string monsterName;
    public int Hp;
    public int Damage;
    public float AttackSpeed;

    public int stagePage;
    public int stage;
}
