using A1.Player;

using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A1
{
    /// <summary>
    /// This class will handle all the items that are brought back to the spaceship object.
    /// </summary>
    public class ItemManager : NetworkBehaviour
    {
        [SerializeField] private List<Item> organicItems = new List<Item>();
        [SerializeField] private List<Item> partItems = new List<Item>();
        

        private void OnCollisionEnter(Collision _collision)
        {
            if(_collision.gameObject.CompareTag("Player"))
            {
                PlayerInteract player = _collision.gameObject.GetComponent<PlayerInteract>();
                if(player.itemHolding != null)
                {
                    Item item = player.itemHolding.GetComponent<Item>();
                    switch(item.itemType)
                    {
                            case ItemType.Organic:
                                organicItems.Add(item);
                                break;
                            case ItemType.ShipPart:
                                partItems.Add(item);
                                break;
                            default:
                                Debug.Log("Item has no type assigned");
                                break;

                    }

                    // Set the tranforms of the item and deactivate
                    item.gameObject.transform.parent = this.transform;
                    item.gameObject.transform.position = this.transform.position;
                    item.gameObject.SetActive(false);
                    // Set the player item slot to null so they can pick up another item.
                    player.itemHolding = null;
                }
                else
                {
                    Debug.Log("Not holding an item.");
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