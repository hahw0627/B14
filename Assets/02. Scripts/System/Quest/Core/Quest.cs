using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForCompletion
}

[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangedHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceledHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion

    [FormerlySerializedAs("category")]
    [SerializeField]
    private Category _category;
    [FormerlySerializedAs("icon")]
    [SerializeField]
    private Sprite _icon;

    [FormerlySerializedAs("codeName")]
    [Header("Text")]
    [SerializeField]
    private string _codeName;
    [FormerlySerializedAs("displayName")]
    [SerializeField]
    private string _displayName;
    [FormerlySerializedAs("description")]
    [SerializeField, TextArea]
    private string _description;

    [FormerlySerializedAs("taskGroups")]
    [Header("Task")]
    [SerializeField]
    private TaskGroup[] _taskGroups;

    [FormerlySerializedAs("rewards")]
    [Header("Reward")]
    [SerializeField]
    private Reward[] _rewards;

    [FormerlySerializedAs("useAutoComplete")]
    [Header("Option")]
    [SerializeField]
    private bool _useAutoComplete;
    [FormerlySerializedAs("isCancelable")]
    [SerializeField]
    private bool _isCancelable;
    [FormerlySerializedAs("isSavable")]
    [SerializeField]
    private bool _isSavable;

    [FormerlySerializedAs("acceptionConditions")]
    [Header("Condition")]
    [SerializeField]
    private Condition[] _acceptionConditions;
    [FormerlySerializedAs("cancelConditions")]
    [SerializeField]
    private Condition[] _cancelConditions;

    private int _currentTaskGroupIndex;

    public Category Category => _category;
    public Sprite Icon => _icon;
    public string CodeName => _codeName;
    public string DisplayName => _displayName;
    public string Description => _description;
    public QuestState State { get; private set; }
    public TaskGroup CurrentTaskGroup => _taskGroups[_currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => _taskGroups;
    public IReadOnlyList<Reward> Rewards => _rewards;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsCompletable => State == QuestState.WaitingForCompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => _isCancelable && _cancelConditions.All(x => x.IsPass(this));
    public bool IsAcceptable => _acceptionConditions.All(x => x.IsPass(this));
    public virtual bool IsSavable => _isSavable;

    public event TaskSuccessChangedHandler onTaskSuccessChanged;
    public event CompletedHandler onCompleted;
    public event CanceledHandler onCanceled;
    public event NewTaskGroupHandler onNewTaskGroup;

    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "This quest has already been registered.");

        foreach (var taskGroup in _taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
                task.onSuccessChanged += OnSuccessChanged;
        }

        State = QuestState.Running;
        CurrentTaskGroup.Start();
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegistered, "This quest has already been registered.");
        Debug.Assert(!IsCancel, "This quest has been canceled.");

        if (IsComplete)
            return;

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if (_currentTaskGroupIndex + 1 == _taskGroups.Length)
            {
                State = QuestState.WaitingForCompletion;
                if (_useAutoComplete)
                    Complete();
            }
            else
            {
                var prevTasKGroup = _taskGroups[_currentTaskGroupIndex++];
                prevTasKGroup.End();
                CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTasKGroup);
            }
        }
        else
            State = QuestState.Running;
    }

    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskGroup in _taskGroups)
            taskGroup.Complete();

        State = QuestState.Complete;

        foreach (var reward in _rewards)
            reward.Give(this);

        onCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onCompleted = null;
        onCanceled = null;
        onNewTaskGroup = null;
    }

    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(IsCancelable, "This quest can't be canceled");

        State = QuestState.Cancel;
        onCanceled?.Invoke(this);
    }

    public bool ContainsTarget(object target) => _taskGroups.Any(x => x.ContainsTarget(target));

    public bool ContainsTarget(TaskTarget target) => ContainsTarget(target.Value);

    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone._taskGroups = _taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            CodeName = _codeName,
            State = State,
            TaskGroupIndex = _currentTaskGroupIndex,
            TaskSuccessCounts = CurrentTaskGroup.Tasks.Select(x => x.CurrentSuccess).ToArray()
        };
    }

    public void LoadFrom(QuestSaveData saveData)
    {
        State = saveData.State;
        _currentTaskGroupIndex = saveData.TaskGroupIndex;

        for (var i = 0; i < _currentTaskGroupIndex; i++)
        {
            var taskGroup = _taskGroups[i];
            taskGroup.Start();
            taskGroup.Complete();
        }

        for (var i = 0; i < saveData.TaskSuccessCounts.Length; i++)
        {
            CurrentTaskGroup.Start();
            CurrentTaskGroup.Tasks[i].CurrentSuccess = saveData.TaskSuccessCounts[i];
        }
    }

    private void OnSuccessChanged(Task task, int currentSuccess, int prevSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);

    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "This quest has already been registered");
        Debug.Assert(!IsCancel, "This quest has been canceled.");
        Debug.Assert(!IsComplete, "This quest has already been completed");
    }
}
