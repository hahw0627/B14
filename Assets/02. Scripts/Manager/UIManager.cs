using TMPro;

public class UIManager : SingletonDestroyable<UIManager>
{
    public TextMeshProUGUI GoldTMP;

    public void UpdateCurrencyUI()
    {
        GoldTMP.text = DataManager.Instance.playerDataSO.Gold.ToString(); 
    }


}
