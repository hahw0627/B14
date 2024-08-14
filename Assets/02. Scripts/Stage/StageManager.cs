using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StageManager : SingletonDestroyable<StageManager>
{
    public event Action<int, int> onStageChanged;

    [SerializeField]
    private TextMeshProUGUI _stageTmp;

    public StageDataSO StageDataSO;

    [SerializeField]
    private MonsterSpawner _monsterSpawner;

    protected override void Awake()
    {
        onStageChanged -= UpdateStageDisplay;
    }

    private void Start()
    {
        UpdateStageDisplay(StageDataSO.Stage, StageDataSO.StagePage);
        onStageChanged += UpdateStageDisplay;
        
        //if (StageDataSO.Stage == 1 && StageDataSO.StagePage == 0)
        //{
        //    StartCoroutine(WaitTime(4.0f));
        //    StartCoroutine(_monsterSpawner.CheckMonsters());
        //}
        StartCoroutine(_monsterSpawner.CheckMonsters());
    }

    public IEnumerator WaitTime(float num)
    {
        yield return new WaitForSeconds(num);
    }

    public void ChangeStage(int newStage, int newStagePage)
    {
        onStageChanged?.Invoke(newStage, newStagePage);
    }

    private void UpdateStageDisplay(int newStage, int newStagePage)
    {
        _stageTmp.text = $"스테이지 {newStage}-{newStagePage}";
    }
}