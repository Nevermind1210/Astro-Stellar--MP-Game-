using Astro_Stellar;

using Mirror;

using Network_Learning.Scripts.Networking;

using NetworkGame.Networking;

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace A1.Player
{
    /// <summary>
    /// This class handles all the player interactions with environment.
    /// </summary>
    public class PlayerInteract : NetworkBehaviour
    {
	    private Camera playerCamera;
	    [Header("Camera")]
	    [SerializeField] private Vector3 camOffset;
		[Header("Item Variables")]
	    [SerializeField] public Item itemHolding = null;
	    [SerializeField] public Transform itemLocation;
	    [Header("Player Score")]
	    [SyncVar] public int personalScore;
	    //todo make this a syncvar

	    public TMP_Text personalScoreText;
	    public PlayerScores scores;
	    
        /* 
         *  update HUD
         * WIN
         * LOOSE ??
         */




        public override void OnStartClient()
        {
	        PlayerMotor motor = gameObject.GetComponent<PlayerMotor>();
	        if(isLocalPlayer)
	        {
		        motor.enabled = false;
	        }

	        playerCamera = FindObjectOfType<Camera>();
	        CustomNetworkManager.AddPlayer(this);
	        
	        scores = FindObjectOfType<PlayerScores>();
	        scores.AddPlayer(this);
			//scores.GetActivePlayers();
			
        }

        public void EnableMotor()
        {
	        PlayerMotor motor = gameObject.GetComponent<PlayerMotor>();
	        motor.enabled = isLocalPlayer;
        }
        
        public override void OnStartLocalPlayer()
        {
	        SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
	        
	        CmdAssignAuthority();
        }
        
        [Command]
        public void CmdAssignAuthority()
        {
	        MatchManager.instance.netIdentity.AssignClientAuthority(connectionToClient);
        }

        [Command]
        public void CmdCharacterName(string _name) => RpcCharacterName(_name);

        [ClientRpc]
        public void RpcCharacterName(string _name)
        {
	        CharacterName(_name);
        }

        public void CharacterName(string _name)
        {
	        gameObject.name = _name;
        }
        


        // Start is called before the first frame update
        void Start()
        {
	        // scores = FindObjectOfType<PlayerScores>();
	        // scores.playerList.Clear();
	        // scores.AddPlayer(this);
	        // scores.GetActivePlayers();
        }

        
        // Update is called once per frame
        void Update()
        {
	        if(isLocalPlayer)
	        {
		        playerCamera.transform.position = transform.position + camOffset;
		        playerCamera.transform.LookAt(transform.position);
	        }

	        if(personalScoreText != null)
	        {
		        personalScoreText.text = personalScore.ToString();
	        }

	        if(MatchManager.instance.matchStarted)
	        {
		        EnableMotor();
	        }
        }
    }
}