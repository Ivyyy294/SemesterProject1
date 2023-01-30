using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatsScreen : MonoBehaviour
{
	//Editor Values
	[SerializeField] GameObject playerStatsPrefab;

	[Header ("Lara Values")]
	[SerializeField] GameObject rootLayout;
	[SerializeField] float fourPlayerUiScale = 0.75f;

    //Public Functions
	public void SpawnStatsPanel (PlayerStats stats)
	{
		if (rootLayout != null)
		{
			GameObject menu = Instantiate (playerStatsPrefab, rootLayout.transform);

			if (PlayerManager.Me.MaxPlayers == 4)
				menu.transform.localScale = new Vector3 (fourPlayerUiScale, fourPlayerUiScale);

			PlayerStatsPanel statsPanel = menu.GetComponent <PlayerStatsPanel>();

			if (statsPanel != null)
				statsPanel.Init (stats);
		}
	}

	public void SavePlayerStats()
	{
		PlayerStatsManager.Me.SaveToFile ();
	}

	public void GoToMenu ()
	{
		MapManager.Me.LoadMap (Map.Menu);
	}

	public void ButtonExit()
	{
		Application.Quit();
	}

	//Private Functions
	private void Start()
	{
		foreach (PlayerStats i in PlayerStatsManager.Me.Stats())
			SpawnStatsPanel (i);
	}
}
