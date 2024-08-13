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
    public Sprite nullIcon;
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
        companionIcon.sprite = companionData.Icon;
        companionNameText.text = companionData.CompanionName;
        companionDescriptionText.text = companionData.Description;
        companionLevelText.text = companionData.Level.ToString();
        companionCountText.text = companionData.Count.ToString();
        companionDamageText.text = companionData.Damage.ToString();

        gameObject.SetActive(true);
    }

    public void EquipCompanion()
    {
        if (currentCompanionData.IsEquipped)
        {
            choiceFail.SetActive(true);
            return;
        }
        else if (currentCompanionData.Count == 0 && currentCompanionData.Level == 1)
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
            btnChoice.gameObject.SetActive(false); // ��ư ���� �Ϸ� ��, ��ư ���� UI ����

            if (currentCompanionData.IsEquipped)
            {
                return;
            }
            else if (currentCompanionData.Count == 0 && currentCompanionData.Level == 1)
            {
                return;
            }
            else
            {
                // ���õ� ��ư�� �̹����� �����մϴ�.
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = currentCompanionData.Icon;
                }
            }

            // ���� ���� ���� ���θ� ������Ʈ�մϴ�.
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
            currentCompanionData.IsEquipped = true;
            formerCompanionData = currentCompanionData;
        }
        else
        {
            formerCompanionData.IsEquipped = false;
            currentCompanionData.IsEquipped = true;
            formerCompanionData = currentCompanionData;
        }
    }

    public void CompanionUpgrade()
    {
        if (currentCompanionData.Count > 4)
        {
            currentCompanionData.Level += 1;
            currentCompanionData.Count -= 5;
            currentCompanionData.Damage += 5;
            companionLevelText.text = currentCompanionData.Level.ToString();
            companionCountText.text = currentCompanionData.Count.ToString();
            companionDamageText.text = currentCompanionData.Damage.ToString();
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
        // ���� ���� Pet������Ʈ�� �˻��Ͽ� �迭�� ����
        Pet[] allPets = FindObjectsOfType<Pet>();

        // �ش� Pet������Ʈ�� SO�� �����Ͽ� �������ΰ� false�̸� ������Ʈ ����
        foreach (Pet pet in allPets)
        {
            if (!pet.companionData.IsEquipped)
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
            companionDataSO.IsEquipped = false;
        }
        currentCompanionButton1.image.sprite = nullIcon;
        currentCompanionButton2.image.sprite = nullIcon;
        currentCompanionButton3.image.sprite = nullIcon;
        formerCompanionData1 = null;
        formerCompanionData2 = null;
        formerCompanionData3 = null;
        selectedButton = null;
        currentCompanionData = null;
        DestroyCompanion();
    }
}
