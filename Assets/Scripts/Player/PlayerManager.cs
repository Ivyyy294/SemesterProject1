using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerConfiguration
{
	public PlayerInput Input { get; set;}
	public int PlayerIndex {get; set;}
	public int TeamIndex {get; set;}
	public bool IsReady { get; set; }
	public Vector3 SpawnPoint {get; set;}

	public PlayerConfiguration (PlayerInput pi)
	{
		PlayerIndex = pi.playerIndex;
		Input = pi;
	}
}

public class PlayerManager : MonoBehaviour
{
	//Public Values
	public static PlayerManager Me { get; private set;}

	//Editor Values
	[SerializeField] int MaxPlayers = 2;

	//Private Values
	private List <PlayerConfiguration> playerConfigs;

	//Public Functions
	public void OnPlayerJoined (PlayerInput playerInput)
	{
		if (!playerConfigs.Any (p => p.PlayerIndex == playerInput.playerIndex))
		{
			playerInput.transform.SetParent (transform);
			playerConfigs.Add (new PlayerConfiguration (playerInput));
		}


		//int playerId = playerInput.playerIndex;
		//int teamId = playerId % 2;

		//GameStatus.Me.AddPlayerToTeam ( (uint)playerId, teamId);

		//GameObject player = playerInput.gameObject;
		//player.transform.SetParent (transform);
		//player.transform.position = spawnPoints[teamId].position;
		//player.SetActive (false);
		//waitingForPlayers = false;
	}

	public void SetPlayerTeam (int index, int team)
	{
		PlayerConfiguration c = playerConfigs[index];
		c.TeamIndex = team;
		SetLayerID (c.Input.gameObject, 6 + team);
	}

	public void ReadyPlayer (int index)
	{
		playerConfigs[index].IsReady = true;

		if (playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.IsReady == true))
		{
			SceneManager.LoadScene ("Town Test");
		}
	}

	//Private Functions
	private void Awake()
	{
		if (Me != null)
			Debug.Log ("Tyring o create another Instance of SIngelton!");
		else
		{
			Me = this;
			DontDestroyOnLoad (Me);
				playerConfigs = new List<PlayerConfiguration>();
		}
	}

	private void SetLayerID (GameObject obj, int layerId)
	{
		obj.layer = layerId;

		foreach (Transform t in obj.GetComponentInChildren <Transform>())
		{
			t.gameObject.layer = layerId;
		}
	}
}