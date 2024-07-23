using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02._Scripts.Management
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}