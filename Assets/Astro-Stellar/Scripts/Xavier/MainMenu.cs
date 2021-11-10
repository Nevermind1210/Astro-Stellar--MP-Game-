using UnityEngine;
using UnityEngine.SceneManagement;

namespace AltarChase.Scripts.Xavier_Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Offline A1S");
        }
        
        public void QuitGame()
        {
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        
    }
}