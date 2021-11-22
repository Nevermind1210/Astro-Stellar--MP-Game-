using A1.Player;

using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;
using Network_Learning.Scripts.Networking;
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
        public SyncList<Item> organicItems = new SyncList<Item>();
        public SyncList<Item> partItems = new SyncList<Item>();
        [Header("Items Level Counts")]
        [SerializeField] private int organicsCount;
        [SerializeField] private int partsCount;
        [Header("Items Values for scoring")]
        [SerializeField] private int organicsValue = 10;
        [SerializeField] private int partsValue = 20;
        [Header("Total Group Score")]
        [SyncVar, SerializeField] public int totalScore;

        [Header("UI Elements")]
        public List<GameObject> partsUI = new List<GameObject>();
        private int index;
        [SerializeField] private TMP_Text organicsText;
        [SerializeField] private TMP_Text popupText;
        [SerializeField] private TMP_Text totalScoreText;

        public bool allPartsFound;
        private bool allOrganicsFound;
        [SyncVar] public bool coOpMode = true;

        [Header("Audio SFX")] 
        [SerializeField] private AudioSource organicItem;
        [SerializeField] private AudioSource partItem;


        [Server]
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
                                organicItem.Play();
                                break;
                            case ItemType.ShipPart:
                                partItems.Add(item);
                                totalScore += partsValue;
                                player.personalScore += partsValue;
                                partItem.Play();
                                RpcDisplayPartsUI();
                                break;
                            default:
                                Debug.Log("Item has no type assigned");
                                break;

                    }

                    // Set the tranforms of the item and deactivate
                    RpcPassItemToManager(item);
                    
                    // Set the player item slot to null so they can pick up another item.
                    player.itemHolding = null;
                    
                    CheckCounts();
                }
                else
                {
                    Debug.Log("Not holding an item.");
                }
            }
        }

        
        
        /// <summary>
        /// Passes the item to the manager and sets inactive.
        /// </summary>
        /// <param name="_item"> The Item being passed to the manager.</param>
        [ClientRpc]
        public void RpcPassItemToManager(Item _item)
        {
            _item.gameObject.transform.parent = this.transform;
            _item.gameObject.transform.position = this.transform.position;
            _item.gameObject.SetActive(false);
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
            if(organicItems.Count == organicsCount && !allOrganicsFound)
            {
                Debug.Log("All organics have been found");
                RpcPopupText("All organics have been found.");
                allOrganicsFound = true;

            }
            
            if(partItems.Count == partsCount && !allPartsFound)
            {
                Debug.Log("All parts have been found");
                RpcPopupText("All ship parts have been found.");
                allPartsFound = true;
            }
        }

        /// <summary>
        /// Popup text to display item status to all clients.
        /// </summary>
        /// <param name="_text">Message to display</param>
        [ClientRpc]
        public void RpcPopupText(string _text)
        {
            popupText.text = _text;
            popupText.gameObject.SetActive(true);
            Invoke(nameof(HidePopup),4);
        }

        /// <summary>
        /// Hides the popup.
        /// </summary>
        public void HidePopup() => popupText.gameObject.SetActive(false);

        
        
        public override void OnStartServer()
        {
            //coOpMode = CustomNetworkManager.instance.coopMode;

        }

        [Command]
        public void CmdCoopScore() => RpcCoopScore();

        [ClientRpc]
        public void RpcCoopScore() => totalScoreText.gameObject.SetActive(coOpMode);


        // Update is called once per frame
        void Update()
        {
            RpcCoopScore();
            organicsText.text = organicItems.Count.ToString();
            totalScoreText.text = $"Total Group Score: {totalScore.ToString()}";

        }
    }
}