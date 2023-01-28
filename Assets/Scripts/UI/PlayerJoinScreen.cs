using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoinScreen : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerManager.Me != null)
			PlayerManager.Me.EnablePlayerJoin (true);
	}
}
