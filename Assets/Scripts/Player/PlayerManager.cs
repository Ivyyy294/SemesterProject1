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
	private PlayerInputManager inputManager;

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
	}

	public bool AllPlayersReady ()
	{		
		if (playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.playerConfiguration.IsReady == true))
			return true;

		return false;
	}

	public void Reset()
	{
		foreach (PlayerConfigurationDisplay i in playerConfigs)
			Destroy (i.gameObject);

		playerConfigs.Clear();
	}

	//Private Functions
	private void Awake()
	{
		if (Me != null)
			Debug.Log ("Tyring o create another Instance of Singelton!");
		else
		{
			Me = this;
			//DontDestroyOnLoad (this);
			playerConfigs = new List<PlayerConfigurationDisplay>();
			inputManager = GetComponent <PlayerInputManager>();
			MaxPlayers = inputManager.maxPlayerCount;
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

	private void Update()
	{
		MapManager mapManager = MapManager.Me;

		if (mapManager != null)
		{
			if (mapManager.CurrentMap() == Map.Lobby && AllPlayersReady())
				MapManager.Me.LoadMap (Map.Game);
		}
	}
}