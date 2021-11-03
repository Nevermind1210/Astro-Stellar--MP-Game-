using A1.Player;

using System.Collections.Generic;
using UnityEngine;
using Mirror;

using Network_Learning.Scripts.Networking;

using System;

using TMPro;

using NetworkPlayer = Network_Learning.Scripts.Networking.NetworkPlayer;

namespace Astro_Stellar
{
	/// <summary>
	/// This class handles displaying the individual player scores on the HUD.
	/// </summary>
	public class PlayerScores : NetworkBehaviour
	{
		private CustomNetworkManager instance = CustomNetworkManager.instance;

		[SerializeField] private GameObject playerScorePanel;
		[SerializeField] private TMP_Text playerName;
		[SerializeField] private TMP_Text playerScore;

		public void GetActivePlayers()
		{
			foreach(NetworkPlayer player in instance._players.Values)
			{
				Debug.Log("We do a little looping");
				Instantiate(playerScorePanel);
				playerName.text = player.name;
				playerScore.text = player.GetComponent<PlayerInteract>().personalScore.ToString();
			}
			Debug.Log("We done (: or haven't done a loop D:");
		}

		private void OnConnectedToServer()
		{
			GetActivePlayers();
		}
	}
}