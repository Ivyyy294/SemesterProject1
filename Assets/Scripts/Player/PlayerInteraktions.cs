using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerInteraktions: MonoBehaviour
{

	//Editor Values
	[SerializeField] float rayDistance;
	[SerializeField] GameObject dropIndicator;
	[SerializeField] Vector3 rotationOffset;
	[SerializeField] Transform warePos;
	[SerializeField] Transform center;

	//Private Values
	private Grid snapGrid;
	private WareDisplay grabbedObject;
	private Vector3 oScale;
	private int layerMask;
	private Vector3 dir;
	private uint playerId;
	private InputAction moveAction;
	private InputAction grabAction;
	private InputAction rotateAction;
	private bool indicatorRotated = false;
	
	//Public Functions
	public void InitInput(PlayerConfiguration pc)
	{
		moveAction = pc.Input.actions["Movement"];
		grabAction = pc.Input.actions["Grab"];
		rotateAction = pc.Input.actions ["Rotate"];
		playerId = (uint) pc.PlayerIndex;
	}

	public bool CarriesWare() { return grabbedObject != null;}

	public Ware GetCarriedWare () 	{ return Ware.GetFromGameObject (grabbedObject.gameObject);}

	//Private Functions
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
		if (moveAction != null)
		{
			Vector2 movementVec = moveAction.ReadValue <Vector2>();

			if (movementVec != Vector2.zero)
				dir = movementVec.normalized;
		}
		
		if (grabAction != null && grabAction.WasPressedThisFrame())
			CastRay ();

		bool indicatorActive = grabbedObject != null;

		if (indicatorActive && rotateAction != null && rotateAction.WasPerformedThisFrame ())
			RotateIndicator();

		if (indicatorActive != dropIndicator.activeInHierarchy)
			dropIndicator.SetActive (indicatorActive);
	}

	void CastRay ()
	{
		int layerMask = 1 << LayerMask.NameToLayer ("Interactables");
		Debug.DrawRay (center.transform.position, dir * rayDistance, Color.green, 1f);
		
		RaycastHit2D hitInfo = Physics2D.Raycast (center.transform.position, dir, rayDistance, layerMask);
		
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

	private void LateUpdate()
	{
		if (dropIndicator != null && dropIndicator.activeInHierarchy)
			MoveIndicatorPos();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (grabbedObject != null)
			grabbedObject.AddFragilityDmg();
	}

	void MoveIndicatorPos ()
	{
		if (snapGrid != null && dropIndicator.activeInHierarchy)
		{
			//Getting center of player cell
			Vector3 newPos = center.transform.position;
			
			//Getting Ware size
			Vector3 offset = grabbedObject.ware.GetSizeInWorld() * 0.5f;
			offset += snapGrid.cellSize;

			if (indicatorRotated)
				offset = new Vector3 (offset.y, offset.x);

			//Scaling with direction Vector
			offset.Scale (dir);

			newPos = GetCellCenterPos (newPos + offset);

			dropIndicator.transform.position = newPos;

			dropIndicator.GetComponent<DropIndicator>().RaycastCheck(center.position);
		}
	}

	void DropObject ()
	{
		Debug.Log ("DropObject");
		if (grabbedObject != null)
		{
			DropIndicator tmp = dropIndicator.GetComponent <DropIndicator>();

			if (tmp != null && tmp.IsDropAreaClear())
				ResetGrabbedObject (dropIndicator.transform.position);
		}
	}

	void GrabObject (GameObject obj)
	{
		if (obj != null)
		{
			grabbedObject = obj.GetComponent <WareDisplay>();

			if (grabbedObject != null && dropIndicator != null)
			{
				dropIndicator.SetActive (true);
				oScale = grabbedObject.transform.localScale;
				grabbedObject.PickUp (warePos.transform);

				//Setting Size of Drop indicator to Ware Size
				dropIndicator.transform.localScale = grabbedObject.ware.GetSizeInWorld();
			}
		}
	}

	private void InteractWare (GameObject obj)
	{
		//Player is only able to pick up Wares, when they dont carry an item
		if (grabbedObject == null)
			GrabObject (obj);
	}

	private void InteractStore (GameObject obj)
	{
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
				if (tmp.Interact (grabbedObject.gameObject, playerId))
					ReturnGrabbedObject ();
			}
		}
	}

	//Places the ware back on the ground
	private void ResetGrabbedObject (Vector3 pos)
	{ 
		grabbedObject.transform.localScale = oScale;
		grabbedObject.PlaceOnGround(pos);
		grabbedObject = null;
	}

	//Returns the ware to the pool
	private void ReturnGrabbedObject ()
	{ 
		grabbedObject.transform.localScale = oScale;
		grabbedObject.ReturnToPoolDeactivated();
		grabbedObject = null;
	}

	private void RotateIndicator ()
	{
		Vector2 size = grabbedObject.ware.GetSizeInWorld();

		//Rotation only possible when object is not symetrical
		if (size.x != size.y)
		{
			Vector3 euler = rotationOffset;

			if (indicatorRotated)
				euler *= -1f;

			dropIndicator.transform.Rotate (euler, Space.Self);
			grabbedObject.transform.Rotate (euler, Space.Self);
			indicatorRotated = !indicatorRotated;
		}

	}

	Vector3 GetCellCenterPos (Vector3 pos)
	{
		Vector3Int cp = snapGrid.WorldToCell (pos);
		return snapGrid.GetCellCenterWorld (cp);
	}
}
