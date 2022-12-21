using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
	//Public Values
	public static PlayerManager Me { get; private set;}

	//Private Values
	private int MaxPlayers;
	private List <PlayerConfigurationDisplay> playerConfigs;

	//Public Functions
	public List <PlayerConfigurationDisplay> GetPlayerConfigs()
	{
		return playerConfigs;
	}

	public void OnPlayerJoined (PlayerInput playerInput)
	{
		if (!playerConfigs.Any (p => p.playerConfiguration.PlayerIndex == playerInput.playerIndex))
		{
			playerInput.transform.SetParent (transform);
			PlayerConfigurationDisplay pc = playerInput.gameObject.GetComponent <PlayerConfigurationDisplay>();
			pc.playerConfiguration = new PlayerConfiguration (playerInput);
			playerConfigs.Add (pc);
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
		PlayerConfiguration c = playerConfigs[index].playerConfiguration;
		c.TeamIndex = team;
		SetLayerID (c.Input.gameObject, 6 + team);
	}

	public void ReadyPlayer (int index)
	{
		playerConfigs[index].playerConfiguration.IsReady = true;

		if (playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.playerConfiguration.IsReady == true))
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
			playerConfigs = new List<PlayerConfigurationDisplay>();
			MaxPlayers = GetComponent <PlayerInputManager>().maxPlayerCount;
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