using A1.Player;

using Astro_Stellar;

using Mirror;

using System.Collections.Generic;
using UnityEngine;

using System;

using TMPro;


public class PlayerScorePanel : NetworkBehaviour
{
	[SyncVar] public uint playerNetId;
	public PlayerInteract player;
	public TMP_Text playerName;
	public TMP_Text playerScore;

	private void Start()
	{
		// player = NetworkServer.spawned[playerNetId].GetComponent<PlayerInteract>();
		
		transform.parent = FindObjectOfType<PlayerScores>().transform;
	}

	private void Update()
	{
		if(player == null)
		{
			player = NetworkIdentity.spawned[playerNetId].GetComponent<PlayerInteract>();
		}
		else
		{
			playerScore.text = player.personalScore.ToString();
		}
	}
}
