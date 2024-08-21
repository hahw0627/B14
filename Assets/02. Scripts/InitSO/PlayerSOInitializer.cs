#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(PlayerDataSO))]
public class PlayerSOInitializer : ScriptableObjectEditorBase<PlayerDataSO>
{
    protected override void InitializeValues()
    {
        for (var i = 0; i < ScriptableObject.EquippedSkills.Count; i++)
        {
            ScriptableObject.EquippedSkills[i] = null; 
        }
            
        ScriptableObject.Gold = 0;
        ScriptableObject.Gem = 3000;
        ScriptableObject.TotalPower = 10;
        ScriptableObject.Damage = 10;
        ScriptableObject.Def = 0;
        ScriptableObject.AttackSpeed = 1;
        ScriptableObject.MaxHp = 100;
        ScriptableObject.HpRecovery = 5;
        ScriptableObject.CriticalPer = 0;
        ScriptableObject.CriticalMultiplier = 1.5f;
    }
}

#endif