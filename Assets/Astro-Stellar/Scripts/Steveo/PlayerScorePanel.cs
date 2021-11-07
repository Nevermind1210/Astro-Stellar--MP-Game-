using A1.Player;

using Mirror;

using System.Collections.Generic;
using UnityEngine;

using System;

using TMPro;


public class PlayerScorePanel : NetworkBehaviour
{
	public PlayerInteract player;
	public TMP_Text playerName;
	public TMP_Text playerScore;

	private void Update()
	{
		playerScore.text = player.personalScore.ToString();
	}
}
