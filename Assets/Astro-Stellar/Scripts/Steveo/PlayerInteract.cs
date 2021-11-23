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

using Random = UnityEngine.Random;

namespace A1.Player
{
    /// <summary>
    /// This class handles all the player interactions with environment.
    /// </summary>
    public class PlayerInteract : NetworkBehaviour
    {
	    private Camera playerCamera;
	    public PlayerMotor motor;
	    [Header("Camera")]
	    [SerializeField] private Vector3 camOffset;
		[Header("Item Variables")]
	    [SerializeField] public Item itemHolding = null;
	    [SerializeField] public Transform itemLocation;
	    [Header("Player Score")]
	    [SyncVar] public int personalScore;
	   

	    public TMP_Text personalScoreText;
	    public PlayerScores scores;

	    [Header("Player Materials")] 
	    public List<Material> playerMaterials = new List<Material>();
	    [SyncVar(hook = nameof(OnSetPlayerMaterial)), SerializeField] private int playerMaterial;
	    private Material cachedMaterial;
	    [SerializeField] private SkinnedMeshRenderer playerRend;

	    /// <summary>
	    /// Hook for setting the player material on the server.
	    /// </summary>
	    private void OnSetPlayerMaterial(int _old, int _new)
	    {
		    if(cachedMaterial == null)
		    {
			    cachedMaterial = playerMaterials[playerMaterial];
		    }

		    playerRend.material = cachedMaterial;
	    }
	    
	    
        public override void OnStartClient()
        {
	        // This disables the player motor until the match has started.
	        motor = gameObject.GetComponent<PlayerMotor>();
	        if(isLocalPlayer)
	        {
		        motor.enabled = false;
				CmdGetColor();
	        }

	        playerCamera = FindObjectOfType<Camera>();
	        CustomNetworkManager.AddPlayer(this);
	        
	        scores = FindObjectOfType<PlayerScores>();
	        scores.AddPlayer(this);
	        
        }

        /// <summary>
        /// Enables the player motor if the match has started. Called in Update.
        /// </summary>
        public void EnableMotor()
        {
	        motor = gameObject.GetComponent<PlayerMotor>();
	        motor.enabled = isLocalPlayer;
        }

        /// <summary>
        /// Command for setting the player material index.
        /// </summary>
        [Command]
        public void CmdGetColor()
        {
	        playerMaterial = Random.Range(0, playerMaterials.Count);
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

        // CALLS FOR SETTING THE PLAYER NAME
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