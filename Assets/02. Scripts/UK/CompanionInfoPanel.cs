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

    private void Start()
    {
        // 시작 시 정보창을 비활성화 상태로 설정
        gameObject.SetActive(false);
    }

    public void ShowPetInfo(PetDataSO petData)
    {
        petIcon.sprite = petData.icon;
        petNameText.text = petData.petName;
        petDescriptionText.text = petData.description;
        petLevelText.text = petData.level.ToString();
        petCountText.text = petData.count.ToString();

        gameObject.SetActive(true);
    }

    // 강화 기능 필요


    // 장착 기능 필요
}
