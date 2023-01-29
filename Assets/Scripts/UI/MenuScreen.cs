using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
	private PlayerManager playerManager;

	public void OnButtonPressed (int anzPlayer)
	{
		if (PlayerManager.Me != null)
		{
			PlayerManager.Me.SetPlayerCount (anzPlayer);

			if (MapManager.Me != null)
				MapManager.Me.LoadMap (Map.Lobby);
		}
	}

	private void Awake()
	{
		if (PlayerManager.Me != null)
		{
			PlayerManager.Me.Reset();
			PlayerManager.Me.EnablePlayerJoin (false);
		}
	}
}
