using UnityEngine;

namespace Quest.Core
{
    public class PlayerPrefsUtility : MonoBehaviour
    {
        [ContextMenu("DeleteSaveData")]
        private void DeleteSaveData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
