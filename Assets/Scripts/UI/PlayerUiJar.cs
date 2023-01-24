using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUiJar : MonoBehaviour
{
	//Editor Values
	[SerializeField] TextMeshProUGUI silverCoinsTag;
	[SerializeField] GameObject silverCoins;
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

		if (silverCoins != null)
			silverCoins.SetActive (team.SilverCoins > 15f);

		if (coinRain != null)
		{ 
			if (lastSilverCoins != team.SilverCoins)
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
