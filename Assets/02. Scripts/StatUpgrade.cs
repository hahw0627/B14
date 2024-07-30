using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgrade : MonoBehaviour
{
    public PlayerDataSO playerData;

    public Text attackTxt;
    public Text hpTxt;
    public Text recoverHpTxt;
    public Text attackSpeedTxt;
    public Text criticalPercentTxt;
    public Text criticalDamageTxt;

    public Text attackCostTxt;
    public Text hpCostTxt;
    public Text recoverHpCostTxt;
    public Text attackSpeedCostTxt;
    public Text criticalPercentCostTxt;
    public Text criticalDamageCostTxt;

    public Button attackBtn;
    public Button hpBtn;
    public Button recoverHpBtn;
    public Button attackSpeedBtn;
    public Button criticalPercentBtn;
    public Button criticalDamageBtn;

    private int attackCost = 1;
    private int hpCost = 1;
    private int recoverHpCost = 3;
    private int attackSpeedCost = 10;
    private int criticalPercentCost = 100;
    private int criticalDamageCost = 50;

    public static event System.Action OnStatsChanged;

    private void Start()
    {
        UpdateUI();

        attackBtn.onClick.AddListener(() => UpgradeStat(ref playerData.Damage, 2, ref attackCost, attackTxt, "���ݷ� : ", attackCostTxt));
        hpBtn.onClick.AddListener(() => UpgradeStat(ref playerData.Hp, 100, ref hpCost, hpTxt, "ü�� : ", hpCostTxt));
        recoverHpBtn.onClick.AddListener(() => UpgradeStat(ref playerData.HpRecovery, 100, ref recoverHpCost, recoverHpTxt, "ü��ȸ���� : ", recoverHpCostTxt));
        attackSpeedBtn.onClick.AddListener(() => UpgradeStat(ref playerData.AttackSpeed, 0.5f, ref attackSpeedCost, attackSpeedTxt, "���ݼӵ� : ", attackSpeedCostTxt));
        criticalPercentBtn.onClick.AddListener(() => UpgradeStat(ref playerData.CriticalPer, 0.2f, ref criticalPercentCost, criticalPercentTxt, "ġ��ŸȮ�� : ", criticalPercentCostTxt));
        criticalDamageBtn.onClick.AddListener(() => UpgradeStat(ref playerData.CriticalDamage, 0.3f, ref criticalDamageCost, criticalDamageTxt, "ġ��Ÿ������ : ", criticalDamageCostTxt));
    }

    private void UpgradeStat(ref int stat, int increment, ref int cost, Text statTxt, string statName, Text costTxt)
    {
        if (playerData.Gold >= cost)
        {
            
            stat += increment;

            playerData.Gold -= cost;
            cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
            statTxt.text = statName +stat;
            costTxt.text = "Upgrade\n"+ cost;
            UpdateUI();
            OnStatsChanged?.Invoke();
        }
    }

    private void UpgradeStat(ref float stat, float increment, ref int cost, Text statTxt, string statName, Text costTxt)
    {
        if (playerData.Gold >= cost)
        {

            stat += increment;

            playerData.Gold -= cost;
            cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
            statTxt.text = statName + stat;
            costTxt.text = cost.ToString();
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        attackTxt.text = "공격력\n" + playerData.Damage;
        attackSpeedTxt.text = "공격속도\n" + playerData.AttackSpeed+"%";
        hpTxt.text = "체력\n" + playerData.Hp;
        recoverHpTxt.text = "체력 회복량\n" + playerData.HpRecovery;
        criticalPercentTxt.text = "치명타 확률" + playerData.CriticalPer+"%";
        criticalDamageTxt.text = "치명타 데미지\n" + playerData.CriticalDamage;

        attackCostTxt.text = attackCost.ToString();
        attackSpeedCostTxt.text = attackSpeedCost.ToString();
        hpCostTxt.text = hpCost.ToString();
        recoverHpCostTxt.text = recoverHpCost.ToString();
        criticalPercentCostTxt.text = criticalPercentCost.ToString();
        criticalDamageCostTxt.text = criticalDamageCost.ToString();
    
        UIManager.Instance.UpdateCurrencyUI();    
    }
}
