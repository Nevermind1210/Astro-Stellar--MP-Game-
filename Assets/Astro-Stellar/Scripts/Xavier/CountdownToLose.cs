using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class CountdownToLose : NetworkBehaviour
{
   public float timeRemaining = 10;
   public TextMeshProUGUI timer;

   private static bool timerRunning = false;
   
   private void Start()
   {
      timerRunning = true;
   }
   
   private void Update()
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

   [Server]
   void ServerDisplayTime(float timeToDisplay)
   {
      float minutes = Mathf.FloorToInt(timeToDisplay / 60);
      float seconds = Mathf.FloorToInt(timeToDisplay % 60);

      timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
   }

   void ClientDisplayTime(float _timeToDisplay)
   {
      float minutes = Mathf.FloorToInt(_timeToDisplay / 60);
      float seconds = Mathf.FloorToInt(_timeToDisplay % 60);

      timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
   }
}
