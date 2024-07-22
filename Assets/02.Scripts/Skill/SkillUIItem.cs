using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIItem : MonoBehaviour
{
    public Image iconImage;
    public Text levelText;

    private SkillDataSO skill;
    public event System.Action OnClick;

    public void SetSkill(SkillDataSO newSkill, bool isEquipped)
    {
        skill = newSkill;
        iconImage.sprite = skill.icon;
        levelText.text = $"Lv.{skill.level}";
        // ���� ���ο� ���� �ð��� ǥ�� (��: �׵θ� ���� ����)
    }

    public void OnClickItem()
    {
        OnClick?.Invoke();
    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickItem);
    }
}