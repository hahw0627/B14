using System.Collections.Generic;
using System.Linq;
using Quest.Core;
using Quest.Core.Task.Target.Base;
using TMPro;
using UnityEngine;

public class QuestTest : SingletonDestroyable<QuestTest>
{
    [SerializeField]
    private List<Quest.Core.Quest> _questList;

    [SerializeField]
    private Category _category;

    [SerializeField]
    private TaskTarget _target;

    [SerializeField]
    private GameObject _questImage;

    [SerializeField]
    private TextMeshProUGUI _questDescription;

    private int _currentQuestIndex; // 현재 진행 중인 퀘스트 인덱스

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

            // 현재 퀘스트가 완료되면 다음 퀘스트를 등록
            _currentQuestIndex++;
            RegisterNextQuest(questSystem);
            DataManager.Instance.AddGem(_questList[_currentQuestIndex].Rewards[0].Quantity);
            _questImage.SetActive(true);
            _questDescription.text = _questList[_currentQuestIndex].Description;
        };

        // 첫 번째 퀘스트 등록
        RegisterNextQuest(questSystem);
        _questImage.SetActive(true);
        _questDescription.text = _questList[_currentQuestIndex].Description;
    }

    private void RegisterNextQuest(QuestSystem questSystem)
    {
        if (_currentQuestIndex < _questList.Count)
        {
            var newQuest = questSystem.Register(_questList[_currentQuestIndex]);
            newQuest.onTaskSuccessChanged += (quest1, task, currentSuccess, _) =>
            {
                print(
                    $"<color=orange>Quest:{quest1.CodeName}, Task:{task.CodeName}, CurrentSuccess:{currentSuccess}</color>");
            };
        }
        else
        {
            print("<color=green>All quests have been registered.</color>");
        }
    }

    public void CountOneQuestSuccess()
    {
        QuestSystem.Instance.ReceiveReport(_category, _target, 1);
    }
}