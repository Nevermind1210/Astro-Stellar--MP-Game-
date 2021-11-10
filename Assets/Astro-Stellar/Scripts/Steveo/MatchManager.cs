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
    
        [SyncVar(hook  = nameof(OnRecievedMatchStarted))] public bool matchStarted = false;

        // Any match settings you want here
        [SyncVar] public bool coopMode = true;

        [SyncVar] public bool doubleSpeed = false;
        
        public void StartMatch()
        {
            if(hasAuthority)
            {
                CmdStartMatch();
            }

            //coopMode = true;
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
            // Anything else you want to do in awake
        }
    }
}