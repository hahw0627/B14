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
        if (skill != null)
        {
            iconImage.sprite = skill.icon;
            levelText.text = $"Lv.{skill.level}";
            iconImage.color = Color.white; // 스킬이 있을 때 이미지를 보이게 함
        }
        else
        {
            iconImage.sprite = null;
            levelText.text = "";
            iconImage.color = new Color(1, 1, 1, 0); // 스킬이 없을 때 이미지를 투명하게 함
        }
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

    public void ClearOnClickListeners()
    {
        OnClick = null;
    }
}
