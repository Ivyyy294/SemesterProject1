using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDisplay : MonoBehaviour
{
	//Editor values
	[SerializeField] List <Transform> warePos;
	[SerializeField] Ivyyy.AnimationData2D swimAnimation;

	//LaraValues
	public GameObject boardingRamp;

	//Private Values
	uint counterPlayersOnShip;
	Ivyyy.Animation2D animationPlayer = new Ivyyy.Animation2D();

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

	private void Start()
	{
		animationPlayer.Init (transform, swimAnimation);
		StartCoroutine (animationPlayer.Play());
	}

	void InitWares (Ship ship)
	{
		for (int i = 0; i < ship.wares.Count && i < warePos.Count; ++i)
		{
			WareDisplay wareDisplay = WareDisplay.CreateInstance (ship.wares[i]);

			if (wareDisplay != null)
			{
				wareDisplay.gameObject.transform.position = warePos[i].position;
				wareDisplay.gameObject.transform.SetParent (transform);
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
