using System.Collections.Generic;
using System.Linq;
using Quest.Core.Task;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.UI.Quest_Tracker
{
    public class QuestTracker : MonoBehaviour
    {
        [FormerlySerializedAs("questTitleText")]
        [SerializeField]
        private TextMeshProUGUI _questTitleText;
        [FormerlySerializedAs("taskDescriptorPrefab")]
        [SerializeField]
        private TaskDescriptor _taskDescriptorPrefab;

        private readonly Dictionary<Task, TaskDescriptor> _taskDescriptorsByTask = new();

        private Core.Quest _targetQuest;

        private void OnDestroy()
        {
            if (_targetQuest != null)
            {
                _targetQuest.onNewTaskGroup -= UpdateTaskDescriptors;
                _targetQuest.onCompleted -= DestroySelf;
            }

            foreach (var task in _taskDescriptorsByTask.Select(tuple => tuple.Key))
            {
                task.onSuccessChanged -= UpdateText;
            }
        }

        public void Setup(Core.Quest targetQuest, Color titleColor)
        {
            _targetQuest = targetQuest;

            _questTitleText.text = targetQuest.Category == null ?
                targetQuest.DisplayName :
                $"[{targetQuest.Category.DisplayName}] {targetQuest.DisplayName}";

            _questTitleText.color = titleColor;

            targetQuest.onNewTaskGroup += UpdateTaskDescriptors;
            targetQuest.onCompleted += DestroySelf;

            IReadOnlyList<TaskGroup> taskGroups = targetQuest.TaskGroups;
            UpdateTaskDescriptors(targetQuest, taskGroups[0]);

            if (taskGroups[0] == targetQuest.CurrentTaskGroup) return;
            for (var i = 1; i < taskGroups.Count; i++)
            {
                var taskGroup = taskGroups[i];
                UpdateTaskDescriptors(targetQuest, taskGroup, taskGroups[i - 1]);

                if (taskGroup == targetQuest.CurrentTaskGroup)
                    break;
            }
        }

        private void UpdateTaskDescriptors(Core.Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup = null)
        {
            foreach (var task in currentTaskGroup.Tasks)
            {
                var taskDescriptor = Instantiate(_taskDescriptorPrefab, transform);
                taskDescriptor.UpdateText(task);
                task.onSuccessChanged += UpdateText;

                _taskDescriptorsByTask.Add(task, taskDescriptor);
            }

            if (prevTaskGroup == null) return;
            {
                foreach (var task in prevTaskGroup.Tasks)
                {
                    var taskDescriptor = _taskDescriptorsByTask[task];
                    taskDescriptor.UpdateTextUsingStrikeThrough(task);
                }
            }
        }

        private void UpdateText(Task task, int currentSuccess, int prevSuccess)
        {
            _taskDescriptorsByTask[task].UpdateText(task);
        }

        private void DestroySelf(Core.Quest quest)
        {
            Destroy(gameObject);
        }
    }
}
