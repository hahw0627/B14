using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public GameObject gachaPage;
    public GameObject scrollPrefab;
    public List<PetDataSO> petDataList;
    public Button onePullButton;
    public Button twelvePullButton;
    public Button closeButton;
    public Button allOpenButton;

    private List<GachaScroll> activeScrolls = new List<GachaScroll>();

    private void Start()
    {
        // ����Ƽ ��ư ������Ʈ���� �������� �ʰ� �ڵ�� �����غ���
        closeButton.onClick.AddListener(CloseGacha);
        allOpenButton.onClick.AddListener(AllScrollOpen);
    }

    public void PullGacha(int pullCount)
    {
        // ��í������ Ȱ��ȭ
        gachaPage.SetActive(true);
        closeButton.SetActive(true);
        allOpenButton.SetActive(true);

        for (int i = 0; i < pullCount; i++)
        {
            GameObject scrollInstance = Instantiate(scrollPrefab, gachaPage.transform);
            GachaScroll scroll = scrollInstance.GetComponent<GachaScroll>();
            PetDataSO pulledPet = GetRandomPet();
            scroll.Setup(pulledPet);
            activeScrolls.Add(scroll);
        }
    }

    private PetDataSO GetRandomPet()
    {
        // ���⿡ ��޿� ���� Ȯ�� ��� ����
        // Rarity�� ���� ���� ���� Ȯ���� ������
        int totalWeight = 0;
        foreach (var pet in petDataList)
        {
            totalWeight += GetWeightByRarity(pet.rarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (var pet in petDataList)
        {
            accumulatedWeight += GetWeightByRarity(pet.rarity);
            if (randomValue < accumulatedWeight)
            {
                pet.count++;
                return pet;
            }
        }

        return null; // ������ġ, �� �ڵ忡 �����ϸ� �� ��.
    }

    private int GetWeightByRarity(Define.SkillRarity rarity)
    {
        switch (rarity)
        {
            case Define.SkillRarity.Normal:
                return 70;
            case Define.SkillRarity.Rare:
                return 15;
            case Define.SkillRarity.Unique:
                return 9;
            case Define.SkillRarity.Epic:
                return 5;            
            case Define.SkillRarity.Legendary:
                return 1;
            default:
                return 0;
        }
    }

    private void CloseGacha()
    {
        // ��� Scroll ������Ʈ ����
        foreach (Transform child in gachaPage.transform)
        {
            Destroy(child.gameObject);
        }
        // ����Ʈ �ʱ�ȭ
        activeScrolls.Clear();

        // GachaPage ��Ȱ��ȭ
        gachaPage.SetActive(false);
        closeButton.SetActive(false);
        allOpenButton.SetActive(false);
    }

    private void AllScrollOpen()
    {
        // Ȱ��ȭ�� ��� Scroll�� Front�� Ȱ��ȭ
        foreach (var scroll in activeScrolls)
        {
            scroll.RevealPet();
        }
    }
}
