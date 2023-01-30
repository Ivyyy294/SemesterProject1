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

    //Private Values

	private void Start()
	{
		Team team = GameStatus.Me.GetTeamForPlayer ((uint)player.playerConfiguration.PlayerIndex);

		if (scroll != null)
			scroll.SetTeam (team);

		if (jar != null)
			jar.SetTeam (team);

		if (notebook != null)
			notebook.SetTeam (team);
	}
}
