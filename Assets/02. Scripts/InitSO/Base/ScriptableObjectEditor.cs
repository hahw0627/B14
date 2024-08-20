using UnityEditor;
using UnityEngine;

public abstract class ScriptableObjectEditorBase<T> : Editor where T : ScriptableObject
{
    protected T ScriptableObject;

    protected virtual void OnEnable()
    {
        ScriptableObject = (T)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!GUILayout.Button("값 초기화")) return;
        InitializeValues();
        EditorUtility.SetDirty(ScriptableObject);
        AssetDatabase.SaveAssets();
        Debug.Log($"<b>{ScriptableObject.name}</b>의 값이 초기화되었습니다.");
    }

    protected abstract void InitializeValues();
}