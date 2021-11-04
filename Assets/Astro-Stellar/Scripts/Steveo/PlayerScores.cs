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
		//[SerializeField] private GameObject playerScorePanel;
		public List<GameObject> scorePanels = new List<GameObject>();

		public SyncList<PlayerInteract> playerList = new SyncList<PlayerInteract>();
		
		
		public SyncList<PlayerScorePanel> panels = new SyncList<PlayerScorePanel>();
		private int index = 0;

		public override void OnStartServer()
		{
			foreach(GameObject scorePanel in scorePanels)
			{
				PlayerScorePanel playerScorePanel = scorePanel.GetComponent<PlayerScorePanel>();
				panels.Add(playerScorePanel);
			}
		}

		[Server]
		public void GetActivePlayers()
		{
			// panels.Clear();
			foreach(PlayerInteract player in playerList)
			{
				
				PlayerScorePanel playerScorePanel = panels[index];
				playerScorePanel.player = player;
				playerScorePanel.playerName.text = player.name;
				player.personalScoreText = playerScorePanel.playerScore;
				RpcDisplayPlayerPanel();
				index += 1;


				// Debug.Log("in the for loop");
				// GameObject scorePanel = Instantiate(playerScorePanel,this.transform);
				// scorePanel.gameObject.SetActive(true);
				// PlayerScorePanel panel = scorePanel.GetComponent<PlayerScorePanel>();
				// panel.player = player;
				// panel.playerName.text = player.name;
				// player.personalScoreText = panel.playerScore;
				// panels.Add(panel);
				// NetworkServer.Spawn(scorePanel);
			}
			Debug.Log("past the loop");
		}

		[ClientRpc]
		public void RpcDisplayPlayerPanel() => panels[index].gameObject.SetActive(true);

		private void Start()
		{
			//GetActivePlayers();
		}

		[Server]
		private void Update()
		{
			// foreach(PlayerScorePanel panel in panels)
			// {
			// 	panel.playerScore.text = panel.player.personalScore.ToString();
			// }
		}


		[Server]
		public void AddPlayer(PlayerInteract _player) => playerList.Add(_player);




		// private void OnConnectedToServer()
		// {
		// 	GetActivePlayers();
		// }
	}
}