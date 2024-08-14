using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanionList : MonoBehaviour
{
    public GameObject[] panels;
    public CompanionDataSO[] companionDataArray;
    public GameObject[] companionPrefabs;
    public CompanionInfoPanel companionInfoPanel;
    public Button allUnEquipBtn;

    private void Start()
    {
        allUnEquipBtn.onClick.AddListener(companionInfoPanel.UnEquippedCompanion);
    }

    // �г� Ȱ��ȭ �Ǹ� ���� �ʱ�ȭ
    private void OnEnable()
    {
        // Panel�� Image�ʱ�ȭ
        for (int i = 0; i < panels.Length; i++)
        {
            Image image = panels[i].GetComponent<Image>();
            image.sprite = companionDataArray[i].Icon;
            UpdatePanelColor(image, companionDataArray[i]);

            // �� �гο� Button ������Ʈ�� ������
            Button button = panels[i].GetComponent<Button>();
            if (button != null)
            {
                int index = i;
                // ��ư�� ������ ��, 
                button.onClick.AddListener(() => OnPanelClicked(index));
            }
        }
    }

    public void UpdatePanelColor(Image image, CompanionDataSO companionData)
    {
        // ���ǿ� ���� Color�� Alpha �� ����
        Color color = image.color;
        if (companionData.Level == 1 && companionData.Count == 0)
        {
            color.a = 50 / 255f; // ���� ���� 100���� ���� (0~1 ������ ��ȯ)
        }
        else
        {
            color.a = 1f; // ���� ���� 255�� ���� (1�� ����)
        }
        image.color = color;
    }

    private void OnPanelClicked(int index)
    {
        if (index >= 0 && index < companionDataArray.Length)
        {
            // ����â�� ������Ʈ�ϰ� Ȱ��ȭ
            companionInfoPanel.ShowCompanionInfo(companionDataArray[index]);
        }
    }

    public GameObject[] GetAllCompanionPrefabs()
    {
        return companionPrefabs;
    }
}
