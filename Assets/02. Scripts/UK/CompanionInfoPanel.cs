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
        // 시작 시 정보창을 비활성화 상태로 설정
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
        selectedButton = null; // 버튼이 선택되지 않았음을 표시
    }

    public void SelectButton(Button button)
    {
        if (selectedButton == null) // 버튼이 아직 선택되지 않았다면
        {
            selectedButton = button;
            btnChoice.gameObject.SetActive(false); // 버튼 선택 완료 후, 선택 UI 숨김

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
                // 선택된 버튼의 이미지를 변경합니다.
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = currentPetData.icon;
                }
            }

            // 현재 펫의 장착 여부를 업데이트합니다.
            UpdateEquippedStatus(button);
        }
    }

    private void UpdateEquippedStatus(Button newButton)
    {
        // CompanionList에서 모든 펫 데이터를 가져옵니다.
        // DataManager로 변경하는게 좋아보임 잠시 보류
        PetDataSO[] allPetData = companionList.GetAllPetData();
        foreach (PetDataSO petData in allPetData)
        {
            if (petData.isEquipped && petData != currentPetData)
            {
                petData.isEquipped = false; // 이미 장착된 다른 펫의 장착 상태를 해제합니다.
            }
        }

        currentPetData.isEquipped = true; // 현재 선택된 펫을 장착합니다.
    }
}
