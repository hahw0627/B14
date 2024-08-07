using UnityEngine;

public class PlayerPrefsUtility : MonoBehaviour
{
    [ContextMenu("DeleteSaveData")]
    private void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
    }
}
