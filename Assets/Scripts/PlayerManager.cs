using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	[SerializeField]
	private List <Transform> spawnPoints;


	public void OnPlayerJoined (PlayerInput playerInput)
	{
		int playerId = playerInput.playerIndex;
		int teamId = playerId % 2;

		GameStatus.Me.AddPlayerToTeam ( (uint)playerId, teamId);

		playerInput.gameObject.layer = 6 + teamId;

		//if (index < spawnPoints.Count)
		//	pTrans.position = spawnPoints[index].position;

		//PlayerConfig tmp = playerInput.transform.GetComponent <PlayerConfig>();

		//if (tmp != null)
		//	playerConfig.Add (tmp);
	}

	//private void SetLayerId(Transform trans, int layerId)
	//{
	//	foreach (Transform i in trans.GetComponentsInChildren<Transform>())
	//	{
	//		if (i.gameObject != null)
	//			i.gameObject.layer = layerId;
	//	}
	//}
}
