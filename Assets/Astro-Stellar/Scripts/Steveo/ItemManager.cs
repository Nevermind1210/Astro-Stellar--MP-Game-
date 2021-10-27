using A1.Player;

using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

namespace A1
{
    /// <summary>
    /// This class will handle all the items that are brought back to the spaceship object.
    /// </summary>
    public class ItemManager : NetworkBehaviour
    {
        [Header("Items Lists")]
        [SerializeField] private List<Item> organicItems = new List<Item>();
        [SerializeField] private List<Item> partItems = new List<Item>();
        [Header("Items Level Counts")]
        [SerializeField] private int organicsCount;
        [SerializeField] private int partsCount;
        [Header("Items Values for scoring")]
        [SerializeField] private int organicsValue = 10;
        [SerializeField] private int partsValue = 20;
        [Header("Total Group Score")]
        [SerializeField] private int totalScore;

        [Header("UI Elements")]
        public List<GameObject> partsUI = new List<GameObject>();
        private int index;
        // [SerializeField] private TMP_Text organicsText;


        private void OnCollisionEnter(Collision _collision)
        {
            if(_collision.gameObject.CompareTag("Player"))
            {
                // Get the player and check if they are holding an Item.
                PlayerInteract player = _collision.gameObject.GetComponent<PlayerInteract>();
                if(player.itemHolding != null)
                {
                    Item item = player.itemHolding.GetComponent<Item>();
                    // Switch through the different item types and add to respective lists and scores.
                    switch(item.itemType)
                    {
                            case ItemType.Organic:
                                organicItems.Add(item);
                                totalScore += organicsValue;
                                player.personalScore += organicsValue;
                                // RpcDisplayOrganicsCount();
                                break;
                            case ItemType.ShipPart:
                                partItems.Add(item);
                                totalScore += partsValue;
                                player.personalScore += partsValue;
                                // RpcDisplayPartsUI();
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
                    
                    // CheckCounts();
                }
                else
                {
                    Debug.Log("Not holding an item.");
                }
            }
        }

        /// <summary>
        /// Displays the oganics count collected to all clients.
        /// </summary>
        [ClientRpc]
        public void RpcDisplayOrganicsCount()
        {
            // organicsText.text = organicItems.Count.ToString();
        }
        
        /// <summary>
        /// Displays the next parts UI for all clients.
        /// </summary>
        [ClientRpc]
        public void RpcDisplayPartsUI()
        {
            partsUI[index].SetActive(true);
            index += 1;
        }

        /// <summary>
        /// This checks the counts of the lists vs the total items in the level.
        /// </summary>
        private void CheckCounts()
        {
            if(organicItems.Count == organicsCount)
            {
                Debug.Log("All organics have been found");
            }
            
            if(partItems.Count == partsCount)
            {
                Debug.Log("All parts have been found");
                // todo UI display showing all parts have been found.
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