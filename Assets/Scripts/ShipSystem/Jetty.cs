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
	[SerializeField] Transform spawnPoint;
	[SerializeField] Transform destinationPoint;
	[SerializeField] float speed = 1.0f;

	//Private Values
	private float journeyLength;
	private float startTime;
	private float startTimeCastOff;
	bool shipDocked;
	bool shipCastOff;


	//Public Functions
	public void CastOffShip ()
	{
		if (!shipCastOff)
		{
			shipCastOff = true;
			shipDocked = false;
			startTimeCastOff = Time.time;
		}
	}

	public void SpawnShip(Ship obj)
	{
		Debug.Log ("Ship spawned!");

		ship.SetActive (true);
		ship.transform.position = spawnPoint.position;
		
		ShipDisplay shipDisplay = ship.GetComponent<ShipDisplay>();

		if (shipDisplay != null)
			shipDisplay.Init (obj);

		startTime = Time.time;

		shipCastOff = false;
	}

	public bool IsShipActive () { return ship != null && ship.activeInHierarchy;}
	public bool IsShipDocked () { return IsShipActive() && shipDocked;}
	public bool IsShipCastOff () { return shipCastOff;}

	//Private Functions
	// Start is called before the first frame update
	void Start()
	{
		journeyLength = Vector3.Distance (spawnPoint.position, destinationPoint.position);
	}

	// Update is called once per frame
	void Update()
	{
		if (ship.activeInHierarchy)
		{
			if (!shipDocked && !shipCastOff)
				MoveShip();
			else if (shipCastOff)
				MoveShipCastOff();

			harbourBarrier.SetActive (!shipDocked);
		}
	}

	private void MoveShip()
	{
		// Distance moved equals elapsed time times speed..
		float distCovered = (Time.time - startTime) * speed;

		// Fraction of journey completed equals current distance divided by total distance.
		float fractionOfJourney = distCovered / journeyLength;

		// Set our position as a fraction of the distance between the markers.
		ship.transform.position = Vector3.Lerp(spawnPoint.position, destinationPoint.position, fractionOfJourney);

		shipDocked = ship.transform.position == destinationPoint.position;
	}

	private void MoveShipCastOff()
	{
		// Distance moved equals elapsed time times speed..
		float distCovered = (Time.time - startTimeCastOff) * speed;

		// Fraction of journey completed equals current distance divided by total distance.
		float fractionOfJourney = distCovered / journeyLength;

		// Set our position as a fraction of the distance between the markers.
		ship.transform.position = Vector3.Lerp(destinationPoint.position, spawnPoint.position, fractionOfJourney);

		if (ship.transform.position == spawnPoint.position)
			ship.SetActive (false);
	}
}
