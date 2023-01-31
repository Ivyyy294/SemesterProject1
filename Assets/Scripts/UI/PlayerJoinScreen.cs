using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoinScreen : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerManager.Me != null)
			PlayerManager.Me.EnablePlayerJoin (true);

		//Ensures the time scale is rest to normal
		Time.timeScale = 1f;
	}
}
