using System.Collections;
using System.Collections.Generic;
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
    private GameObject _badgeImage; // 도장 이미지 추가

    [SerializeField]
    private TextMeshProUGUI _questDescription;

    [SerializeField]
    private GoldAcquireEffect _goldAcquireEffect;

    [SerializeField]
    private Transform _startPositionTransformOfEffect;


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

            // 현재 퀘스트가 완료되면 보상 추가
            DataManager.Instance.AddGem(_questList[_currentQuestIndex].Rewards[0].Quantity);
            StartCoroutine(ShowCompletedQuest(questSystem));
        };

        // 첫 번째 퀘스트 등록
        RegisterQuest(questSystem, _currentQuestIndex);
    }

    private void RegisterQuest(QuestSystem questSystem, int questIndex)
    {
        if (questIndex >= _questList.Count) return;
        var quest = questSystem.Register(_questList[questIndex]);
        quest.onTaskSuccessChanged += (quest1, task, currentSuccess, _) =>
        {
            print($"<color=orange>Quest:{quest1.CodeName}, Task:{task.CodeName}, CurrentSuccess:{currentSuccess}</color>");
        };

        // 퀘스트 이미지 및 설명 설정
        _questImage.SetActive(true);
        _questDescription.text = _questList[questIndex].Description;
    }

    private IEnumerator ShowCompletedQuest(QuestSystem questSystem)
    {
        // 직전 퀘스트의 이미지와 설명 활성화
        _questImage.SetActive(true);
        _questDescription.text = _questList[_currentQuestIndex].Description; // 완료된 퀘스트의 설명

        // 도장 이미지 활성화
        _badgeImage.SetActive(true);
        
        yield return new WaitForSecondsRealtime(3f); // 5초 대기
        
        // 도장 이미지 비활성화
        _questImage.SetActive(false);
        _badgeImage.SetActive(false);
        _goldAcquireEffect.PlayGoldAcquireEffect(_startPositionTransformOfEffect.position,
            _questList[_currentQuestIndex].Rewards[0].Quantity); // ���� ����Ʈ ����
        
        yield return new WaitForSecondsRealtime(3f); // 5초 대기

        _currentQuestIndex++; // 다음 퀘스트 인덱스 증가
        RegisterQuest(questSystem, _currentQuestIndex); // 다음 퀘스트 등록
    }

    public void CountOneQuestSuccess()
    {
        QuestSystem.Instance.ReceiveReport(_category, _target, 1);
    }
}
