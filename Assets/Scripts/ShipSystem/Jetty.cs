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
	[SerializeField] AudioClip audioShipArrived;
	[SerializeField] [Range (0f, 1f)] float playBellThreshold;

	//Private Values
	ShipDisplay shipDisplay;
	bool bellPlayed = false;

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
		string name = "Arriving";

		if (shipPathfinding != null && shipPathfinding.CurrentPath () != name)
			shipPathfinding.StartPath (name);

		ship.SetActive (true);
		
		if (shipDisplay != null)
			shipDisplay.Init (obj);

		timerInactice = 0f;
		timerShipDocked = 0f;
		bellPlayed = false;
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
			else
			{
				if (!bellPlayed && shipPathfinding.FractionOfJourney >= playBellThreshold)
				{
					Ivyyy.AudioHandler.Me.PlayOneShot (audioShipArrived);
					bellPlayed = true;
				}
			}

			harbourBarrier.SetActive (!ShipDocked);
			shipDisplay.boardingRamp.SetActive (ShipDocked);
		}
		else
			timerInactice += Time.deltaTime;
	}
}
