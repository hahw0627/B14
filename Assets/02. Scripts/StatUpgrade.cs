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
    void UpgradeStat(ref int stat, int increment, ref int cost, Text statTxt, string statName, Text costTxt)
    {
        if (playerData.Gold >= cost)
        {
            
            stat += increment;

            playerData.Gold -= cost;
            cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
            statTxt.text = statName +stat.ToString();
            costTxt.text = "Upgrade\n"+ cost.ToString();
            UpdateUI();
        }
    }

    void UpgradeStat(ref float stat, float increment, ref int cost, Text statTxt, string statName, Text costTxt)
    {
        if (playerData.Gold >= cost)
        {

            stat += increment;

            playerData.Gold -= cost;
            cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
            statTxt.text = statName + stat.ToString();
            costTxt.text = "Upgrade \n" + cost.ToString();
            UpdateUI();
        }
    }
    void UpdateUI()
    {
        attackTxt.text = "���ݷ� : \n" + playerData.Damage;
        hpTxt.text = "ü�� : \n" + playerData.Hp;
        recoverHpTxt.text = "ü��ȸ���� : \n" + playerData.HpRecovery;
        attackSpeedTxt.text = "���ݼӵ� : \n" + playerData.AttackSpeed+"%";
        criticalPercentTxt.text = "ġ��ŸȮ�� : \n" + playerData.CriticalPer+"%";
        criticalDamageTxt.text = "ġ��Ÿ������ : \n" + playerData.CriticalDamage;

        attackCostTxt.text = "Upgrade \n" + attackCost;
        hpCostTxt.text = "Upgrade \n" + hpCost;
        recoverHpCostTxt.text = "Upgrade \n" + recoverHpCost;
        attackSpeedCostTxt.text = "Upgrade \n" + attackSpeedCost;
        criticalPercentCostTxt.text = "Upgrade \n" + criticalPercentCost;
        criticalDamageCostTxt.text = "Upgrade \n" + criticalDamageCost;

    }
}
