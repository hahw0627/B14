using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaScroll : MonoBehaviour
{
    public GameObject back;
    public GameObject front;
    public Button revealButton;
    public Image petIcon;

    public GameObject normal;
    public GameObject rare;
    public GameObject unique;
    public GameObject epic;
    public GameObject legendary;

    private PetDataSO petData;

    private void Start()
    {
        revealButton.onClick.AddListener(RevealPet);
    }

    public void Setup(PetDataSO pet)
    {
        petData = pet;
        front.SetActive(false);
        back.SetActive(true);

        normal.SetActive(false);
        rare.SetActive(false);
        unique.SetActive(false);
        epic.SetActive(false);
        legendary.SetActive(false);
    }

    public void RevealPet()
    {
        back.SetActive(false);
        front.SetActive(true);
        petIcon.sprite = petData.icon;

        switch (petData.rarity)
        {
            case Define.SkillRarity.Normal:
                normal.SetActive(true);
                break;
            case Define.SkillRarity.Rare:
                rare.SetActive(true);
                break;
            case Define.SkillRarity.Unique:
                unique.SetActive(true);
                break;
            case Define.SkillRarity.Epic:
                epic.SetActive(true);
                break;
            case Define.SkillRarity.Legendary:
                legendary.SetActive(true);
                break;
        }
    }
}
