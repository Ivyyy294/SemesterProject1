using TMPro;
using UnityEngine;

public class PlayerUiScroll : MonoBehaviour
{
	//Editor Values
	[SerializeField] TextMeshProUGUI day;
	[SerializeField] TextMeshProUGUI time;
	[SerializeField] TextMeshProUGUI tax;
	[SerializeField] GameObject startPos;
	[SerializeField] GameObject endPos;
	[SerializeField] GameObject pin;

	//Private Values
	Team team = null;

   //Public Functions
   public void SetTeam (Team t) {team = t;}

    //Private Values
    void Update()
    {
		if (team != null)
		{
			if (day != null)
				day.text = GetDay();

			if (time != null)
				time.text = GetTime();

			if (tax != null)
				tax.text = GetTaxToPay(team);

			if (pin != null)
				SetDayPinPosition();
		}
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
}
