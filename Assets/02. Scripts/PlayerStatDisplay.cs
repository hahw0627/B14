using UnityEngine;
using TMPro;

public class PlayerStatDisplay : MonoBehaviour
{
    public PlayerDataSO playerData;
    public TextMeshProUGUI damageText;

    private void OnEnable()
    {
        StatUpgrade.OnStatsChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        StatUpgrade.OnStatsChanged -= UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (playerData != null)
        {
            damageText.text = $"공격력 {playerData.Damage}";
        }
    }
}
