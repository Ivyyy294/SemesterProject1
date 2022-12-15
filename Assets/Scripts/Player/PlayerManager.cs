using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	[SerializeField]
	private List <Transform> spawnPoints;
	//bool waitingForPlayers = true;

	public void OnPlayerJoined (PlayerInput playerInput)
	{
		int playerId = playerInput.playerIndex;
		int teamId = playerId % 2;

		GameStatus.Me.AddPlayerToTeam ( (uint)playerId, teamId);

		playerInput.gameObject.layer = 6 + teamId;

		//waitingForPlayers = false;
	}

	//private void OnGUI()
	//{
	//	if (waitingForPlayers)
	//	{
	//		GUILayout.BeginArea(new Rect (Screen.width / 2, Screen.height / 2, 500, 500));
	//		GUILayout.Label ("Waiting for players!");

	//		GUILayout.EndArea();
	//	}
	//}
}
