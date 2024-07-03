using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgrade : MonoBehaviour
{
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

    public int gold = 1000;
    private int attack = 10;
    private int hp = 1000;
    private int recoverHp = 100;
    private float attackSpeed = 0.5f;
    private float criticalPercent = 0.5f;
    private int criticalDamage = 100;

    private int attackCost = 1;
    private int hpCost = 1;
    private int recoverHpCost = 3;
    private int attackSpeedCost = 10;
    private int criticalPercentCost = 100;
    private int criticalDamageCost = 50;

    private void Start()
    {
        UpdateUI();

        attackBtn.onClick.AddListener(() => UpgradeStat(ref attack, 2, ref attackCost, attackTxt, "���ݷ� : ", attackCostTxt));
        hpBtn.onClick.AddListener(() => UpgradeStat(ref hp, 100, ref hpCost, hpTxt, "ü�� : ", hpCostTxt));
        recoverHpBtn.onClick.AddListener(() => UpgradeStat(ref recoverHp, 100, ref recoverHpCost, recoverHpTxt, "ü��ȸ���� : ", recoverHpCostTxt));
        attackSpeedBtn.onClick.AddListener(() => UpgradeStat(ref attackSpeed, 0.5f, ref attackSpeedCost, attackSpeedTxt, "���ݼӵ� : ", attackSpeedCostTxt));
        criticalPercentBtn.onClick.AddListener(() => UpgradeStat(ref criticalPercent, 0.5f, ref criticalPercentCost, criticalPercentTxt, "ġ��ŸȮ�� : ", criticalPercentCostTxt));
        criticalDamageBtn.onClick.AddListener(() => UpgradeStat(ref criticalDamage, 2, ref criticalDamageCost, criticalDamageTxt, "ġ��Ÿ������ : ", criticalDamageCostTxt));
    }
    void UpgradeStat(ref int stat, int increment, ref int cost, Text statTxt, string statName, Text costTxt)
    {
        if (gold >= cost)
        {
            
            stat += increment;

            gold -= cost;
            cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
            statTxt.text = statName +stat.ToString();
            costTxt.text = "Upgrade\n"+ cost.ToString();
            UpdateUI();
        }
    }

    void UpgradeStat(ref float stat, float increment, ref int cost, Text statTxt, string statName, Text costTxt)
    {
        if (gold >= cost)
        {

            stat += increment;

            gold -= cost;
            cost = Mathf.CeilToInt(cost * 1.1f); // ��� 10% ����
            statTxt.text = statName + stat.ToString();
            costTxt.text = "Upgrade \n" + cost.ToString();
            UpdateUI();
        }
    }
    void UpdateUI()
    {
        attackTxt.text = "���ݷ� : \n" + attack;
        hpTxt.text = "ü�� : \n" + hp;
        recoverHpTxt.text = "ü��ȸ���� : \n" + recoverHp;
        attackSpeedTxt.text = "���ݼӵ� : \n" + attackSpeed+"%";
        criticalPercentTxt.text = "ġ��ŸȮ�� : \n" + criticalPercent+"%";
        criticalDamageTxt.text = "ġ��Ÿ������ : \n" + criticalDamage;

        attackCostTxt.text = "Upgrade \n" + attackCost;
        hpCostTxt.text = "Upgrade \n" + hpCost;
        recoverHpCostTxt.text = "Upgrade \n" + recoverHpCost;
        attackSpeedCostTxt.text = "Upgrade \n" + attackSpeedCost;
        criticalPercentCostTxt.text = "Upgrade \n" + criticalPercentCost;
        criticalDamageCostTxt.text = "Upgrade \n" + criticalDamageCost;

    }
}
