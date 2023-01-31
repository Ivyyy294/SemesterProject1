using TMPro;
using UnityEngine;

public class PlayerUiNotebook : MonoBehaviour
{
	//Editor Values
	[SerializeField] TextMeshProUGUI valueText;
	[SerializeField] GameObject EmoteSad;	// > 15
	[SerializeField] GameObject EmoteHappy; // > 50
	[SerializeField] GameObject EmoteAngry; // < 15

	//Private Values
	Team team = null;

	//Public Functions
	public void SetTeam (Team t)
	{
		team = t;
	}

    // Update is called once per frame
    void Update()
    {
        if (valueText != null && team != null)
			valueText.text = ((int)team.Reputation).ToString();

		if (team.Reputation >= 50)
		{
			EmoteHappy.SetActive (true);
			EmoteSad.SetActive (false);
			EmoteAngry.SetActive (false);
		}
		else if (team.Reputation >= 15)
		{
			EmoteHappy.SetActive (false);
			EmoteSad.SetActive (true);
			EmoteAngry.SetActive (false);
		}
		else
		{
			EmoteHappy.SetActive (false);
			EmoteSad.SetActive (false);
			EmoteAngry.SetActive (true);
		}
    }
}
