using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class CompanionInfoPanel : MonoBehaviour
{
    [Header("UI_CompanionStatus")]
    public Image companionIcon;
    public TextMeshProUGUI companionNameText;
    public TextMeshProUGUI companionDescriptionText;
    public TextMeshProUGUI companionLevelText;
    public TextMeshProUGUI companionCountText;
    public TextMeshProUGUI companionDamageText;

    [Header("UI_Guide")]
    public GameObject btnChoice;
    public GameObject choiceFail;

    [Header("Btn_Equip")]
    public Image nullIcon;
    public Button equipButton;
    public Button currentCompanionButton1;
    public Button currentCompanionButton2;
    public Button currentCompanionButton3;

    [Header("")]
    public Button upgradeButton;

    [Header("SO_CompanionData")]
    public CompanionDataSO currentCompanionData;
    public CompanionDataSO formerCompanionData1;
    public CompanionDataSO formerCompanionData2;
    public CompanionDataSO formerCompanionData3;

    [Header("Position")]
    public GameObject pos1;
    public GameObject pos2;
    public GameObject pos3;

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
        else if (currentCompanionData.count == 0 && currentCompanionData.level == 1)
        {
            choiceFail.SetActive(true);
            return;
        }

        choiceFail.SetActive(false);
        btnChoice.gameObject.SetActive(true);
        selectedButton = null; // 버튼이 선택되지 않았음을 표시
    }

    public void SelectButton(Button button)
    {   // 버튼골라서 이미지 입히기
        if (selectedButton == null) // 버튼이 아직 선택되지 않았다면
        {
            selectedButton = button;
            btnChoice.gameObject.SetActive(false); // 버튼 선택 완료 후, 버튼 선택 UI 숨김

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
                // 선택된 버튼의 이미지를 변경합니다.
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = currentCompanionData.icon;
                }
            }

            // 현재 펫의 장착 여부를 업데이트합니다.
            UpdateEquippedStatus(button);
        }
    }

    private void UpdateEquippedStatus(Button newButton)
    {
        if (newButton == currentCompanionButton1)
        {
            EquippedCheck(ref formerCompanionData1);
            SpawnCompanion(pos1);
        }
        else if (newButton == currentCompanionButton2)
        {
            EquippedCheck(ref formerCompanionData2);
            SpawnCompanion(pos2);
        }
        else if (newButton == currentCompanionButton3)
        {
            EquippedCheck(ref formerCompanionData3);
            SpawnCompanion(pos3);
        }
    }

    private void EquippedCheck(ref CompanionDataSO formerCompanionData)
    {
        if (formerCompanionData == currentCompanionData)
        {
            return;
        }

        if (formerCompanionData == null)
        {
            currentCompanionData.isEquipped = true;
            formerCompanionData = currentCompanionData;
        }
        else
        {
            formerCompanionData.isEquipped = false;
            currentCompanionData.isEquipped = true;
            formerCompanionData = currentCompanionData;
        }
    }

    public void CompanionUpgrade()
    {
        if (currentCompanionData.count > 4)
        {
            currentCompanionData.level += 1;
            currentCompanionData.count -= 5;
            currentCompanionData.damage += 5;
            companionLevelText.text = currentCompanionData.level.ToString();
            companionCountText.text = currentCompanionData.count.ToString();
            companionDamageText.text = currentCompanionData.damage.ToString();
        }
    }

    private void SpawnCompanion(GameObject pos)
    {
        GameObject[] allCompanionPrefabs = companionList.GetAllCompanionPrefabs();
        foreach (GameObject companionPrefab in allCompanionPrefabs)
        {
            if (companionPrefab.GetComponent<Pet>().companionData == currentCompanionData)
            {
                Instantiate(companionPrefab, pos.transform.position, Quaternion.identity);
                break;
            }
        }
        DestroyCompanion();
    }

    private void DestroyCompanion()
    {
        // 현재 씬의 Pet컴포넌트를 검색하여 배열에 저장
        Pet[] allPets = FindObjectsOfType<Pet>();

        // 해당 Pet컴포넌트의 SO에 접근하여 장착여부가 false이면 오브젝트 제거
        foreach (Pet pet in allPets)
        {
            if (!pet.companionData.isEquipped)
            {
                Destroy(pet.gameObject);
            }
        }
    }

    public void UnEquippedCompanion()
    {
        CompanionDataSO[] allCompanionData = companionList.companionDataArray;
        foreach(CompanionDataSO companionDataSO in allCompanionData)
        {
            companionDataSO.isEquipped = false;
        }
        currentCompanionButton1.image = nullIcon;
        currentCompanionButton2.image = nullIcon;
        currentCompanionButton3.image = nullIcon;
        formerCompanionData1 = null;
        formerCompanionData2 = null;
        formerCompanionData3 = null;
        selectedButton = null;
        currentCompanionData = null;
        DestroyCompanion();
    }
}
