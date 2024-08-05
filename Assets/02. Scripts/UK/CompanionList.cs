using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanionList : MonoBehaviour
{
    public GameObject[] panels;
    public PetDataSO[] petDataArray;
    public CompanionInfoPanel companionInfoPanel;

    // 패널 활성화 되면 정보 초기화
    private void OnEnable()
    {
        // Panel의 Image초기화
        for (int i = 0; i < panels.Length; i++)
        {
            Image image = panels[i].GetComponent<Image>();
            image.sprite = petDataArray[i].icon;
            UpdatePanelColor(image, petDataArray[i]);

            // 각 패널에 Button 컴포넌트가 있으면
            Button button = panels[i].GetComponent<Button>();
            if (button != null)
            {
                int index = i;
                // 버튼을 눌렀을 때, 
                button.onClick.AddListener(() => OnPanelClicked(index));
            }
        }
    }

    public void UpdatePanelColor(Image image, PetDataSO petData)
    {
        // 조건에 따라 Color의 Alpha 값 변경
        Color color = image.color;
        if (petData.level == 1 && petData.count == 0)
        {
            color.a = 50 / 255f; // 알파 값을 100으로 설정 (0~1 범위로 변환)
        }
        else
        {
            color.a = 1f; // 알파 값을 255로 설정 (1로 설정)
        }
        image.color = color;
    }

    private void OnPanelClicked(int index)
    {
        if (index >= 0 && index < petDataArray.Length)
        {
            // 정보창을 업데이트하고 활성화
            companionInfoPanel.ShowPetInfo(petDataArray[index]);
        }
    }
}
