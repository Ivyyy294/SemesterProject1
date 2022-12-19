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
	[SerializeField] GameObject dropIndicator;

	//Private Values
	private Grid snapGrid;
	private GameObject grabbedObject;
	private Vector3 oScale;
	private int layerMask;
	private Vector3 dir;
	private PlayerInput input;
	private InputAction moveAction;
	private uint playerId;

	//Public Functions
	public void CastRay (InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			int layerMask = 1 << 0;
			RaycastHit2D hitInfo = Physics2D.Raycast (transform.position, dir, rayDistance, layerMask);
			Debug.DrawRay (transform.position, dir * rayDistance, Color.green, 1f);

			if (grabbedObject != null)
			{
				if (hitInfo.collider != null && hitInfo.collider.CompareTag ("Merchant"))
					InteractMerchant (hitInfo.collider.gameObject);
				else
					DropObject();
			}
			else if (hitInfo.collider != null)
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
		}
	}

	void Start()
	{
		dir = Vector3.right;
		layerMask = LayerMask.NameToLayer ("Objects");
		dropIndicator.SetActive (false);

		snapGrid = GameStatus.Me.gameObject.GetComponentInChildren <Grid>();

		if (snapGrid == null)
			Debug.Log ("SnapGrid not found!");
	}

	// Update is called once per frame
	void Update()
	{
		if (input == null)
			InitInput();

		Vector2 movementVec = moveAction.ReadValue <Vector2>();

		if (movementVec != Vector2.zero)
			dir = movementVec.normalized;
	}

	private void LateUpdate()
	{
		MoveIndicatorPos();
	}

	private void InitInput()
	{
		input = GetComponent <PlayerInput>();
		moveAction = input.actions["Movement"];
		playerId = (uint) input.playerIndex;
	}

	void MoveIndicatorPos ()
	{
		if (dropIndicator != null && snapGrid != null)
		{
			Vector3Int cp = snapGrid.WorldToCell (transform.position + dir);
			Vector3 newPos = snapGrid.GetCellCenterWorld (cp);

			dropIndicator.transform.position = newPos; //snapGrid.GetCellCenterWorld (cp) + snapGrid.cellSize;
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
				oScale = grabbedObject.transform.localScale;
				grabbedObject.transform.localScale = new Vector3 (0.5f, 0.5f);
				grabbedObject.layer = 8;
			}
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
				GrabObject (storeSlot.BuyWare(playerId));
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
				if (tmp.Interact (grabbedObject, playerId))
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
		grabbedObject.transform.localScale = oScale;
		grabbedObject.transform.SetParent (WarePool.Me.transform);
		grabbedObject.layer = 0;
		grabbedObject = null;
	}
}
