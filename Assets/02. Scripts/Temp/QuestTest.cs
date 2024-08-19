using System.Collections.Generic;
using System.Linq;
using Quest.Core;
using Quest.Core.Task.Target.Base;
using UnityEngine;

public class QuestTest : SingletonDestroyable<QuestTest>
{
    [SerializeField]
    private List<Quest.Core.Quest> _questList;

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

        foreach (var newQuest in _questList.Select(quest => questSystem.Register(quest)))
        {
            newQuest.onTaskSuccessChanged += (quest1, task, currentSuccess, _) =>
            {
                print($"<color=orange>Quest:{quest1.CodeName}, Task:{task.CodeName}, CurrentSuccess:{currentSuccess}</color>");
            };
        }
    }

    public void CountOneQuestSuccess()
    {
        QuestSystem.Instance.ReceiveReport(_category, _target, 1);
    }
}