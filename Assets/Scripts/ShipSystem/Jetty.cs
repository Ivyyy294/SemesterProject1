using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Jetty : MonoBehaviour
{
	//Editor Values
	[SerializeField] GameObject harbourBarrier;

	[Header ("ShipSettings")]
	[SerializeField] GameObject ship;
	[SerializeField] Ivyyy.Pathfinding2D shipPathfinding;
	[SerializeField] float shipDockTime;

	//Private Values
	ShipDisplay shipDisplay;

	//Public Values	
	public bool ShipDocked {get; private set;}
	public float timerShipDocked {get; private set;}
	public float timerInactice { get; private set;}

	//Public Functions
	public void CastOffShip ()
	{
		if (!shipDisplay.IsPlayerOnShip())
		{
			ShipDocked = false;

			string name = "CastOff";

			if (shipPathfinding != null && shipPathfinding.CurrentPath () != name)
				shipPathfinding.StartPath (name);
		}
	}

	public void SpawnShip(Ship obj)
	{
		Debug.Log ("Ship spawned!");

		string name = "Arriving";

		if (shipPathfinding != null && shipPathfinding.CurrentPath () != name)
			shipPathfinding.StartPath (name);

		ship.SetActive (true);
		
		if (shipDisplay != null)
			shipDisplay.Init (obj);

		timerInactice = 0f;
		timerShipDocked = 0f;
	}

	public bool IsShipActive () { return ship != null && ship.activeInHierarchy;}

	//Private Functions
	// Start is called before the first frame update
	void Start()
	{
		shipDisplay = ship.GetComponent<ShipDisplay>();
	}

	// Update is called once per frame
	void Update()
	{
		if (ship.activeInHierarchy)
		{
			string currentPath = shipPathfinding.CurrentPath();

			if (currentPath.Equals ("Arriving"))
			{
				if (shipPathfinding.CurrentPathDone())
				{
					ShipDocked = true;
					//Play sound
				}
			}
			else if (currentPath.Equals ("CastOff"))
			{
				if (shipPathfinding.CurrentPathDone())
					shipDisplay.DeactivateSafe();
			}

			if (ShipDocked)
			{
				if (timerShipDocked > shipDockTime && !currentPath.Equals ("CastOff"))
					CastOffShip();

				timerShipDocked += Time.deltaTime;
			}

			harbourBarrier.SetActive (!ShipDocked);
			shipDisplay.boardingRamp.SetActive (ShipDocked);
		}
		else
			timerInactice += Time.deltaTime;
	}
}
