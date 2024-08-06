using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionInfoPanel : MonoBehaviour
{
    public Image petIcon;
    public TextMeshProUGUI petNameText;
    public TextMeshProUGUI petDescriptionText;
    public TextMeshProUGUI petLevelText;
    public TextMeshProUGUI petCountText;

    public GameObject btnChoice;
    public GameObject choiceFail;

    public Button equipButton;
    public Button currentCompanionButton1;
    public Button currentCompanionButton2;
    public Button currentCompanionButton3;

    public PetDataSO currentPetData;

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

    }

    public void ShowPetInfo(PetDataSO petData)
    {
        currentPetData = petData;
        petIcon.sprite = petData.icon;
        petNameText.text = petData.petName;
        petDescriptionText.text = petData.description;
        petLevelText.text = petData.level.ToString();
        petCountText.text = petData.count.ToString();

        gameObject.SetActive(true);
    }

    public void EquipCompanion()
    {
        if (currentPetData.isEquipped)
        {
            choiceFail.SetActive(true);
            return;
        }
        else if(currentPetData.count == 0 && currentPetData.level == 1)
        {
            choiceFail.SetActive(true);
            return;
        }

        choiceFail.SetActive(false);
        btnChoice.gameObject.SetActive(true);
        selectedButton = null; // ��ư�� ���õ��� �ʾ����� ǥ��
    }

    public void SelectButton(Button button)
    {
        if (selectedButton == null) // ��ư�� ���� ���õ��� �ʾҴٸ�
        {
            selectedButton = button;
            btnChoice.gameObject.SetActive(false); // ��ư ���� �Ϸ� ��, ���� UI ����

            if (currentPetData.isEquipped)
            {
                return;
            }
            else if (currentPetData.count == 0 && currentPetData.level == 1)
            {
                return;
            }
            else
            {
                // ���õ� ��ư�� �̹����� �����մϴ�.
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = currentPetData.icon;
                }
            }

            // ���� ���� ���� ���θ� ������Ʈ�մϴ�.
            UpdateEquippedStatus(button);
        }
    }

    private void UpdateEquippedStatus(Button newButton)
    {
        // CompanionList���� ��� �� �����͸� �����ɴϴ�.
        // DataManager�� �����ϴ°� ���ƺ��� ��� ����
        PetDataSO[] allPetData = companionList.GetAllPetData();
        foreach (PetDataSO petData in allPetData)
        {
            if (petData.isEquipped && petData != currentPetData)
            {
                petData.isEquipped = false; // �̹� ������ �ٸ� ���� ���� ���¸� �����մϴ�.
            }
        }

        currentPetData.isEquipped = true; // ���� ���õ� ���� �����մϴ�.
    }
}
