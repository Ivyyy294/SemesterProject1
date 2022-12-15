using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbour : MonoBehaviour
{
	[SerializeField] List <Ship> ships;
	[SerializeField] List <Jetty> jetties;

	// Start is called before the first frame update
	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{
		foreach (Jetty i in jetties)
		{
			if (!i.IsShipActive())
			{
				if (ships.Count > 0)
					i.SpawnShip (ships[0]);
			}
		}
	}
}
