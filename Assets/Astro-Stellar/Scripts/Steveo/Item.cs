using A1.Player;

using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A1
{
    /// <summary>
    /// Enum for item identification
    /// </summary>
    public enum ItemType
    {
        Organic,
        ShipPart,
    };
    
    
    /// <summary>
    /// This class handles all the Item functionality
    /// </summary>
    public class Item : NetworkBehaviour
    {
        [SerializeField] public ItemType itemType;


        /// <summary>
        /// This detects collision by a player and adds that item to the PlayerIteract.
        /// </summary>
        /// <param name="_collision"> The collider colliding with this object</param>
        private void OnCollisionEnter(Collision _collision)
        {
            if(_collision.gameObject.CompareTag("Player"))
            {
                PlayerInteract player = _collision.gameObject.GetComponent<PlayerInteract>();
                if(player.itemHolding == null)
                {
                    RpcPickupItem(player);
                    // transform.parent = player.itemLocation.transform;
                    // transform.position = player.itemLocation.position;
                    player.itemHolding = this;
                }
                else
                {
                    Debug.Log("Already holding an item.");
                }
            }
        }

        [ClientRpc]
        public void RpcPickupItem(PlayerInteract _player)
        {
            transform.parent = _player.itemLocation.transform;
            transform.position = _player.itemLocation.position;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}