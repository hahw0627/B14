using UnityEngine;

[CreateAssetMenu(fileName = "StatDataSO", menuName = "ScriptableObjects/StatDataSO", order = 8)]
public class StatDataSO : ScriptableObject
{
    public int AttackCost;
    public int AttackSpeedCost;
    public int HpCost;
    public int HpRecoveryCost;
    public int CriticalPercentageCost;
    public int CriticalMultiplierCost;
}
