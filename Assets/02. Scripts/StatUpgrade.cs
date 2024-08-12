using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatUpgrade : MonoBehaviour
{
    public PlayerDataSO PlayerData;

    [Header("Label")]
    public Text AttackTmp;
    public Text AttackSpeedTmp;
    public Text HpTmp;
    public Text RecoverHpTmp;
    public Text CriticalPercentTmp;
    public Text CriticalDamageTmp;
    
    [Header("Cost Label")]
    public Text AttackCostTmp;
    public Text AttackSpeedCostTmp;
    public Text HpCostTmp;
    public Text RecoverHpCostTmp;
    public Text CriticalPercentCostTmp;
    public Text CriticalDamageCostTmp;

    [Header("Upgrade Button")]
    public Button AttackBtn;
    public Button AttackSpeedBtn;
    public Button HpBtn;
    public Button RecoverHpBtn;
    public Button CriticalPercentBtn;
    public Button CriticalDamageBtn;

    private int _attackCost = 1;
    private int _hpCost = 1;
    private int _recoverHpCost = 3;
    private int _attackSpeedCost = 10;
    private int _criticalPercentCost = 100;
    private int _criticalDamageCost = 50;

    public static event System.Action onStatsChanged;
    private Coroutine _currentCoroutine;
    private float _holdTime = 0.5f; // 롱 터치로 인식할 시간
    private float _upgradeInterval = 0.1f; // 연속 업그레이드 간격

    private void Start()
    {
        UpdateUI();
        SetupButton(AttackBtn, () => UpgradeStat(ref PlayerData.Damage, 2, ref _attackCost, AttackTmp, "공격력 : ", AttackCostTmp));
        SetupButton(HpBtn, () => UpgradeStat(ref PlayerData.Hp, 100, ref _hpCost, HpTmp, "체력 : ", HpCostTmp));
        SetupButton(RecoverHpBtn, () => UpgradeStat(ref PlayerData.HpRecovery, 100, ref _recoverHpCost, RecoverHpTmp, "체력회복량 : ", RecoverHpCostTmp));
        SetupButton(AttackSpeedBtn, () => UpgradeStat(ref PlayerData.AttackSpeed, 0.5f, ref _attackSpeedCost, AttackSpeedTmp, "공격속도 : ", AttackSpeedCostTmp));
        SetupButton(CriticalPercentBtn, () => UpgradeCriticalPercent());
        SetupButton(CriticalDamageBtn, () => UpgradeStat(ref PlayerData.CriticalMultiplier, 0.003f, ref _criticalDamageCost, CriticalDamageTmp, "치명타데미지 : ", CriticalDamageCostTmp));
    }

    private void SetupButton(Button button, System.Action upgradeAction)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => { _currentCoroutine = StartCoroutine(LongPressCoroutine(upgradeAction)); });
        trigger.triggers.Add(pointerDown);

        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((data) => { if (_currentCoroutine != null) StopCoroutine(_currentCoroutine); });
        trigger.triggers.Add(pointerUp);

        button.onClick.AddListener(() => upgradeAction());
    }

    private IEnumerator LongPressCoroutine(System.Action upgradeAction)
    {
        yield return new WaitForSeconds(_holdTime);

        while (true)
        {
            upgradeAction();
            yield return new WaitForSeconds(_upgradeInterval);
        }
    }

    private void UpgradeStat(ref int stat, int increment, ref int cost, Text statTmp, string statName, Text costTmp)
    {
        if (PlayerData.Gold < cost) return;
        stat += increment;

        PlayerData.Gold -= cost;
        cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
        statTmp.text = statName +stat;
        costTmp.text = "Upgrade\n"+ cost;
        UpdateUI();
        onStatsChanged?.Invoke();
    }

    private void UpgradeCriticalPercent()
    {
        if (PlayerData.CriticalPer >= 100f)
        {
            CriticalPercentBtn.interactable = false;
            return;
        }

        float newValue = Mathf.Min(PlayerData.CriticalPer + 1.0f, 100f);
        UpgradeStat(ref PlayerData.CriticalPer, newValue - PlayerData.CriticalPer, ref _criticalPercentCost, CriticalPercentTmp, "치명타확률 : ", CriticalPercentCostTmp);

        if (PlayerData.CriticalPer >= 100f)
        {
            CriticalPercentBtn.interactable = false;
        }
    }

    private void UpgradeStat(ref float stat, float increment, ref int cost, Text statTmp, string statName, Text costTmp)
    {
        if (PlayerData.Gold < cost) return;
        stat += increment;

        PlayerData.Gold -= cost;
        cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
        statTmp.text = statName + stat;
        costTmp.text = cost.ToString();
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        AttackTmp.text = "공격력\n" + PlayerData.Damage;
        AttackSpeedTmp.text = "공격속도\n" + PlayerData.AttackSpeed+"%";
        HpTmp.text = "체력\n" + PlayerData.Hp;
        RecoverHpTmp.text = "체력 회복량\n" + PlayerData.HpRecovery;
        CriticalPercentTmp.text = "치명타 확률\n" + PlayerData.CriticalPer.ToString("F1") + "%";
        CriticalPercentBtn.interactable = PlayerData.CriticalPer < 100f;
        CriticalDamageTmp.text = "치명타 데미지\n" + (PlayerData.CriticalMultiplier * 100) +"%";

        AttackCostTmp.text = _attackCost.ToString();
        AttackSpeedCostTmp.text = _attackSpeedCost.ToString();
        HpCostTmp.text = _hpCost.ToString();
        RecoverHpCostTmp.text = _recoverHpCost.ToString();
        CriticalPercentCostTmp.text = _criticalPercentCost.ToString();
        CriticalDamageCostTmp.text = _criticalDamageCost.ToString();
    
        UIManager.Instance.UpdateCurrencyUI();    
    }
}
