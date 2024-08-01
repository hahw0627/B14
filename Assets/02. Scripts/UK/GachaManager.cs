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

    private void Start()
    {
        onePullButton.onClick.AddListener(() => PullGacha(1));
        twelvePullButton.onClick.AddListener(() => PullGacha(12));
    }

    public void PullGacha(int pullCount)
    {
        gachaPage.SetActive(true);

        for (int i = 0; i < pullCount; i++)
        {
            GameObject scrollInstance = Instantiate(scrollPrefab, gachaPage.transform);
            GachaScroll scroll = scrollInstance.GetComponent<GachaScroll>();
            PetDataSO pulledPet = GetRandomPet();
            scroll.Setup(pulledPet);
        }
    }

    private PetDataSO GetRandomPet()
    {
        // ���⿡ ��޿� ���� Ȯ�� ��� ������ �߰��ϼ���.
        // ���� ���, Rarity�� ���� ���� ���� Ȯ���� ������ �մϴ�.
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
}
