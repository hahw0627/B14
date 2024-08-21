namespace Quest.Core
{
    public struct QuestSaveData
    {
        public string CodeName;
        public QuestState State;
        public int TaskGroupIndex;
        public int[] TaskSuccessCounts;
    }
}