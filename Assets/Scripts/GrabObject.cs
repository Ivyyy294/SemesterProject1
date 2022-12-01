using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
	[SerializeField] KeyCode interactKey;
	//[SerializeField] private Transform grabPoint;
	//[SerializeField] private Transform dropPoint;
	//[SerializeField] private Transform rayPoint;
	[SerializeField] float rayDistance;
	private GameObject grabbedObject;
	private bool castRay;
	private Vector3 dir;
	// Start is called before the first frame update
	void Start()
	{
		dir = Vector3.right;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.D))
			dir = Vector3.right;
		if (Input.GetKeyDown (KeyCode.A))
			dir = Vector3.left;
		if (Input.GetKeyDown (KeyCode.W))
			dir = Vector3.up;
		if (Input.GetKeyDown (KeyCode.S))
			dir = Vector3.down;

		if (grabbedObject != null)
			grabbedObject.transform.localPosition = dir + Vector3.back;

		if (Input.GetKeyDown (interactKey))
			castRay = true;
	}

	private void FixedUpdate()
	{
		if (castRay)
		{
			//Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
			//Debug.DrawRay(transform.position, forward, Color.green,1f);

			if (grabbedObject != null)
			{
				grabbedObject.transform.SetParent (null);
				grabbedObject.transform.position = transform.position + dir;
				grabbedObject = null;
			}
			else
			{
				RaycastHit hitInfo;
				Debug.DrawRay (transform.position, dir, Color.green, 1f);

				if (Physics.Raycast (transform.position, dir, out hitInfo, rayDistance))
				{
					grabbedObject = hitInfo.collider.gameObject;
					grabbedObject.transform.SetParent (transform);
					grabbedObject.transform.localPosition = dir + Vector3.back;
				}
			}

			castRay = false;
		}
		
	}
}
