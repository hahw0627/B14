using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
  

    private void Awake()
    {
        SaveLoadManager.Instance.LoadSOData();
    }

    private void OnApplicationQuit()
    {
        SaveLoadManager.Instance.SaveSOData();
    }

}