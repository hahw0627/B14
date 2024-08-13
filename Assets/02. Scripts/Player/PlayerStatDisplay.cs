using UnityEngine;
using TMPro;

public class PlayerStatDisplay : MonoBehaviour
{
    public PlayerDataSO PlayerData;
    public TextMeshProUGUI DamageText;

    private void OnEnable()
    {
        StatUpgrade.onStatsChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        StatUpgrade.onStatsChanged -= UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (PlayerData != null)
        {
            DamageText.text = $"공격력 {PlayerData.Damage}";
        }
    }
}
