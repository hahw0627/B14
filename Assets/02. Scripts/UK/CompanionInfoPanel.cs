using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionInfoPanel : MonoBehaviour
{
    [Header("")]
    public Image companionIcon;
    public TextMeshProUGUI companionNameText;
    public TextMeshProUGUI companionDescriptionText;
    public TextMeshProUGUI companionLevelText;
    public TextMeshProUGUI companionCountText;
    public TextMeshProUGUI companionDamageText;

    [Header("")]
    public GameObject btnChoice;
    public GameObject choiceFail;

    [Header("")]
    public Button equipButton;
    public Button currentCompanionButton1;
    public Button currentCompanionButton2;
    public Button currentCompanionButton3;

    [Header("")]
    public Button upgradeButton;

    [Header("")]
    public CompanionDataSO currentCompanionData;

    private Button selectedButton;

    public CompanionList companionList;


    private void Start()
    {
        // ���� �� ����â�� ��Ȱ��ȭ ���·� ����
        gameObject.SetActive(false);

        equipButton.onClick.AddListener(EquipCompanion);
        currentCompanionButton1.onClick.AddListener(() => SelectButton(currentCompanionButton1));
        currentCompanionButton2.onClick.AddListener(() => SelectButton(currentCompanionButton2));
        currentCompanionButton3.onClick.AddListener(() => SelectButton(currentCompanionButton3));

        upgradeButton.onClick.AddListener(CompanionUpgrade);

    }

    public void ShowCompanionInfo(CompanionDataSO companionData)
    {
        currentCompanionData = companionData;
        companionIcon.sprite = companionData.icon;
        companionNameText.text = companionData.companionName;
        companionDescriptionText.text = companionData.description;
        companionLevelText.text = companionData.level.ToString();
        companionCountText.text = companionData.count.ToString();
        companionDamageText.text = companionData.damage.ToString();

        gameObject.SetActive(true);
    }

    public void EquipCompanion()
    {
        if (currentCompanionData.isEquipped)
        {
            choiceFail.SetActive(true);
            return;
        }
        else if(currentCompanionData.count == 0 && currentCompanionData.level == 1)
        {
            choiceFail.SetActive(true);
            return;
        }

        choiceFail.SetActive(false);
        btnChoice.gameObject.SetActive(true);
        selectedButton = null; // ��ư�� ���õ��� �ʾ����� ǥ��
    }

    public void SelectButton(Button button)
    {   // ��ư��� �̹��� ������
        if (selectedButton == null) // ��ư�� ���� ���õ��� �ʾҴٸ�
        {
            selectedButton = button;
            btnChoice.gameObject.SetActive(false); // ��ư ���� �Ϸ� ��, ���� UI ����

            if (currentCompanionData.isEquipped)
            {
                return;
            }
            else if (currentCompanionData.count == 0 && currentCompanionData.level == 1)
            {
                return;
            }
            else
            {
                // ���õ� ��ư�� �̹����� �����մϴ�.
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = currentCompanionData.icon;
                }
            }

            // ���� ���� ���� ���θ� ������Ʈ�մϴ�.
            UpdateEquippedStatus(button);
        }
    }

    private void UpdateEquippedStatus(Button newButton)
    {   // �� ���� ���� ������Ʈ
        // CompanionList���� ��� �� �����͸� ������
        // DataManager�� �����ϴ°� ���ƺ��� ��� ����
        CompanionDataSO[] allCompanionData = companionList.GetAllCompanionData();
        foreach (CompanionDataSO companionData in allCompanionData)
        {
            if (companionData.isEquipped && companionData != currentCompanionData)
            {
                companionData.isEquipped = false; // �̹� ������ �ٸ� ���� ���� ���¸� ����
            }
        }

        currentCompanionData.isEquipped = true; // ���� ���õ� ���� ����
    }

    public void CompanionUpgrade()
    {
        if(currentCompanionData.count > 4)
        {
            currentCompanionData.level += 1;
            currentCompanionData.count -= 5;
            currentCompanionData.damage += 5;
            companionLevelText.text = currentCompanionData.level.ToString();
            companionCountText.text = currentCompanionData.count.ToString();
            companionDamageText.text = currentCompanionData.damage.ToString();
        }
    }
}
