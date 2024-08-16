using UnityEngine;

[CreateAssetMenu(fileName = "StatDataSO", menuName = "ScriptableObjects/StatDataSO", order = 8)]
public class StatDataSO : ScriptableObject
{
    public long AttackCost;
    public long AttackSpeedCost;
    public long MaxHpCost;
    public long HpRecoveryCost;
    public long CriticalPercentageCost;
    public long CriticalMultiplierCost;
}
