using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatUpgrade : MonoBehaviour
{
    public PlayerDataSO PlayerData;
    public StatDataSO StatData;

    [Header("Label")]
    public TextMeshProUGUI AttackTmp;

    public TextMeshProUGUI AttackSpeedTmp;
    public TextMeshProUGUI MaxHpTmp;
    public TextMeshProUGUI RecoverHpTmp;
    public TextMeshProUGUI CriticalPercentTmp;
    public TextMeshProUGUI CriticalDamageTmp;

    [Header("Cost Label")]
    public TextMeshProUGUI AttackCostTmp;

    public TextMeshProUGUI AttackSpeedCostTmp;
    public TextMeshProUGUI HpCostTmp;
    public TextMeshProUGUI RecoverHpCostTmp;
    public TextMeshProUGUI CriticalPercentCostTmp;
    public TextMeshProUGUI CriticalDamageCostTmp;

    [Header("Upgrade Button")]
    public Button AttackBtn;

    public Button AttackSpeedBtn;
    public Button HpBtn;
    public Button RecoverHpBtn;
    public Button CriticalPercentBtn;
    public Button CriticalDamageBtn;

    private long _attackCost = 1;
    private long _maxHpCost = 1;
    private long _recoverHpCost = 3;
    private long _attackSpeedCost = 10;
    private long _criticalPercentCost = 100;
    private long _criticalMultiplierCost = 50;

    public static event System.Action onStatsChanged;
    private Coroutine _currentCoroutine;
    private const float HOLD_TIME = 0.5f; // 롱 터치로 인식할 시간
    private const float UPGRADE_INTERVAL = 0.1f; // 연속 업그레이드 간격

    public void SaveCost()
    {
        Debug.Log("SaveCost");
        //코스트 저장 
        StatData.AttackCost = _attackCost;
        StatData.AttackSpeedCost = _attackSpeedCost;
        StatData.MaxHpCost = _maxHpCost;
        StatData.HpRecoveryCost = _recoverHpCost;
        StatData.CriticalPercentageCost = _criticalPercentCost;
        StatData.CriticalMultiplierCost = _criticalMultiplierCost;
    }

    public void LoadCost()
    {
        Debug.Log("LoadCost");
        _attackCost = StatData.AttackCost;
        _maxHpCost = StatData.AttackSpeedCost;
        _recoverHpCost = StatData.MaxHpCost;
        _attackSpeedCost = StatData.HpRecoveryCost;
        _criticalPercentCost = StatData.CriticalPercentageCost;
        _criticalMultiplierCost = StatData.CriticalMultiplierCost;
    }

    public void OnApplicationQuit()
    {
        SaveCost();
    }

    private void Update()
    {
        AttackBtn.interactable = PlayerData.Gold >= _attackCost;
        AttackSpeedBtn.interactable = PlayerData.Gold >= _attackSpeedCost;
        HpBtn.interactable = PlayerData.Gold >= _maxHpCost;
        RecoverHpBtn.interactable = PlayerData.Gold >= _recoverHpCost;
        CriticalDamageBtn.interactable = PlayerData.Gold >= _criticalMultiplierCost;
        CriticalPercentBtn.interactable = PlayerData.Gold >= _criticalPercentCost;
    }

    private void Start()
    {
        if (SaveLoadManager.ExistJson(SaveLoadManager.Instance.StatSavePath))
            LoadCost();

        UpdateUI();
        SetupButton(AttackBtn,
            () => UpgradeStat(ref PlayerData.Damage, 2, ref _attackCost, AttackTmp, "공격력 : ", AttackCostTmp));
        SetupButton(HpBtn,
            () => UpgradeStat(ref PlayerData.MaxHp, 100, ref _maxHpCost, MaxHpTmp, "최대 체력 : ", HpCostTmp));
        SetupButton(RecoverHpBtn,
            () => UpgradeStat(ref PlayerData.HpRecovery, 3, ref _recoverHpCost, RecoverHpTmp, "체력회복량 : ",
                RecoverHpCostTmp));
        SetupButton(AttackSpeedBtn,
            () => UpgradeStat(ref PlayerData.AttackSpeed, 0.005f, ref _attackSpeedCost, AttackSpeedTmp, "공격속도 : ",
                AttackSpeedCostTmp)); // 최대 3번 공격
        SetupButton(CriticalDamageBtn,
            () => UpgradeStat(ref PlayerData.CriticalMultiplier, 0.003f, ref _criticalMultiplierCost, CriticalDamageTmp,
                "치명타데미지 : ", CriticalDamageCostTmp));
        SetupButton(CriticalPercentBtn, UpgradeCriticalPercent);
    }

    private void SetupButton(Button button, System.Action upgradeAction)
    {
        var trigger = button.gameObject.AddComponent<EventTrigger>();

        var pointerDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDown.callback.AddListener((_) =>
        {
            _currentCoroutine = StartCoroutine(LongPressCoroutine(upgradeAction));
        });
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUp.callback.AddListener((_) =>
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        });
        trigger.triggers.Add(pointerUp);

        button.onClick.AddListener(() => upgradeAction());
    }

    private static IEnumerator LongPressCoroutine(System.Action upgradeAction)
    {
        yield return new WaitForSeconds(HOLD_TIME);

        while (true)
        {
            upgradeAction();
            yield return new WaitForSeconds(UPGRADE_INTERVAL);
        }
    }

    private void UpgradeStat(ref int stat, int increment, ref long cost, TextMeshProUGUI statTmp, string statName,
        TextMeshProUGUI costTmp)
    {
        if (PlayerData.Gold < cost)
        {
            SoundManager.Instance.Play("ReinforcementFailure");
            return;
        }

        SoundManager.Instance.Play("ReinforcementSuccess");
        stat += increment;

        PlayerData.Gold -= cost;
        cost = Mathf.CeilToInt(cost * 1.1f);
        statTmp.text = statName + stat;
        costTmp.text = "Upgrade\n" + CurrencyFormatter.FormatBigInteger(cost);
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

        var newValue = Mathf.Min(PlayerData.CriticalPer + 1.0f, 100f);
        UpgradeStat(ref PlayerData.CriticalPer, newValue - PlayerData.CriticalPer, ref _criticalPercentCost,
            CriticalPercentTmp, "치명타확률 : ", CriticalPercentCostTmp);

        if (PlayerData.CriticalPer >= 100f)
        {
            CriticalPercentBtn.interactable = false;
        }
    }

    private void UpgradeStat(ref float stat, float increment, ref long cost, TextMeshProUGUI statTmp, string statName,
        TextMeshProUGUI costTmp)
    {
        if (PlayerData.Gold < cost)
        {
            SoundManager.Instance.Play("ReinforcementFailure");
            return;
        }

        SoundManager.Instance.Play("ReinforcementSuccess");
        stat += increment;

        PlayerData.Gold -= cost;
        cost = Mathf.CeilToInt(cost * 1.1f);
        statTmp.text = statName + stat;
        costTmp.text = CurrencyFormatter.FormatBigInteger(cost);
        UpdateUI();
    }

    public void UpdateUI()
    {
        AttackTmp.text = "공격력\n" + PlayerData.Damage;
        AttackSpeedTmp.text = "공격속도\n" + (PlayerData.AttackSpeed * 100).ToString("F1") + "%";
        MaxHpTmp.text = "최대 체력\n" + PlayerData.MaxHp;
        RecoverHpTmp.text = "체력 회복량\n" + PlayerData.HpRecovery;
        CriticalPercentTmp.text = "치명타 확률\n" + PlayerData.CriticalPer.ToString("F1") + "%";
        CriticalPercentBtn.interactable = PlayerData.CriticalPer < 100f;
        CriticalDamageTmp.text = "치명타 데미지\n" + PlayerData.CriticalMultiplier * 100 + "%";

        AttackCostTmp.text = CurrencyFormatter.FormatBigInteger(_attackCost);
        AttackSpeedCostTmp.text = CurrencyFormatter.FormatBigInteger(_attackSpeedCost);
        HpCostTmp.text = CurrencyFormatter.FormatBigInteger(_maxHpCost);
        RecoverHpCostTmp.text = CurrencyFormatter.FormatBigInteger(_recoverHpCost);
        CriticalPercentCostTmp.text = CurrencyFormatter.FormatBigInteger(_criticalPercentCost);
        CriticalDamageCostTmp.text = CurrencyFormatter.FormatBigInteger(_criticalMultiplierCost);

        UIManager.Instance.UpdateCurrencyUI();
    }
}