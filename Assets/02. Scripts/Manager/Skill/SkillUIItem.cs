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
            iconImage.sprite = skill.Icon;
            levelText.text = $"Lv.{skill.Level}";
            iconImage.color = Color.white; // ��ų�� ���� �� �̹����� ���̰� ��
        }
        else
        {
            iconImage.sprite = null;
            levelText.text = "";
            iconImage.color = new Color(1, 1, 1, 0); // ��ų�� ���� �� �̹����� �����ϰ� ��
        }
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

    public void ClearOnClickListeners()
    {
        OnClick = null;
    }
}
