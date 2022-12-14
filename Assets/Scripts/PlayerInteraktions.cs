using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerInteraktions: MonoBehaviour
{
	//Editor Values
	[SerializeField] KeyCode interactKey;
	[SerializeField] float rayDistance;
	[SerializeField] float rayOffset;
	[SerializeField] GameObject dropIndicator;
	[SerializeField] float snapGridSize;

	//Private Values
	private GameObject grabbedObject;
	private int layerMask;
	private Vector3 dir;
	private PlayerInputActions playerInputActions;

	//Private Functions
	private void Awake()
	{
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		playerInputActions.Player.Grab.performed += CastRay;
	}

	void Start()
	{
		dir = Vector3.right;
		layerMask = LayerMask.NameToLayer ("Objects");
		dropIndicator.SetActive (false);
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 input = playerInputActions.Player.Movement.ReadValue <Vector2>();

		if (input != Vector2.zero)
			dir = playerInputActions.Player.Movement.ReadValue <Vector2>();
	}

	private void LateUpdate()
	{
		MoveIndicatorPos();
	}

	float GetSnapOffset (float val)
	{
		float prefix = val < 0f ? -1f : 1;
		float newVal = Mathf.Abs (val);
		newVal = snapGridSize - newVal % snapGridSize;
		newVal *= prefix;
		return newVal;
	}

	void MoveIndicatorPos ()
	{
		if (dropIndicator != null)
		{
			float x = GetSnapOffset (transform.position.x);
			float y = GetSnapOffset (transform.position.y);
			Vector2 newPos = new Vector3 (x, y) + dir;
			dropIndicator.transform.localPosition = newPos;
		}
	}

	void DropObject ()
	{
		Debug.Log ("DropObject");
		if (grabbedObject != null)
		{
			DropIndicator tmp = dropIndicator.GetComponent <DropIndicator>();

			if (tmp != null && tmp.IsDropAreaClear())
			{
				grabbedObject.transform.position = dropIndicator.transform.position;
				ResetGrabbedObject ();
			}
		}
	}

	void GrabObject (GameObject obj)
	{
		if (obj != null)
		{
			grabbedObject = obj;

			if (dropIndicator != null)
			{
				dropIndicator.SetActive (true);
				grabbedObject.transform.position = transform.position;
				grabbedObject.transform.SetParent (transform);
				grabbedObject.transform.localScale = new Vector3 (0.5f, 0.5f);
				grabbedObject.layer = 8;
			}
		}
	}

	public void CastRay (InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			RaycastHit2D hitInfo = Physics2D.Raycast (transform.position + dir * rayOffset, dir, rayDistance);
			Debug.DrawRay (transform.position, dir, Color.green, 1f);

			if (hitInfo.collider != null)
			{
				if (hitInfo.collider.CompareTag ("Ware"))
					InteractWare(hitInfo.collider.gameObject);
				else if (hitInfo.collider.CompareTag ("Merchant"))
					InteractMerchant (hitInfo.collider.gameObject);
				else if (hitInfo.collider.CompareTag ("Store"))
					InteractStore (hitInfo.collider.gameObject);
				else if (grabbedObject != null)
					DropObject();
			}
			else if (grabbedObject != null)
				DropObject();
		}
	}

	private void InteractWare (GameObject obj)
	{
		Debug.Log ("InteractWare");

		//Player is only able to pick up Wares, when they dont carry an item
		if (grabbedObject == null)
			GrabObject (obj);
	}

	private void InteractStore (GameObject obj)
	{
		Debug.Log ("InteractStore");

		//Player is only able to buy, when they dont carry an item
		if (grabbedObject == null)
		{
			StoreSlot storeSlot = obj.GetComponent<StoreSlot>();

			if (storeSlot != null)
				GrabObject (storeSlot.BuyWare(0));
		}
	}

	private void InteractMerchant(GameObject gameObject)
	{
		Debug.Log ("InteractMerchant");

		if (gameObject != null)
		{
			MerchantDisplay tmp = gameObject.GetComponent<MerchantDisplay>();

			if (tmp != null && grabbedObject != null)
			{
				if (tmp.Interact (grabbedObject))
				{
					grabbedObject.SetActive (false);
					ResetGrabbedObject();
				}
			}
		}
	}

	private void ResetGrabbedObject()
	{ 
		dropIndicator.SetActive (false);
		grabbedObject.transform.localScale = new Vector3 (1f, 1f);
		grabbedObject.transform.SetParent (WarePool.Me.transform);
		grabbedObject.layer = 0;
		grabbedObject = null;
	}
}