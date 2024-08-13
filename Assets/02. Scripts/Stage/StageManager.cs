using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager; // PlayerSO를 참조합니다.

    [SerializeField]
    private TextMeshProUGUI _stageTmp;

    private void Start()
    {
        UpdateStageDisplay(_gameManager.Stage, _gameManager.StagePage);
        _gameManager.onStageChanged += UpdateStageDisplay; // 이벤트 구독
    }

    private void Awake()
    {
        _gameManager.onStageChanged -= UpdateStageDisplay; // 이벤트 구독 해제
    }

    private void UpdateStageDisplay(int newStage, int newStagePage)
    {
        _stageTmp.text = $"스테이지 {newStage}-{newStagePage}";
    }
}