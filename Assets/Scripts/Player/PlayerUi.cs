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
		return new string ("Silver: " + t.SilverCoins.ToString());
	}
}
