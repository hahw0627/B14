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
        // 장착 여부에 따른 시각적 표시 (예: 테두리 색상 변경)
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