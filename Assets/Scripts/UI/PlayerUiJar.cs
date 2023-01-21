using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUiJar : MonoBehaviour
{
	//Editor Values
	[SerializeField] TextMeshProUGUI silverCoinsTag;
	[SerializeField] GameObject silverCoins;
	
	//Private Values
	Team team = null;

	//Public Functions
	public void SetTeam (Team t)
	{
		team = t;
	}

    //Private Functions
    void Update()
    {
        if (silverCoinsTag != null && team != null)
			silverCoinsTag.text = ((int)team.SilverCoins).ToString();

		if (silverCoins != null)
			silverCoins.SetActive (team.SilverCoins > 15f);
    }
}
