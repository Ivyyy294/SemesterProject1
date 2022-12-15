using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDisplay : MonoBehaviour
{
	//Editor values
	[SerializeField] List <Transform> warePos;

	//Public Functions
	public void Init (Ship ship)
	{
		InitWares(ship);
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
}
