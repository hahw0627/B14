using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Quest.Core.Task.Target.Base;
using UnityEngine;

namespace Quest.Core
{
    public class QuestSystem : MonoBehaviour
    {
        #region Save Path
        private const string K_SAVE_ROOT_PATH = "questSystem";
        private const string K_ACTIVE_QUESTS_SAVE_PATH = "activeQuests";
        private const string K_COMPLETED_QUESTS_SAVE_PATH = "completedQuests";
        private const string K_ACTIVE_ACHIEVEMENTS_SAVE_PATH = "activeAchievement";
        private const string K_COMPLETED_ACHIEVEMENTS_SAVE_PATH = "completedAchievement";
        #endregion

        #region Events
        public delegate void QuestRegisteredHandler(Quest newQuest);
        public delegate void QuestCompletedHandler(Quest quest);
        public delegate void QuestCanceledHandler(Quest quest);
        #endregion

        private static QuestSystem s_instance;
        private static bool s_isApplicationQuitting;

        public static QuestSystem Instance
        {
            get
            {
                if (s_isApplicationQuitting || s_instance) return s_instance;
                s_instance = FindObjectOfType<QuestSystem>();
            
                if (s_instance) return s_instance;
                s_instance = new GameObject("Quest System").AddComponent<QuestSystem>();
                DontDestroyOnLoad(s_instance.gameObject);
            
                return s_instance;
            }
        }

        private readonly List<Quest> _activeQuests = new();
        private readonly List<Quest> _completedQuests = new();

        private readonly List<Quest> _activeAchievements = new();
        private readonly List<Quest> _completedAchievements = new();

        private QuestDatabase _questDatabase;
        private QuestDatabase _achievementDatabase;

        public event QuestRegisteredHandler onQuestRegistered;
        public event QuestCompletedHandler onQuestCompleted;
        public event QuestCanceledHandler onQuestCanceled;

        public event QuestRegisteredHandler onAchievementRegistered;
        public event QuestCompletedHandler onAchievementCompleted;

        public IReadOnlyList<Quest> ActiveQuests => _activeQuests;
        public IReadOnlyList<Quest> CompletedQuests => _completedQuests;
        public IReadOnlyList<Quest> ActiveAchievements => _activeAchievements;
        public IReadOnlyList<Quest> CompletedAchievements => _completedAchievements;

        private void Awake()
        {
            _questDatabase = Resources.Load<QuestDatabase>("QuestDatabase");
            _achievementDatabase = Resources.Load<QuestDatabase>("AchievementDatabase");

            if (Load()) return;
            foreach (var achievement in _achievementDatabase.Quests)
                Register(achievement);
        }

        private void OnApplicationQuit()
        {
            s_isApplicationQuitting = true;
            Save();
        }

        public Quest Register(Quest quest)
        {
            var newQuest = quest.Clone();

            newQuest.onCompleted += OnQuestCompleted;
            newQuest.onCanceled += OnQuestCanceled;

            _activeQuests.Add(newQuest);

            newQuest.OnRegister();
            onQuestRegistered?.Invoke(newQuest);

            return newQuest;
        }

        private void ReceiveReport(string category, object target, int successCount)
        {
            ReceiveReport(_activeQuests, category, target, successCount);
            ReceiveReport(_activeAchievements, category, target, successCount);
        }

        public void ReceiveReport(Category category, TaskTarget target, int successCount)
            => ReceiveReport(category.CodeName, target.Value, successCount);

        private static void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
        {
            foreach (var quest in quests.ToArray())
                quest.ReceiveReport(category, target, successCount);
        }

        public void CompleteWaitingQuests()
        {
            foreach (var quest in _activeQuests.ToList().Where(quest => quest.IsCompletable))
            {
                quest.Complete();
            }
        }

        public bool ContainsInActiveQuests(Quest quest) => _activeQuests.Any(x => x.CodeName == quest.CodeName);

        public bool ContainsInCompleteQuests(Quest quest) => _completedQuests.Any(x => x.CodeName == quest.CodeName);

        public bool ContainsInActiveAchievements(Quest quest) => _activeAchievements.Any(x => x.CodeName == quest.CodeName);

        public bool ContainsInCompletedAchievements(Quest quest) => _completedAchievements.Any(x => x.CodeName == quest.CodeName);

        public void Save()
        {
            var root = new JObject
            {
                { K_ACTIVE_QUESTS_SAVE_PATH, CreateSaveData(_activeQuests) },
                { K_COMPLETED_QUESTS_SAVE_PATH, CreateSaveData(_completedQuests) },
                { K_ACTIVE_ACHIEVEMENTS_SAVE_PATH, CreateSaveData(_activeAchievements) },
                { K_COMPLETED_ACHIEVEMENTS_SAVE_PATH, CreateSaveData(_completedAchievements) }
            };

            PlayerPrefs.SetString(K_SAVE_ROOT_PATH, root.ToString());
            PlayerPrefs.Save();
        }

        public bool Load()
        {
            if (!PlayerPrefs.HasKey(K_SAVE_ROOT_PATH)) return false;
            var root = JObject.Parse(PlayerPrefs.GetString(K_SAVE_ROOT_PATH));

            LoadSaveData(root[K_ACTIVE_QUESTS_SAVE_PATH], _questDatabase, LoadActiveQuest);
            LoadSaveData(root[K_COMPLETED_QUESTS_SAVE_PATH], _questDatabase, LoadCompletedQuest);

            LoadSaveData(root[K_ACTIVE_ACHIEVEMENTS_SAVE_PATH], _achievementDatabase, LoadActiveQuest);
            LoadSaveData(root[K_COMPLETED_ACHIEVEMENTS_SAVE_PATH], _achievementDatabase, LoadCompletedQuest);

            return true;

        }

        private static JArray CreateSaveData(IReadOnlyList<Quest> quests)
        {
            var saveData = new JArray();
            foreach (var quest in quests)
            {
                if (quest.IsSavable)
                    saveData.Add(JObject.FromObject(quest.ToSaveData()));
            }
            return saveData;
        }

        private static void LoadSaveData(JToken dataToken, QuestDatabase database, Action<QuestSaveData, Quest> onSuccess)
        {
            var data = dataToken as JArray;
            foreach (var datum in data!)
            {
                var saveData = datum.ToObject<QuestSaveData>();
                var quest = database.FindQuestBy(saveData.CodeName);
                onSuccess.Invoke(saveData, quest);
            }
        }
    
        private void LoadActiveQuest(QuestSaveData saveData, Quest quest)
        {
            var newQuest = Register(quest);
            newQuest.LoadFrom(saveData);
        }

        private void LoadCompletedQuest(QuestSaveData saveData, Quest quest)
        {
            var newQuest = quest.Clone();
            newQuest.LoadFrom(saveData);

            _completedQuests.Add(newQuest);
        }

        #region Callback
        private void OnQuestCompleted(Quest quest)
        {
            _activeQuests.Remove(quest);
            _completedQuests.Add(quest);

            onQuestCompleted?.Invoke(quest);
        }

        private void OnQuestCanceled(Quest quest)
        {
            _activeQuests.Remove(quest);
            onQuestCanceled?.Invoke(quest);

            Destroy(quest, Time.deltaTime);
        }

        private void OnAchievementCompleted(Quest achievement)
        {
            _activeAchievements.Remove(achievement);
            _completedAchievements.Add(achievement);

            onAchievementCompleted?.Invoke(achievement);
        }
        #endregion
    }
}
