using System;
using System.Collections;
using System.Collections.Generic;
using A1;
using Mirror;

using NetworkGame.Networking;

using TMPro;
using UnityEngine;

public class CountdownToLose : NetworkBehaviour
{ 
   [Header("Timer Set")]
   [SerializeField,SyncVar] private float timeRemaining = 30;

   private ItemManager _itemManager;
   [SyncVar] private float minutes;
   [SyncVar] private float seconds;
   public TextMeshProUGUI timer;
  
   [SyncVar]public bool timerRunning = false;
   
   
   
   private void Start()
   {
      _itemManager = FindObjectOfType<ItemManager>();
      timerRunning = false;
   }
   
   private void Update()
   {
      if(timerRunning)
      {
         TimeOnEveryone();
      }

      if(MatchManager.instance.matchStarted && MatchManager.instance.isPlaying)
      {
         timerRunning = true;
      }

   }
   
   
   void TimeOnEveryone()
   {
      if (timeRemaining > 0)
      {
         timeRemaining -= Time.deltaTime;
         ServerDisplayTime(timeRemaining);
      }
      else
      {
         Debug.LogError("Time has ran out!");
         if (timerRunning)
         {
            MatchManager.instance.isPlaying = false;
            MatchManager.instance.RunEndGame();
            //_itemManager.RpcPopupText("Time has ran out!");
            timeRemaining = 0;
            timerRunning = false;
         }
         timer.text = "0:00";
      }

      // if(timeRemaining == 15)
      // {
      //    _itemManager.RpcPopupText("15 seconds remaining.");
      // }
   }
   
   void ServerDisplayTime(float timeToDisplay)
   {
      minutes = Mathf.FloorToInt(timeToDisplay / 60); 
      seconds = Mathf.FloorToInt(timeToDisplay % 60);

      timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
   }

   void ClientDisplayTime(float _timeToDisplay)
   {
      float minutes = Mathf.FloorToInt(_timeToDisplay / 60);
      float seconds = Mathf.FloorToInt(_timeToDisplay % 60);

      timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
   }
}
