using A1.Player;

using System.Collections.Generic;
using UnityEngine;
using Mirror;

using System;

using TMPro;

[Serializable]
public class PlayerScorePanel : NetworkBehaviour
{
	public PlayerInteract player;
	public TMP_Text playerName;
	public TMP_Text playerScore;

}
