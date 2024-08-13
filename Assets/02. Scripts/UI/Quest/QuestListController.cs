using System;
using Quest.Core;
using UnityEngine;

namespace UI.Quest
{
    public class QuestListController : MonoBehaviour
    {
        public GameObject QuestItemPrefab;
        public Transform QuestListParent;

        [SerializeField]
        private QuestDatabase _questDatabase;
        
        private void Start()
        {
            var questSystem = QuestSystem.Instance;
            UpdateQuestList();
        }

        private void UpdateQuestList()
        {
            // 기존 아이템 제거
            foreach (Transform child in QuestListParent)
            {
                Destroy(child.gameObject);
            }

            // 퀘스트 아이템 UI 생성 및 설정
            foreach (var quest in _questDatabase.Quests)
            {
                var questItem = Instantiate(QuestItemPrefab, QuestListParent);
                var questItemUI = questItem.GetComponent<QuestItemUIController>();
                questItemUI.Setup(quest); // 퀘스트 정보 설정
            }
        }
    }
}