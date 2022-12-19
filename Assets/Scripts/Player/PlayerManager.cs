using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	[SerializeField]
	private Transform[] spawnPoints = new Transform[2];
	//bool waitingForPlayers = true;

	public void OnPlayerJoined (PlayerInput playerInput)
	{
		int playerId = playerInput.playerIndex;
		int teamId = playerId % 2;

		GameStatus.Me.AddPlayerToTeam ( (uint)playerId, teamId);

		GameObject player = playerInput.gameObject;
		SetLayerID (player, 6 + teamId);

		player.transform.position = spawnPoints[teamId].position;
		player.transform.SetParent (transform);

		//waitingForPlayers = false;
	}

	private void SetLayerID (GameObject obj, int layerId)
	{
		obj.layer = layerId;

		foreach (Transform t in obj.GetComponentInChildren <Transform>())
		{
			t.gameObject.layer = layerId;
		}
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
