using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreSlot : MonoBehaviour
{
	//Editor Values
	[SerializeField] SpriteRenderer iconRenderer;
	[SerializeField] TextMeshProUGUI price;
	[SerializeField] Transform wareSpawnPos;

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
			float wareValue = ware.value;

			if (GameStatus.Me.CurrentMarketEvet != null)
				wareValue *= GameStatus.Me.CurrentMarketEvet.buyMod;

			if (wareValue <= gameStatus.GetPlayerMoney (playerId))
			{
				WareDisplay wDisplay = WareDisplay.CreateInstance (ware);

				if (wDisplay != null)
				{
					gameStatus.AddSilverCoins (playerId, -wareValue);
					PlayerStatsManager.Me.Stats (playerId).WareBought++;
					val = wDisplay.gameObject;

					//Spawns the ware at the slots position
					if (wareSpawnPos != null)
						val.transform.position = wareSpawnPos.position;
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
