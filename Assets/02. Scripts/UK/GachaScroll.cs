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
    }

    private void RevealPet()
    {
        back.SetActive(false);
        front.SetActive(true);
        petIcon.sprite = petData.icon;
    }
}
