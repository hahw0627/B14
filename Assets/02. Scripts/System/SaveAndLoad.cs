using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    private SaveLoadManager _saveLoadManager;

    private void Awake()
    {
        //_saveLoadManager = gameObject.GetComponent<SaveLoadManager>();
        //_saveLoadManager.LoadSOData();
    }

    private void OnApplicationQuit()
    {
        //_saveLoadManager.SaveSOData();
    }
}