using Quest.Core;
using Quest.Core.Task.Target.Base;
using UnityEngine;

public class QuestTest : SingletonDestroyable<QuestTest>
{
    [SerializeField]
    private Quest.Core.Quest _quest;

    [SerializeField]
    private Category _category;

    [SerializeField]
    private TaskTarget _target;

    private void Start()
    {
        var questSystem = QuestSystem.Instance;

        questSystem.onQuestRegistered += quest =>
        {
            print($"<color=orange>New Quest:{quest.CodeName} Registered</color>");
            print($"<color=orange>Active Quests Count:{questSystem.ActiveQuests.Count}</color>");
        };

        questSystem.onQuestCompleted += quest =>
        {
            print($"<color=orange>Quest:{quest.CodeName} Completed</color>");
            print($"<color=orange>Completed Quests Count:{questSystem.CompletedQuests.Count}</color>");
        };

        var newQuest = questSystem.Register(_quest);
        newQuest.onTaskSuccessChanged += (quest, task, currentSuccess, prevSuccess) =>
        {
            print($"<color=orange>Quest:{quest.CodeName}, Task:{task.CodeName}, CurrentSuccess:{currentSuccess}</color>");
        };
    }

    public void CountOneQuestSuccess()
    {
        QuestSystem.Instance.ReceiveReport(_category, _target, 1);
    }
}