using UnityEngine;
using TMPro;

public class PlayerUi : MonoBehaviour
{
	//Editor Values
	[Header ("Lara Values")]
	[SerializeField] PlayerConfigurationDisplay player;
	[SerializeField] TextMeshProUGUI dayTime;
	[SerializeField] TextMeshProUGUI reputation;
	[SerializeField] TextMeshProUGUI silver;
	[SerializeField] TextMeshProUGUI tax;

    //Private Values
	uint playerId;

	private void Update()
	{
		if (dayTime != null)
			dayTime.text = GetDayTime();

		Team team = GameStatus.Me.GetTeamForPlayer ((uint) player.playerConfiguration.PlayerIndex);

		if (reputation != null)
			reputation.text = GetReputation (team);

		if (silver != null)
			silver.text = GetSilver(team);

		if (tax != null)
			tax.text = GetTax();
	}

	private string GetDayTime()
	{
		GameDateTime dateTime = GameStatus.Me.GetCurrentDateTime ();
		return new string ("Day: " + dateTime.day + " Time: " + dateTime.hour + ":" + dateTime.minute);
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
