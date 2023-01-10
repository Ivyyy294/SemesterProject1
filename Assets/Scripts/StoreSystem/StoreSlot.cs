using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreSlot : MonoBehaviour
{
	//Editor Values
	[SerializeField] SpriteRenderer iconRenderer;
	[SerializeField] TextMeshProUGUI price;

	//private Values
	private Ware ware;
	private GameStatus gameStatus;

	//Public Functions
	public void SetWare (Ware w)
	{
		ware = w;

		if (iconRenderer != null)
			iconRenderer.sprite = ware.SpriteOk;
		else
			Debug.Log ("iconRenderer missing!");

		if (price != null)
			price.text = w.value.ToString();
		else
			Debug.Log ("Price text missing!");
	}

	public GameObject BuyWare (uint playerId)
	{
		GameObject val = null;

		if (ware != null)
		{
			if (ware.value <= gameStatus.GetPlayerMoney (playerId))
			{
				WareDisplay wDisplay = WareDisplay.CreateInstance (ware);

				if (wDisplay != null)
				{
					gameStatus.AddSilverCoins (playerId, -ware.value);
					val = wDisplay.gameObject;
				}
			}
			else
				Debug.Log ("Not enough silver coins!");
		}
		
		return val;
	}

    // Start is called before the first frame update
    void Start()
    {
        gameStatus = GameStatus.Me;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
