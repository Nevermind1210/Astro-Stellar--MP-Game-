using A1.Player;

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
    public class Item : MonoBehaviour
    {
        [SerializeField] public ItemType itemType;


        private void OnCollisionEnter(Collision _collision)
        {
            if(_collision.gameObject.CompareTag("Player"))
            {
                PlayerInteract player = _collision.gameObject.GetComponent<PlayerInteract>();
                if(player.itemHolding == null)
                {
                    transform.parent = player.itemLocation.transform;
                    transform.position = player.itemLocation.position;
                    player.itemHolding = this;
                }
                else
                {
                    Debug.Log("Already holding an item.");
                }
            }
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