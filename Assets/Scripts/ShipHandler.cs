using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
	//Editor Values
	[SerializeField] bool shipDocked;
	[SerializeField] GameObject harbourBarrier;

	[Header ("ShipSettings")]
	[SerializeField] GameObject ship;
	[SerializeField] Transform spawnPoint;
	[SerializeField] Transform destinationPoint;
	[SerializeField] float speed = 1.0f;

	//Private Values
	private float journeyLength;
	private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        journeyLength = Vector3.Distance (spawnPoint.position, destinationPoint.position);
		SpawnShip ();
    }

    // Update is called once per frame
    void Update()
    {
		if (ship.activeInHierarchy)
		{
			if (!shipDocked)
				MoveShip();

			harbourBarrier.SetActive (!shipDocked);
		}
    }

	private void SpawnShip()
	{
		ship.SetActive (true);
		ship.transform.position = spawnPoint.position;
		startTime = Time.time;
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
}
