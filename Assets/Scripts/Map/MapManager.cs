using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
	Lobby,
	Game
}

[System.Serializable]
public class MapContainer
{
	public GameState state;
	public GameObject mapPrefab;
}

public class MapManager : MonoBehaviour
{
	//EditorValues
	[SerializeField] List <MapContainer> maps;

	//Private Values
	int lastState = -1;
	GameState currentState = GameState.Lobby;
	GameObject currentActiveMap = null;

	private void Update()
	{
		if (PlayerManager.Me.AllPlayersReady())
			currentState = GameState.Game;

		if (lastState != (int) currentState)
		{
			foreach (MapContainer i in maps)
			{
				if (i.state == currentState)
				{
					ActivateMap (i.mapPrefab);
					break;
				}
			}
		} 

		lastState = (int) currentState;
	}

	private void ActivateMap (GameObject map)
	{
		if (currentActiveMap != null)
			Destroy (currentActiveMap);

		currentActiveMap = Instantiate (map, transform);
	}
}
