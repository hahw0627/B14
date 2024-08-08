using UnityEngine;
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

    private void Start()
    {
        UpdateUI();

        AttackBtn.onClick.AddListener(() => UpgradeStat(ref PlayerData.Damage, 2, ref _attackCost, AttackTmp, "���ݷ� : ", AttackCostTmp));
        HpBtn.onClick.AddListener(() => UpgradeStat(ref PlayerData.Hp, 100, ref _hpCost, HpTmp, "ü�� : ", HpCostTmp));
        RecoverHpBtn.onClick.AddListener(() => UpgradeStat(ref PlayerData.HpRecovery, 100, ref _recoverHpCost, RecoverHpTmp, "ü��ȸ���� : ", RecoverHpCostTmp));
        AttackSpeedBtn.onClick.AddListener(() => UpgradeStat(ref PlayerData.AttackSpeed, 0.5f, ref _attackSpeedCost, AttackSpeedTmp, "���ݼӵ� : ", AttackSpeedCostTmp));
        CriticalPercentBtn.onClick.AddListener(() => UpgradeStat(ref PlayerData.CriticalPer, 0.2f, ref _criticalPercentCost, CriticalPercentTmp, "ġ��ŸȮ�� : ", CriticalPercentCostTmp));
        CriticalDamageBtn.onClick.AddListener(() => UpgradeStat(ref PlayerData.CriticalMultiplier, 0.003f, ref _criticalDamageCost, CriticalDamageTmp, "ġ��Ÿ������ : ", CriticalDamageCostTmp));
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
        CriticalPercentTmp.text = "치명타 확률" + PlayerData.CriticalPer+"%";
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
