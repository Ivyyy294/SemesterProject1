using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
	protected bool isInRange;
	[SerializeField] KeyCode interactKey;
	[SerializeField] UnityEvent interactAction;

	private void Update()
	{
		if (isInRange)
		{
			if (Input.GetKeyDown (interactKey))
				interactAction.Invoke ();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			isInRange = true;

		Debug.Log ("In range");
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
			isInRange = false;

		Debug.Log ("left range");
	}
}
