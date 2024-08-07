using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Quest/QuestDatabase")]
public class QuestDatabase : ScriptableObject
{
    [FormerlySerializedAs("quests")]
    [SerializeField]
    private List<Quest> _quests;

    public IReadOnlyList<Quest> Quests => _quests;

    public Quest FindQuestBy(string codeName) => _quests.FirstOrDefault(x => x.CodeName == codeName);

#if UNITY_EDITOR 
    [ContextMenu("FindQuests")]
    private void FindQuests()
    {
        FindQuestsBy<Quest>();
    }

    [ContextMenu("FindAchievements")]
    private void FindAchievements()
    {
        FindQuestsBy<Achievement>();
    }

    private void FindQuestsBy<T>() where T : Quest
    {
        _quests = new List<Quest>();

        var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var quest = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (quest.GetType() == typeof(T))
                _quests.Add(quest);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
