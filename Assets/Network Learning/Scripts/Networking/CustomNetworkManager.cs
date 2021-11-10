using A1.Player;

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.UI;

namespace Network_Learning.Scripts.Networking
{
   public class CustomNetworkManager : NetworkManager
   {
      /// <summary>
      ///  A reference to the CustomNetworkManager version of the singleton.
      /// </summary>
      public static CustomNetworkManager instance => singleton as CustomNetworkManager;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="_id"> The NetID of the player that we are trying to find. </param>
      /// <returns></returns>
      public static PlayerInteract FindPlayer(uint _id)
      {
         instance._players.TryGetValue(_id, out PlayerInteract player);
         return player;
      }

      /// <summary> Adds a player to the dictionary </summary>
      public static void AddPlayer([NotNull] PlayerInteract _player) => instance._players.Add(_player.netId, _player);
      
      /// <summary> Removes a player from the dictionary. </summary>
      public static void RemovePlayer([NotNull] PlayerInteract _player) => instance._players.Remove(_player.netId);

      public static PlayerInteract LocalPlayer
      {
         get
         {
            // If the internal local player is null
            if (localPlayer == null)
            {
               // Loop through each player in the game and check if it is a local player
               foreach (PlayerInteract networkPlayer in instance._players.Values)
               {
                  if (networkPlayer.isLocalPlayer)
                  {
                     // Set localPlayer to this player as it is the localPlayer
                     localPlayer = networkPlayer;
                     break;
                  }
               }
            }
            
            // Return the cached local player
            return localPlayer;
         }
      }
      
      private static PlayerInteract localPlayer = null;
      
      /// <summary> Whether or not this NetworkManager is the host. /// </summary>
      public bool isHost { get; private set; } = false;

      /// <summary>
      /// the dictionary of all connected players using their NetID as the key.
      /// </summary>
      public Dictionary<uint, PlayerInteract> _players = new Dictionary<uint, PlayerInteract>();

      public CustomNetworkDiscovery discovery;

      public bool coopMode;
      public Toggle toggleCoop;
      
      // THIS IS THE NEW FUNCTIONS FOR OUR PLAYER
      
      /// <summary> Adds a player to the dictionary </summary>
      public static void AddPlayerNew([NotNull] PlayerInteract _player) => instance._playersNew.Add(_player.netId, _player);
      
      /// <summary>
      /// the dictionary of all connected players using their NetID as the key.
      /// </summary>
      public Dictionary<uint, PlayerInteract> _playersNew = new Dictionary<uint, PlayerInteract>();

      

      /// <summary>
      /// the dictionary of all connected players using their NetID as the key.
      /// </summary>
      public Dictionary<uint, NetworkPlayer> _newPlayers = new Dictionary<uint, NetworkPlayer>();

      
      // THIS WAS WHERE THE OLD COOP BOOL WAS SET AND USED BEFORE IT WAS MOVED TO THE LOBBY.
      // /// <summary>
      // /// Setting variable
      // /// </summary>
      // /// <param name="_coopMode"></param>
      // public void BoolCheck(bool _coopMode)
      // {
      //    coopMode = _coopMode;
      // }
      //
      // public override void Start()
      // {
      //    if (toggleCoop != null)
      //    {
      //       toggleCoop.onValueChanged.AddListener(BoolCheck);
      //    }
      // }

      /// <summary>
      /// This is invoked when a host is started.
      ///  <para> Start host has multiple signatures, but they all cause this hook to be called.</para>
      /// </summary>
      public override void OnStartHost()
      {
         isHost = true;
         // This makes it visible on the network
         discovery.AdvertiseServer();
      }

      /// <summary> This is called when a host is stopped /// </summary>
      public override void OnStopHost()
      {
         isHost = false;
      }
   }
}