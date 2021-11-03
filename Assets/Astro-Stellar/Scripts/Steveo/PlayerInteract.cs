using Mirror;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	    
        /* 
         *  update HUD
         * WIN
         * LOOSE ??
         */




        public override void OnStartClient()
        {
	        PlayerMotor motor = gameObject.GetComponent<PlayerMotor>();
	        motor.enabled = isLocalPlayer;

	        playerCamera = FindObjectOfType<Camera>();
	        
        }

        
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        
        // Update is called once per frame
        void Update()
        {
	        if(isLocalPlayer)
	        {
		        playerCamera.transform.position = transform.position + camOffset;
		        playerCamera.transform.LookAt(transform.position);
	        }
        }
    }
}