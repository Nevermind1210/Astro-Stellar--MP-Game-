using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class CountdownToLose : NetworkBehaviour
{ 
   [SerializeField,SyncVar] private float timeRemaining = 10;
   [SyncVar] private float minutes;
   [SyncVar] private float seconds;
   public TextMeshProUGUI timer;
  
   private static bool timerRunning = false;
   
   private void Start()
   {
      timerRunning = true;
   }
   
   private void Update()
   {
      TimeOnEveryone();
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
         timeRemaining = 0;
         timerRunning = false;
      }
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
