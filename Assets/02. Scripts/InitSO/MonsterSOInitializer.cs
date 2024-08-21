#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(MonsterDataSO))]
public class MonsterSOInitializer : ScriptableObjectEditorBase<MonsterDataSO>
{
    protected override void InitializeValues()
    {
        ScriptableObject.MaxHp = 50;
        ScriptableObject.Damage = 5;
        ScriptableObject.AttackSpeed = 1;
    }
}

#endif