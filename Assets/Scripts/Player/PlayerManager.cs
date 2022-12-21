using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

enum GameState
{
	Lobby,
	Game
}

[RequireComponent (typeof (PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
	//EditorValues
	[SerializeField] GameObject mapPrefab;
	[SerializeField] GameObject playerLobbyPrefab;

	//Public Values
	public static PlayerManager Me { get; private set;}

	//Private Values
	GameObject mapInstance;
	GameObject lobbyInstance;

	private int MaxPlayers;
	private List <PlayerConfigurationDisplay> playerConfigs;
	GameState currentState = GameState.Lobby;

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
			currentState = GameState.Game;
	}

	//Private Functions
	private void Awake()
	{
		if (Me != null)
			Debug.Log ("Tyring o create another Instance of Singelton!");
		else
		{
			Me = this;
			DontDestroyOnLoad (Me);
			playerConfigs = new List<PlayerConfigurationDisplay>();
			MaxPlayers = GetComponent <PlayerInputManager>().maxPlayerCount;
		}
	}

	private void Update()
	{
		if (currentState == GameState.Lobby && lobbyInstance == null)
		{
			lobbyInstance = Instantiate (playerLobbyPrefab, transform);

			if (mapInstance != null)
				Destroy (mapInstance);
		}
		else if (currentState == GameState.Game && mapInstance == null)
		{
			mapInstance = Instantiate (mapPrefab, transform);

			if (lobbyInstance != null)
				Destroy (lobbyInstance);
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