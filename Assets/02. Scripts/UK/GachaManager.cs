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
        // 유니티 버튼 컴포넌트에서 연결하지 않고 코드로 연결해보기
        closeButton.onClick.AddListener(CloseGacha);
        allOpenButton.onClick.AddListener(AllScrollOpen);
    }

    public void PullGacha(int pullCount)
    {
        // 가챠페이지 활성화
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
        // 여기에 등급에 따른 확률 계산 로직
        // Rarity가 높은 펫이 낮은 확률로 나오게
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

        return null; // 안전장치, 이 코드에 도달하면 안 됨.
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
        // 모든 Scroll 오브젝트 제거
        foreach (Transform child in gachaPage.transform)
        {
            Destroy(child.gameObject);
        }
        // 리스트 초기화
        activeScrolls.Clear();

        // GachaPage 비활성화
        gachaPage.SetActive(false);
        closeButton.SetActive(false);
        allOpenButton.SetActive(false);
    }

    private void AllScrollOpen()
    {
        // 활성화된 모든 Scroll의 Front를 활성화
        foreach (var scroll in activeScrolls)
        {
            scroll.RevealPet();
        }
    }
}
