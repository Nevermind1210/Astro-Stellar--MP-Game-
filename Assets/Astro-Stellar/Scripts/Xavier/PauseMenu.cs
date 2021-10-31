using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public static bool isPaused = false;
    
    // Update is called once per frame
    void Update()
    {
        // Theres a problem with doing it like this I know there are some solution to this, but the only way to close is by clicking resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            if (!isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }
}
