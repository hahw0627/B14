using System.Collections;
using System.Collections.Generic;
using System.Text;
using Quest.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.UI
{
    public class QuestCompletionNotifier : MonoBehaviour
    {
        [FormerlySerializedAs("titleDescription")]
        [SerializeField]
        private string _titleDescription;

        [FormerlySerializedAs("titleText")]
        [SerializeField]
        private TextMeshProUGUI _titleText;

        [FormerlySerializedAs("rewardText")]
        [SerializeField]
        private TextMeshProUGUI _rewardText;

        [FormerlySerializedAs("showTime")]
        [SerializeField]
        private float _showTime = 3f;

        private readonly Queue<Core.Quest> _reservedQuests = new();
        private readonly StringBuilder _stringBuilder = new();

        private void Start()
        {
            var questSystem = QuestSystem.Instance;
            questSystem.onQuestCompleted += Notify;
            questSystem.onAchievementCompleted += Notify;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            var questSystem = QuestSystem.Instance;
            if (questSystem == null) return;
            questSystem.onQuestCompleted -= Notify;
            questSystem.onAchievementCompleted -= Notify;
        }

        private void Notify(Core.Quest quest)
        {
            _reservedQuests.Enqueue(quest);

            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);
            StartCoroutine(nameof(ShowNotice));
        }

        private IEnumerator ShowNotice()
        {
            var waitSeconds = new WaitForSeconds(_showTime);

            Core.Quest quest;
            while (_reservedQuests.TryDequeue(out quest))
            {
                _titleText.text = _titleDescription.Replace("%{dn}", quest.DisplayName);
                foreach (var reward in quest.Rewards)
                {
                    _stringBuilder.Append(reward.Description);
                    _stringBuilder.Append(" ");
                    _stringBuilder.Append(reward.Quantity);
                    _stringBuilder.Append(" ");
                }

                _rewardText.text = _stringBuilder.ToString();
                _stringBuilder.Clear();

                yield return waitSeconds;
            }

            gameObject.SetActive(false);
        }
    }
}