using UnityEngine;
using UnityEngine.SceneManagement;

namespace AltarChase.Scripts.Xavier_Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            Debug.Log("I DO NOTHING NOW");
            
            //SceneManager.LoadScene("SomethingHere");
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