using Quest.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Quest
{
    public class QuestItemUIController : MonoBehaviour
    {
        public Image RewardIcon;
        public TextMeshProUGUI DescriptionTmp;
        public TextMeshProUGUI RewardGoldTmp;
        public TextMeshProUGUI SuccessConditionTmp;

        public Button ClaimRewardButton;

        private QuestDatabase _questDatabase;

        private void Awake()
        {
            _questDatabase = ScriptableObject.CreateInstance<QuestDatabase>();
        }

        public void Setup(global::Quest.Core.Quest quest)
        {
            RewardIcon.sprite = quest.Rewards[0].Icon;
            DescriptionTmp.text = quest.Description;
            RewardGoldTmp.text = $"{quest.Rewards[0].Quantity}G";
            SuccessConditionTmp.text = $"{quest.TaskGroups[0].Tasks[0].CurrentSuccess} / {quest.TaskGroups[0].Tasks[0].NeedSuccessToComplete}회";

            // 보상 버튼 활성화 여부 설정
            ClaimRewardButton.gameObject.SetActive(quest.IsComplete);
            ClaimRewardButton.onClick.AddListener(ClaimReward);
        }

        private void ClaimReward()
        {
            if (ClaimRewardButton == null) return;
            ClaimRewardButton.GetComponent<Image>().sprite = null;
            ClaimRewardButton.GetComponentInChildren<TextMeshProUGUI>().text = "완료";
        }
    }
}