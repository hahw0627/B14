using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI textMeshProUGUI;

    public void UpdateCurrencyUI()
    {
        textMeshProUGUI.text =DataManager.Instance.playerDataSO.Gold.ToString(); 
    }
}
