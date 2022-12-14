using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSlot : MonoBehaviour
{
	//Editor Values
	[SerializeField] SpriteRenderer iconRenderer;

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
	}

	public GameObject BuyWare (uint playerId)
	{
		if (ware != null)
		{
			if (ware.value <= gameStatus.GetPlayerMoney (playerId))
			{
				GameObject obj = WarePool.Me.GetPooledObject ();

				if (obj != null)
				{
					obj.SetActive (true);

					WareDisplay wDisplay = obj.GetComponent<WareDisplay>();

					if (wDisplay != null)
					{
						wDisplay.Init (ware);
						gameStatus.AddSilverCoins (playerId, -ware.value);
						return obj;
					}
				}

			}
			else
				Debug.Log ("Not enough silver coins!");
		}
		
		return null;
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
