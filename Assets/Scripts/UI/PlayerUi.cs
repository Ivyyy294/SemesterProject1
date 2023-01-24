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
	uint playerId;

	private void Start()
	{
		Team team = GameStatus.Me.GetTeamForPlayer (playerId);

		if (scroll != null)
			scroll.SetTeam (team);

		if (jar != null)
			jar.SetTeam (team);

		if (notebook != null)
			notebook.SetTeam (team);
	}
}
