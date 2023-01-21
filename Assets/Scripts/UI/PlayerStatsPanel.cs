using UnityEngine;
using TMPro;

public class PlayerStatsPanel : MonoBehaviour
{
	//Editore Values
	[Header ("General")]
	[SerializeField] TextMeshProUGUI player;
	[SerializeField] GameObject won;
	[SerializeField] GameObject lost;

	[Header ("Silver")]
	[SerializeField] TextMeshProUGUI silverEarned;
	[SerializeField] TextMeshProUGUI silverLost;
	[SerializeField] TextMeshProUGUI tax;

	[Header ("Reputation")]
	[SerializeField] TextMeshProUGUI reputationEarned;
	[SerializeField] TextMeshProUGUI reputationLost;

	[Header ("Ware")]
	[SerializeField] TextMeshProUGUI wareCarried;
	[SerializeField] TextMeshProUGUI wareDamaged;

	[Header ("Merchants")]
	[SerializeField] TextMeshProUGUI request;
	[SerializeField] TextMeshProUGUI wareSold;
	[SerializeField] TextMeshProUGUI wareBought;

	//Private values
	PlayerStats stats;

	//Public Functions
	public void Init (PlayerStats _stats)
	{
		stats = _stats;

		//General
		AppendText (player, (stats.playerIndex + 1).ToString());

		if (PlayerStatsManager.Me.IndexTeamWon == stats.teamIndex)
		{
			if (lost != null)
				lost.SetActive (false);
		}
		else
		{
			if (won != null)
				won.SetActive (false);
		}

		//Silver
		AppendText (silverEarned, stats.SilverEarnedTotal.ToString());
		AppendText (silverLost, stats.SilverSpentTotal.ToString());
		AppendText (tax, Ivyyy.Utils.RoundF (stats.TaxPayed, 2).ToString());

		//Reputation
		AppendText (reputationEarned, ((int)stats.reputationEarnedTotal).ToString());
		AppendText (reputationLost, stats.reputationLostTotal.ToString());

		//Ware
		AppendText (wareCarried, stats.WarePickedUp.ToString());
		AppendText (wareDamaged, stats.WareDamaged.ToString());

		//Merchant
		AppendText (request, stats.RequestCompleted.ToString());
		AppendText (wareSold, stats.WareSold.ToString());
		AppendText (wareBought, stats.WareBought.ToString());
	}

	//Private Functions
	void AppendText (TextMeshProUGUI obj, string text)
	{
		if (obj != null)
			obj.text += text;
		else
			Debug.Log ("Missing TextMeshProUGUI element!");

	}
}
