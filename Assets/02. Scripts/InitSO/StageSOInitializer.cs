#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(StageDataSO))]
public class StageSOInitializer : ScriptableObjectEditorBase<StageDataSO>
{
    protected override void InitializeValues()
    {
        ScriptableObject.Stage = 1;
        ScriptableObject.StagePage = 0;
    }
}

#endif