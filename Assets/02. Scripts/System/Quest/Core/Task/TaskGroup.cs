using System.Collections.Generic;
using System.Linq;
using Quest.Core.Task.Target.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.Core.Task
{
    public enum TaskGroupState
    {
        Inactive,
        Running,
        Complete
    }

    [System.Serializable]
    public class TaskGroup
    {
        [FormerlySerializedAs("tasks")]
        [SerializeField]
        private Task[] _tasks;

        public IReadOnlyList<Task> Tasks => _tasks;
        public Quest Owner { get; private set; }
        public bool IsAllTaskComplete => _tasks.All(x => x.IsComplete);
        public bool IsComplete => State == TaskGroupState.Complete;
        public TaskGroupState State { get; private set; }

        public TaskGroup(TaskGroup copyTarget)
        {
            _tasks = copyTarget.Tasks.Select(Object.Instantiate).ToArray();
        }

        public void Setup(Quest owner)
        {
            Owner = owner;
            foreach (var task in _tasks)
                task.Setup(owner);
        }

        public void Start()
        {
            State = TaskGroupState.Running;
            foreach (var task in _tasks)
                task.Start();
        }

        public void End()
        {
            foreach (var task in _tasks)
                task.End();
        }

        public void ReceiveReport(string category, object target, int successCount)
        {
            foreach (var task in  _tasks)
            {
                if (task.IsTarget(category, target))
                    task.ReceiveReport(successCount);
            }
        }

        public void Complete()
        {
            if (IsComplete)
                return;

            State = TaskGroupState.Complete;

            foreach (var task in _tasks)
            {
                if (!task.IsComplete)
                    task.Complete();
            }
        }

        public Task FindTaskByTarget(object target) => _tasks.FirstOrDefault(x => x.ContainsTarget(target));

        public Task FindTaskByTarget(TaskTarget target) => FindTaskByTarget(target.Value);

        public bool ContainsTarget(object target) => _tasks.Any(x => x.ContainsTarget(target));

        public bool ContainsTarget(TaskTarget target) => ContainsTarget(target.Value);
    }
}