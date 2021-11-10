

using A1.Player;

using Astro_Stellar;

using Mirror;

using Network_Learning.Scripts.Networking;

using NetworkGame.Networking;

using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;


namespace Networking.Scripts
{
	public class Lobby: MonoBehaviour
	{
		[SerializeField] private Button startButton;
		[Header("Character Variables")]
		[SerializeField] private TMP_InputField characterNameInput;
		
		[Header("Match Settings")]
		public Toggle toggleCoop;
		public bool coopMode = true;

		public PlayerScores playerScores;
		
		private void Awake()
		{
			startButton.interactable = CustomNetworkManager.instance.isHost;
			playerScores = FindObjectOfType<PlayerScores>();
			coopMode = true;
			MatchManager.instance.coopMode = coopMode;

			if (toggleCoop != null)
			{
				toggleCoop.onValueChanged.AddListener(BoolCheck);
				toggleCoop.interactable = CustomNetworkManager.instance.isHost;
			}
		}

		public void OnClickStartMatch()
		{
			playerScores.GetActivePlayers();
			
			MatchManager.instance.StartMatch();
			
			gameObject.SetActive(false);

			CountdownToLose count = FindObjectOfType<CountdownToLose>();
			count.timerRunning = true;
		}

		

		/// <summary>
		/// Calls functions on the player to change character name.
		/// </summary>
		public void CharacterName()
		{
			PlayerInteract localPlayer = CustomNetworkManager.LocalPlayer;
			localPlayer.CmdCharacterName(characterNameInput.text);
		}
		
		/// <summary>
		/// Setting variable
		/// </summary>
		/// <param name="_coopMode"></param>
		public void BoolCheck(bool _coopMode)
		{
			coopMode = _coopMode;
			MatchManager.instance.coopMode = coopMode;
		}

		
	}
}