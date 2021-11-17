using A1;

using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetworkGame.Networking
{
    public class MatchManager : NetworkBehaviour
    {
        public static MatchManager instance = null;

        public ItemManager itemManager;
        public CountdownToLose countdownToLose;
    
        [SyncVar(hook  = nameof(OnRecievedMatchStarted))] public bool matchStarted = false;

        // Any match settings you want here
        [SyncVar] public bool coopMode = true;

        
        public void StartMatch()
        {
            if(hasAuthority)
            {
                CmdStartMatch();
            }

            itemManager.coOpMode = coopMode;
        }

        [Command(requiresAuthority = false)]
        public void CmdStartMatch()
        {
            matchStarted = true;
        }
        
        private void OnRecievedMatchStarted(bool _old, bool _new)
        {
            if(_new)
            {
                SceneManager.UnloadSceneAsync("Lobby");
            }
        }
        
        [Server]
        public void EndGame()
        {
            //if playing coop
                // if all parts are found and timer hits zero
                    //displays total score and says win message - item manager has total score.
                // if parts are not found and timer hits zero
                    // displays lose message
                    
            // if playing not playing coop
                // if all parts are found and timer hits zero
                    // display high score and winner.
                    //  loop through list in player scores checking personal score to find highest.
                // if all parts are not found and timer hits zero
                    // no one wins and display message saying ship could not take off.
                    
                    
            // after all this wait for 5-10 s and load main menu??
                
        }
        
        

        protected void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
                return;
            }

            itemManager = FindObjectOfType<ItemManager>();
            countdownToLose = FindObjectOfType<CountdownToLose>();
            // Anything else you want to do in awake
        }
    }
}