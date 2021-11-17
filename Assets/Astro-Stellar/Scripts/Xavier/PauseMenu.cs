using System;
using System.Collections;
using System.Collections.Generic;
using A1.Player;
using Mirror;
using Network_Learning.Scripts.Networking;
using UnityEngine;
using static Network_Learning.Scripts.Networking.CustomNetworkManager;
using NetworkPlayer = Network_Learning.Scripts.Networking.NetworkPlayer;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : NetworkBehaviour
{
    [Header("Lazy Initialization")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI leaveButton;

    #region Properties/Whatever
    private PlayerInteract clientPlayer;
    public static bool isPaused = false;
    #endregion
    
    private void Start()
    {
        pauseMenu.SetActive(false);

        leaveButton.text = hasAuthority ? "End Game" : "Disconnect";
    }

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
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    
    [ClientRpc]
    public void DisconnectClientPlayer()
    {
        RemovePlayer(clientPlayer);
    }

    public void LeaveGame() => SceneManager.LoadScene("MainMenu");
}
