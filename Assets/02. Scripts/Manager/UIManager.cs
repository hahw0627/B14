using System.Diagnostics;
using TMPro;
using UnityEngine;

public class UIManager : SingletonDestroyable<UIManager>
{
    public TextMeshProUGUI GoldTMP;

    private void Start()
    {
        UpdateCurrencyUI();
    }

    public void UpdateCurrencyUI()
    {
        if(GoldTMP != null && DataManager.Instance != null && DataManager.Instance.playerDataSO != null)
        {
            GoldTMP.text = DataManager.Instance.playerDataSO.Gold.ToString();
        }
    }


}
