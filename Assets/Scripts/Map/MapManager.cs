using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Map
{
	Lobby,
	Game,
	PlayerStats
}

[System.Serializable]
public class MapContainer
{
	public Map state;
	public GameObject mapPrefab;
}

public class MapManager : MonoBehaviour
{
	//EditorValues
	[SerializeField] List <MapContainer> maps;

	//Private Values
	Map currentMap = Map.Lobby;
	GameObject currentActiveMap = null;

	//Public Values
	public static MapManager Me { get; private set;}

	//Public Functions
	public Map CurrentMap () { return currentMap;}

	public void LoadMap (Map state)
	{
		foreach (MapContainer i in maps)
		{
			if (i.state == state)
			{
				currentMap = state;
				ActivateMap (i.mapPrefab);
				break;
			}
		}
	}

	//Private Functions
	private void Awake()
	{
		if (Me != null)
			Debug.Log ("Tyring o create another Instance of Singelton!");
		else
		{
			Me = this;
			LoadMap (Map.Lobby);
		}
	}

	private void ActivateMap (GameObject map)
	{
		if (currentActiveMap != null)
			Destroy (currentActiveMap);

		currentActiveMap = Instantiate (map, transform);

		Cursor.visible = currentMap != Map.Game;
	}
}
