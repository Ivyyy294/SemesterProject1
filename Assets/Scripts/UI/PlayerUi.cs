using UnityEngine;
using TMPro;

public class PlayerUi : MonoBehaviour
{
	//Editor Values
	[Header ("Lara Values")]
	[SerializeField] PlayerConfigurationDisplay player;
	[SerializeField] PlayerUiScroll scroll;
	[SerializeField] PlayerUiJar jar;

    //Private Values
	uint playerId;

	private void Start()
	{
		Team team = GameStatus.Me.GetTeamForPlayer (playerId);

		if (scroll != null)
			scroll.SetTeam (team);

		if (jar != null)
			jar.SetTeam (team);
	}

	private string GetReputation (Team t)
	{
		return new string ("Reputation: " + ((int)t.Reputation).ToString());
	}

	private string GetSilver (Team t)
	{
		return new string ("Silver: " + ((int)t.SilverCoins).ToString());
	}


	private string GetTax ()
	{
		int currentDay = GameStatus.Me.GetCurrentDateTime ().day;
		int taxInterval = GameStatus.Me.cityTaxInterval;
		int taxDue = taxInterval - (currentDay % taxInterval);
		return new string ("City tax due in:" + taxDue.ToString());
	}
}
