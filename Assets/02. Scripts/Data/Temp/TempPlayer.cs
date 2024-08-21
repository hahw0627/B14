using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    public GameData gameData;
    SaveLoadManager saveLoadManager;
    void Start()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //if(gameData != null)

            //gameData = saveLoadManager.LoadGame();
            
        }
    }
    
}
