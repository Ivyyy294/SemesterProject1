using UnityEngine;
using TMPro;

public class PlayerUi : MonoBehaviour
{
	//Editor Values
	[Header ("Lara Values")]
	[SerializeField] PlayerConfigurationDisplay player;
	[SerializeField] PlayerUiScroll scroll;
	[SerializeField] PlayerUiJar jar;
	[SerializeField] PlayerUiNotebook notebook;
	[SerializeField] float fourPlayerUiScale = 0.5f;
	[SerializeField] Canvas canvas;
	[SerializeField] GameObject won;
	[SerializeField] GameObject lost;

    //Private Values
	Team team;

	private void Start()
	{
		if (PlayerManager.Me.MaxPlayers == 4)
			canvas.scaleFactor = fourPlayerUiScale;

		team = GameStatus.Me.GetTeamForPlayer ((uint)player.playerConfiguration.PlayerIndex);

		if (scroll != null)
			scroll.SetTeam (team);

		if (jar != null)
			jar.SetTeam (team);

		if (notebook != null)
			notebook.SetTeam (team);
	}

	private void Update()
	{
		if (MapManager.Me != null && MapManager.Me.CurrentMap() == Map.Game)
		{
			if (PlayerStatsManager.Me.IndexTeamWon.Count > 0 
				|| PlayerStatsManager.Me.IndexTeamLose.Count > 0)
			{
				if (won != null)
					won.SetActive (team.HasWon());

				if (lost != null)
					lost.SetActive (team.HasLost());
			}
		}
		else
		{
			if (won != null)
				won.SetActive (false);

			if (lost != null)
				lost.SetActive (false);
		}
	}
}
