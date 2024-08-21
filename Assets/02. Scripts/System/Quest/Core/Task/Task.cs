using System.Linq;
using Quest.Core.Task.Target.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.Core.Task
{
    public enum TaskState
    {
        Inactive,
        Running,
        Complete
    }

    [CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
    public class Task : ScriptableObject
    {
        #region Events
        public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);
        public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuccess);
        #endregion

        [FormerlySerializedAs("category")]
        [SerializeField]
        private Category _category;

        [FormerlySerializedAs("codeName")]
        [Header("Text")]
        [SerializeField]
        private string _codeName;
        [FormerlySerializedAs("description")]
        [SerializeField]
        private string _description;

        [FormerlySerializedAs("action")]
        [Header("Action")]
        [SerializeField]
        private TaskAction _action;

        [FormerlySerializedAs("targets")]
        [Header("Target")]
        [SerializeField]
        private TaskTarget[] _targets;

        [FormerlySerializedAs("initialSuccessValue")]
        [Header("Setting")]
        [SerializeField]
        private InitialSuccessValue _initialSuccessValue;
        [FormerlySerializedAs("needSuccessToComplete")]
        [SerializeField]
        private int _needSuccessToComplete;
        [FormerlySerializedAs("canReceiveReportsDuringCompletion")]
        [SerializeField]
        private bool _canReceiveReportsDuringCompletion;

        private TaskState _state;
        private int _currentSuccess;

        public event StateChangedHandler onStateChanged;
        public event SuccessChangedHandler onSuccessChanged;

        public int CurrentSuccess
        {
            get => _currentSuccess;
            set
            {
                var prevSuccess = _currentSuccess;
                _currentSuccess = Mathf.Clamp(value, 0, _needSuccessToComplete);
                if (_currentSuccess == prevSuccess) return;
                State = _currentSuccess == _needSuccessToComplete ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, _currentSuccess, prevSuccess);
            }
        }
        public Category Category => _category;
        public string CodeName => _codeName;
        public string Description => _description;
        public int NeedSuccessToComplete => _needSuccessToComplete;
        public TaskState State
        {
            get => _state;
            set
            {
                var prevState = _state;
                _state = value;
                onStateChanged?.Invoke(this, _state, prevState);
            }
        }
        public bool IsComplete => State == TaskState.Complete;
        public Quest Owner { get; private set; }

        public void Setup(Quest owner)
        {
            Owner = owner;
        }

        public void Start()
        {
            State = TaskState.Running;
            if (_initialSuccessValue)
                CurrentSuccess = _initialSuccessValue.GetValue(this);
        }

        public void End()
        {
            onStateChanged = null;
            onSuccessChanged = null;
        }

        public void ReceiveReport(int successCount)
        {
            CurrentSuccess = _action.Run(this, CurrentSuccess, successCount);
        }

        public void Complete()
        {
            CurrentSuccess = _needSuccessToComplete;
        }

        public bool IsTarget(string category, object target)
            => Category == category &&
               _targets.Any(x => x.IsEqual(target)) &&
               (!IsComplete || (IsComplete && _canReceiveReportsDuringCompletion));

        public bool ContainsTarget(object target) => _targets.Any(x => x.IsEqual(target));
    }
}