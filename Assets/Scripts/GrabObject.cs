using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
	[SerializeField] KeyCode interactKey;
	[SerializeField] private Transform grabPoint;
	[SerializeField] private Transform dropPoint;
	[SerializeField] private Transform rayPoint;
	[SerializeField] float rayDistance;
	private GameObject grabbedObject;
	// Start is called before the first frame update
	void Start()
	{
	
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown (interactKey))
		{
			if (grabbedObject != null)
			{
				grabbedObject.transform.SetParent (null);
				grabbedObject.transform.position = dropPoint.position;
				grabbedObject = null;
			}
			else
			{
				RaycastHit hitInfo;
				
				if (Physics.Raycast (transform.position, rayPoint.localPosition, out hitInfo, rayDistance))
				{
					grabbedObject = hitInfo.collider.gameObject;
					grabbedObject.transform.position = grabPoint.position;
					grabbedObject.transform.SetParent (transform);
				}
			}
		}
	}
}
