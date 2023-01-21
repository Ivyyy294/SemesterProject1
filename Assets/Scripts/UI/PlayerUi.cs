using UnityEngine;
using TMPro;

public class PlayerUi : MonoBehaviour
{
	//Editor Values
	[Header ("Scroll")]
	[SerializeField] TextMeshProUGUI day;
	[SerializeField] TextMeshProUGUI time;
	[SerializeField] TextMeshProUGUI tax;
	[SerializeField] GameObject startPos;
	[SerializeField] GameObject endPos;
	[SerializeField] GameObject pin;

	[Header ("Lara Values")]
	[SerializeField] PlayerConfigurationDisplay player;

	//OLD
	[SerializeField] TextMeshProUGUI reputation;
	[SerializeField] TextMeshProUGUI silver;

    //Private Values
	uint playerId;

	private void Update()
	{
		if (day != null)
			day.text = GetDay();

		if (time != null)
			time.text = GetTime();

		Team team = GameStatus.Me.GetTeamForPlayer ((uint) player.playerConfiguration.PlayerIndex);
		
		if (tax != null)
			tax.text = GetTaxToPay(team);

		SetDayPinPosition();

		//OLD

		if (reputation != null)
			reputation.text = GetReputation (team);

		if (silver != null)
			silver.text = GetSilver(team);

	}

	private string GetDay()
	{
		GameDateTime dateTime = GameStatus.Me.GetCurrentDateTime ();
		return new string ("Day " + dateTime.day);
	}

	private string GetTime()
	{
		GameDateTime dateTime = GameStatus.Me.GetCurrentDateTime ();
		return new string (dateTime.hour + ":" + dateTime.minute);
	}

	private string GetReputation (Team t)
	{
		return new string ("Reputation: " + ((int)t.Reputation).ToString());
	}

	private string GetSilver (Team t)
	{
		return new string ("Silver: " + ((int)t.SilverCoins).ToString());
	}

	private string GetTaxToPay (Team t)
	{
		return new string ("Next\nTax:\n" + ((int)t.tax.TaxToPay).ToString());
	}

	private void SetDayPinPosition ()
	{
		GameDateTime dateTime = GameStatus.Me.GetCurrentDateTime ();
		int currentHour = int.Parse (dateTime.hour);

		int taxInterval = GameStatus.Me.cityTaxInterval;
		float taxIntervalHour = taxInterval * 24f;

		int dayInIntervall = dateTime.day % taxInterval;
		float hourInIntervall = dayInIntervall * 24f + currentHour;

		float factor = hourInIntervall / taxIntervalHour;

		pin.transform.position = Vector3.Lerp (startPos.transform.position, endPos.transform.position, factor);
	}

	private string GetTax ()
	{
		int currentDay = GameStatus.Me.GetCurrentDateTime ().day;
		int taxInterval = GameStatus.Me.cityTaxInterval;
		int taxDue = taxInterval - (currentDay % taxInterval);
		return new string ("City tax due in:" + taxDue.ToString());
	}
}
