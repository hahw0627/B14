using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIManager : SingletonDestroyable<UIManager>, IPointerDownHandler
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
    public ParticleSystem clickParticle;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        touchPosition.z = 0;
        ParticleSystem newParticle = Instantiate(clickParticle, touchPosition, Quaternion.identity);


    }

}
