using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDisplay : MonoBehaviour
{
	//Editor values
	[SerializeField] List <Transform> warePos;

	//LaraValues
	public GameObject boardingRamp;

	//Private Values
	uint counterPlayersOnShip;

	//Public Functions
	public bool IsPlayerOnShip()
	{
		return counterPlayersOnShip > 0;
	}

	public void Init (Ship ship)
	{
		counterPlayersOnShip = 0;
		InitWares(ship);
	}

	public void DeactivateSafe()
	{
		//Return all left wares to pool
		foreach (WareDisplay i in GetComponentsInChildren <WareDisplay>())
			i.ReturnToPoolDeactivated();

		gameObject.SetActive (false);
	}

	//Private Functions

	void InitWares (Ship ship)
	{
		for (int i = 0; i < ship.wares.Count && i < warePos.Count; ++i)
		{
			GameObject obj = WarePool.Me.GetPooledObject();

			if (obj != null)
			{
				obj.SetActive (true);
				obj.transform.position = warePos[i].position;
				obj.transform.SetParent (transform);

				WareDisplay wareDisplay = obj.GetComponent<WareDisplay>();
				wareDisplay.Init (ship.wares[i]);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
			++counterPlayersOnShip;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
			--counterPlayersOnShip;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag ("Ware"))
		{
			if (!collision.transform.parent.CompareTag ("Player"))
			{
				collision.transform.SetParent (transform);
			}
		}
	}
}
