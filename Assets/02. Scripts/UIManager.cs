using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public PlayerDataSO playerDataSO;

   void Start()
   {
     
   }

   void Update()
   {
        textMeshProUGUI.text = playerDataSO.Gold.ToString();
   }
}
