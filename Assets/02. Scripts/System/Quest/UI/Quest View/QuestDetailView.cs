using System.Collections.Generic;
using Quest.Core.Task;
using Quest.UI.Quest_Tracker;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quest.UI.Quest_View
{
    public class QuestDetailView : MonoBehaviour
    {
        [FormerlySerializedAs("displayGroup")]
        [SerializeField]
        private GameObject _displayGroup;
        [FormerlySerializedAs("cancelButton")]
        [SerializeField]
        private Button _cancelButton;

        [FormerlySerializedAs("title")]
        [Header("Quest Description")]
        [SerializeField]
        private TextMeshProUGUI _title;
        [FormerlySerializedAs("description")]
        [SerializeField]
        private TextMeshProUGUI _description;

        [FormerlySerializedAs("taskDescriptorGroup")]
        [Header("Task Description")]
        [SerializeField]
        private RectTransform _taskDescriptorGroup;
        [FormerlySerializedAs("taskDescriptorPrefab")]
        [SerializeField]
        private TaskDescriptor _taskDescriptorPrefab;
        [FormerlySerializedAs("taskDescriptorPoolCount")]
        [SerializeField]
        private int _taskDescriptorPoolCount;

        [FormerlySerializedAs("rewardDescriptionGroup")]
        [Header("Reward Description")]
        [SerializeField]
        private RectTransform _rewardDescriptionGroup;
        [FormerlySerializedAs("rewardDescriptionPrefab")]
        [SerializeField]
        private TextMeshProUGUI _rewardDescriptionPrefab;
        [FormerlySerializedAs("rewardDescriptionPoolCount")]
        [SerializeField]
        private int _rewardDescriptionPoolCount;

        private List<TaskDescriptor> _taskDescriptorPool;
        private List<TextMeshProUGUI> _rewardDescriptionPool;

        public Core.Quest Target { get; private set; }

        private void Awake()
        {
            _taskDescriptorPool = CreatePool(_taskDescriptorPrefab, _taskDescriptorPoolCount, _taskDescriptorGroup);
            _rewardDescriptionPool = CreatePool(_rewardDescriptionPrefab, _rewardDescriptionPoolCount, _rewardDescriptionGroup);
            _displayGroup.SetActive(false);
        }

        private void Start()
        {
            _cancelButton.onClick.AddListener(CancelQuest);
        }

        private List<T> CreatePool<T>(T prefab, int count, RectTransform parent)
            where T : MonoBehaviour
        {
            var pool = new List<T>(count);
            for (int i = 0; i < count; i++)
                pool.Add(Instantiate(prefab, parent));
            return pool;
        }

        private void CancelQuest()
        {
            if (Target.IsCancelable)
                Target.Cancel();
        }

        private void ShowTasks(Core.Quest quest)
        {
            var taskIndex = 0;
            foreach (var taskGroup in quest.TaskGroups)
            {
                foreach (var task in taskGroup.Tasks)
                {
                    var poolObject = _taskDescriptorPool[taskIndex++];
                    poolObject.gameObject.SetActive(true);
 
                    if (taskGroup.IsComplete)
                        poolObject.UpdateTextUsingStrikeThrough(task);
                    else if (taskGroup == quest.CurrentTaskGroup)
                        poolObject.UpdateText(task);
                    else
                        poolObject.UpdateText("● ??????????");
                }
            }
        }
        
        private void OnTaskSuccessChanged(Core.Quest quest, Task task, int currentSuccess, int prevSuccess)
            => ShowTasks(quest);
        
        public void Show(Core.Quest quest)
        {
            _displayGroup.SetActive(true);
 
            if (Target is not null)
                Target.onTaskSuccessChanged -= OnTaskSuccessChanged;
            Target = quest;
            Target.onTaskSuccessChanged += OnTaskSuccessChanged;

            _title.text = quest.DisplayName;
            _description.text = quest.Description;

            ShowTasks(Target);
            
            int taskIndex = 0;
            foreach (var taskGroup in quest.TaskGroups)
            {
                foreach (var task in taskGroup.Tasks)
                {
                    var poolObject = _taskDescriptorPool[taskIndex++];
                    poolObject.gameObject.SetActive(true);

                    if (taskGroup.IsComplete)
                        poolObject.UpdateTextUsingStrikeThrough(task);
                    else if (taskGroup == quest.CurrentTaskGroup)
                        poolObject.UpdateText(task);
                    else
                        poolObject.UpdateText("�� ??????????");
                }
            }

            for (int i = taskIndex; i < _taskDescriptorPool.Count; i++)
                _taskDescriptorPool[i].gameObject.SetActive(false);

            var rewards = quest.Rewards;
            var rewardCount = rewards.Count;
            for (int i = 0; i < _rewardDescriptionPoolCount; i++)
            {
                var poolObject = _rewardDescriptionPool[i];
                if (i < rewardCount)
                {
                    var reward = rewards[i];
                    poolObject.text = $"�� {reward.Description} +{reward.Quantity}";
                    poolObject.gameObject.SetActive(true);
                }
                else
                    poolObject.gameObject.SetActive(false);
            }

            _cancelButton.gameObject.SetActive(quest.IsCancelable && !quest.IsComplete);
        }

        public void Hide()
        {
            if (Target != null)
                Target.onTaskSuccessChanged -= OnTaskSuccessChanged;
            Target = null;
            _displayGroup.SetActive(false);
            _cancelButton.gameObject.SetActive(false);
        }
    }
}
