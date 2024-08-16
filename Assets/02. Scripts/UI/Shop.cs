using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button goldToGemButton;
    public Button gemToGoldButton;
    private const int GoldAmount = 10000;
    private const int GemAmount = 100;
    private bool isInitialized = false;

    void OnEnable()
    {
        InitializeButtons();
    }

    void Update()
    {
        UpdateButtonStates();
    }

    void InitializeButtons()
    {
        if (!isInitialized)
        {
            if (goldToGemButton != null)
            {
                goldToGemButton.onClick.AddListener(ConvertGoldToGem);
            }
            else
            {
                Debug.LogError("Gold to Gem button is not assigned!");
            }

            if (gemToGoldButton != null)
            {
                gemToGoldButton.onClick.AddListener(ConvertGemToGold);
            }
            else
            {
                Debug.LogError("Gem to Gold button is not assigned!");
            }

            isInitialized = true;
            Debug.Log("Shop buttons initialized");
        }
    }

    void UpdateButtonStates()
    {
        if (goldToGemButton != null)
        {
            goldToGemButton.interactable = DataManager.Instance.PlayerDataSo.Gold >= GoldAmount;
        }

        if (gemToGoldButton != null)
        {
            gemToGoldButton.interactable = DataManager.Instance.PlayerDataSo.Gem >= GemAmount;
        }
    }

    public void ConvertGoldToGem()
    {
        Debug.Log("ConvertGoldToGem button clicked");
        if (DataManager.Instance.PlayerDataSo.Gold >= GoldAmount)
        {
            DataManager.Instance.PlayerDataSo.Gold -= GoldAmount;
            DataManager.Instance.AddGem(GemAmount);
            SoundManager.Instance.Play("Trade", volume: 3.0f);
            Debug.Log($"Converted {GoldAmount}G to {GemAmount} Gems");
        }
        else
        {
            Debug.Log("Not enough Gold for conversion");
        }
    }

    public void ConvertGemToGold()
    {
        Debug.Log("ConvertGemToGold button clicked");
        if (DataManager.Instance.PlayerDataSo.Gem >= GemAmount)
        {
            DataManager.Instance.PlayerDataSo.Gem -= GemAmount;
            DataManager.Instance.AddGold(GoldAmount);
            SoundManager.Instance.Play("Trade", volume: 3.0f);
            Debug.Log($"Converted {GemAmount} Gems to {GoldAmount}G");
        }
        else
        {
            Debug.Log("Not enough Gems for conversion");
        }
    }
}