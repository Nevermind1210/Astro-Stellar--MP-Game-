

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
		// [SerializeField] private TMP_Dropdown characterDropdown;
		// [SerializeField] private List<Sprite> characterSprites = new List<Sprite>();
		// [SerializeField] private Image characterImage;
		// [SerializeField] private int index = 0;

		public PlayerScores playerScores;
		public bool coopMode;
		public Toggle toggleCoop;
		
		private void Awake()
		{
			startButton.interactable = CustomNetworkManager.instance.isHost;
			playerScores = FindObjectOfType<PlayerScores>();
			
			if (toggleCoop != null)
			{
				toggleCoop.onValueChanged.AddListener(BoolCheck);
				toggleCoop.interactable = CustomNetworkManager.instance.isHost;
			}
		}

		public void OnClickStartMatch()
		{
			// Don't need local player anymore here.
			//PlayerInteract localPlayer = CustomNetworkManager.LocalPlayer;
			
			// Start timer
			
			playerScores.GetActivePlayers();
			
			MatchManager.instance.StartMatch();
			
			gameObject.SetActive(false);
		}

		/// <summary>
		/// Calls functions on the player to change the character model.
		/// </summary>
		public void CharacterChoice()
		{
			// index = characterDropdown.value;
			// PlayerInteract localPlayer = CustomNetworkManager.LocalPlayer;
			// // localPlayer.modelIndex = index;
			// // localPlayer.CmdChangeModel(localPlayer.modelIndex);
			// characterImage.sprite = characterSprites[index];
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
			//CustomNetworkManager.instance.coopMode = _coopMode;
			MatchManager.instance.coopMode = _coopMode;
		}

		public void Start()
		{
			// if (toggleCoop != null)
			// {
			// 	toggleCoop.onValueChanged.AddListener(BoolCheck);
			// }
		}
	}
}