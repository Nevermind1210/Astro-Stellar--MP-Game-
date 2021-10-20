using A1.Player;

using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A1
{
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

                    item.gameObject.transform.parent = this.transform;
                    item.gameObject.transform.position = this.transform.position;
                    item.gameObject.SetActive(false);
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