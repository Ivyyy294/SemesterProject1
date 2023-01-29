using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUiJar : MonoBehaviour
{
	//Editor Values
	[SerializeField] TextMeshProUGUI silverCoinsTag;
	[SerializeField] GameObject silverCoins30;
	[SerializeField] GameObject silverCoins15;
	[SerializeField] GameObject silverCoins1;
	[SerializeField] GameObject coinRain;
	[SerializeField] float coinRainDuration;

	//Private
	float timer;
	float lastSilverCoins;

	//Private Values
	Team team = null;

	//Public Functions
	public void SetTeam (Team t)
	{
		team = t;
		lastSilverCoins = team.SilverCoins;
	}

    //Private Functions
    void Update()
    {
        if (silverCoinsTag != null && team != null)
			silverCoinsTag.text = ((int)team.SilverCoins).ToString();
		
		if (silverCoins30 != null)
			silverCoins30.SetActive (team.SilverCoins > 30f);

		if (silverCoins15 != null)
			silverCoins15.SetActive (team.SilverCoins > 15f);

		if (silverCoins1 != null)
			silverCoins1.SetActive (team.SilverCoins > 0f);

		if (coinRain != null)
		{ 
			//Plays coin rain when the team got silver
			if (lastSilverCoins < team.SilverCoins)
			{
				lastSilverCoins = team.SilverCoins;
				coinRain.SetActive (true);
				timer = 0f;
			}

			if (coinRain.activeInHierarchy && timer > coinRainDuration)
			{
				coinRain.SetActive (false);
				timer = 0f;
			}
			else
				timer += Time.deltaTime;
		}
    }
}
