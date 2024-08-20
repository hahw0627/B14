using UnityEditor;

[CustomEditor(typeof(StatDataSO))]
public class StatSOInitializer : ScriptableObjectEditorBase<StatDataSO>
{
    protected override void InitializeValues()
    {
        ScriptableObject.AttackCost = 1;
        ScriptableObject.AttackSpeedCost = 1;
        ScriptableObject.MaxHpCost = 10;
        ScriptableObject.HpRecoveryCost = 3;
        ScriptableObject.CriticalPercentageCost = 100;
        ScriptableObject.CriticalMultiplierCost = 50;
    }
}