using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIManager : SingletonDestroyable<UIManager>, IPointerDownHandler
{
    public TextMeshProUGUI GoldTMP;
    public TextMeshProUGUI GemTMP;


    private void Start()
    {
        UpdateCurrencyUI();
    }

    public void UpdateCurrencyUI()
    {
        if (GoldTMP is null || GemTMP is null || DataManager.Instance is null ||
            DataManager.Instance.playerDataSO is null) return;
        GoldTMP.text = DataManager.Instance.playerDataSO.Gold.ToString();
        GemTMP.text = DataManager.Instance.playerDataSO.Gem.ToString();
    }

    public ParticleSystem ClickParticle;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Camera.main == null) return;
        var touchPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        touchPosition.z = 0;
        var newParticle = Instantiate(ClickParticle, touchPosition, Quaternion.identity);
    }
}