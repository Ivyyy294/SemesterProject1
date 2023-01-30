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

    //Private Values

	private void Start()
	{
		if (PlayerManager.Me.MaxPlayers == 4)
			canvas.scaleFactor = fourPlayerUiScale;

		Team team = GameStatus.Me.GetTeamForPlayer ((uint)player.playerConfiguration.PlayerIndex);

		if (scroll != null)
			scroll.SetTeam (team);

		if (jar != null)
			jar.SetTeam (team);

		if (notebook != null)
			notebook.SetTeam (team);
	}
}
