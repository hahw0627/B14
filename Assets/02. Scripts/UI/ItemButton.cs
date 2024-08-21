using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    Button button;
    ModalsController modalsController;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    void Start()
    {
        modalsController = FindObjectOfType<ModalsController>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        modalsController.CurrentItemImage.sprite = gameObject.GetComponent<Image>().sprite;
        modalsController.CurrentItemName.text = gameObject.name;
    }
}
