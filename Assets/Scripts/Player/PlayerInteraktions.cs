using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerInteraktions: MonoBehaviour
{

	//Editor Values
	[Header ("Emotes")]
	[SerializeField] Emote[] emotes;

	[Header ("Lara Values")]
	[SerializeField] float rayDistance;
	[SerializeField] GameObject dropIndicator;
	[SerializeField] Vector3 rotationOffset;
	[SerializeField] Transform warePos;
	[SerializeField] Transform center;
	[SerializeField] PlayerEmotes emoteHandler;
	[SerializeField] PlayerPauseMenu pauseMenu;
	[SerializeField] Vector2 boxraySize;

	//Private Values
	private Grid snapGrid;
	private WareDisplay grabbedObject;
	private Vector3 dir;
	private uint playerId;
	private bool indicatorRotated = false;

	//Input Actions
	private InputAction moveAction;
	private InputAction grabAction;
	private InputAction rotateAction;
	private InputAction[] emoteActions;
	private InputAction pauseAction;
	//Public Functions
	public uint GetPlayerId() { return playerId; }

	public void InitInput(PlayerConfiguration pc)
	{
		moveAction = pc.Input.actions["Movement"];
		grabAction = pc.Input.actions["Grab"];
		rotateAction = pc.Input.actions ["Rotate"];
		pauseAction = pc.Input.actions ["Pause"];

		int anzEmotes = emotes.Length;
		emoteActions = new InputAction[anzEmotes];

		for (int i = 0; i < anzEmotes; ++i)
		{
			string actionName = "Emote " + (i +1).ToString();
			emoteActions[i] = pc.Input.actions[actionName];
		}

		playerId = (uint) pc.PlayerIndex;
	}

	public bool CarriesWare() { return grabbedObject != null;}

	public Ware GetCarriedWare () 	{ return Ware.GetFromGameObject (grabbedObject.gameObject);}

	//Private Functions
	void Start()
	{
		dir = Vector3.right;
		dropIndicator.SetActive (false);

		snapGrid = GameStatus.Me.gameObject.GetComponentInChildren <Grid>();

		if (snapGrid == null)
			Debug.Log ("SnapGrid not found!");
	}

	// Update is called once per frame
	void Update()
	{
		if (GameStatus.Me != null && !GameStatus.Me.GamePaused)
		{
			if (pauseAction != null && pauseAction.WasPressedThisFrame() && pauseMenu != null)
				pauseMenu.Show();

			if (moveAction != null)
			{
				Vector2 movementVec = moveAction.ReadValue <Vector2>();

				if (movementVec != Vector2.zero)
					dir = movementVec.normalized;
			}
		
			//Moving Indicator position before CastRay, prevents wares from being misplaced
			if (dropIndicator != null && dropIndicator.activeInHierarchy)
				MoveIndicatorPos();

			if (grabAction != null && grabAction.WasPressedThisFrame())
				CastRay ();

			bool indicatorActive = grabbedObject != null;

			if (indicatorActive && rotateAction != null && rotateAction.WasPerformedThisFrame ())
				RotateIndicator();

			//Emotes
			if (emoteHandler != null)
			{
				for (int i = 0; i < emoteActions.Length; ++i)
				{
					InputAction action = emoteActions[i];

					if (action != null && action.WasPressedThisFrame())
						emoteHandler.PlayEmote (emotes[i]);
				}
			}
		}
	}

	RaycastHit2D GetHitInfo ()
	{
		int layerMask = 1 << LayerMask.NameToLayer ("Interactables")
			| 1 << LayerMask.NameToLayer ("Team1")
			| 1 << LayerMask.NameToLayer ("Team2");

		Debug.DrawRay (center.transform.position, dir * rayDistance, Color.green, 1f);

		RaycastHit2D hitInfo = new RaycastHit2D();
		hitInfo.distance = rayDistance;

		RaycastHit2D[] tmpInfos = Physics2D.BoxCastAll (center.transform.position, boxraySize, 0f, dir, rayDistance, layerMask);

		//Picking the closest object
		foreach (RaycastHit2D i in tmpInfos)
		{
			if (!i.collider.isTrigger
				&& !i.transform.IsChildOf (transform)
				&& i.distance < hitInfo.distance)
				hitInfo = i;
		}

		return hitInfo;
	}

	void CastRay ()
	{
		RaycastHit2D hitInfo = GetHitInfo ();

		if (grabbedObject == null)
			InteractionsFreeHands (hitInfo);
		else
			InteractionsWareGrabbed (hitInfo);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (grabbedObject != null)
			grabbedObject.OnCollisionPlayer (gameObject, true);
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
		if (grabbedObject != null)
		{
			DropIndicator tmp = dropIndicator.GetComponent <DropIndicator>();
			Vector3 dropPos = dropIndicator.transform.position;

			if (tmp != null && tmp.IsDropAreaClear())
				ResetGrabbedObject (dropPos);
		}
	}

	void GrabObject (GameObject obj)
	{
		if (obj != null)
		{
			dropIndicator.transform.rotation = obj.transform.rotation;

			indicatorRotated = obj.transform.rotation.z != 0f;

			grabbedObject = obj.GetComponent <WareDisplay>();

			if (grabbedObject != null && dropIndicator != null)
			{
				grabbedObject.PickUp (warePos);
				//Setting Size of Drop indicator to Ware Size
				dropIndicator.SetActive (true);
				dropIndicator.transform.localScale = grabbedObject.ware.GetSizeInWorld();

				PlayerStatsManager.Me.Stats (playerId).WarePickedUp++;
			}
		}
	}

	private void InteractionsFreeHands (RaycastHit2D hitinfos)
	{
		//Skip child objects
		if (hitinfos.collider != null)
		{
			if (hitinfos.collider.CompareTag ("Ware"))
				InteractWare(hitinfos.collider.gameObject);
			else if (hitinfos.collider.CompareTag ("Merchant"))
				InteractMerchant (hitinfos.collider.gameObject);
			else if (hitinfos.collider.CompareTag ("Store"))
				InteractStore (hitinfos.collider.gameObject);
			else if (hitinfos.collider.CompareTag ("Bell"))
				InteractBell (hitinfos.collider.gameObject);
		}
	}

	private void InteractBell(GameObject gameObject)
	{
		HarbourBell bell = gameObject.GetComponent<HarbourBell>();

		if (bell != null)
			bell.Interact();
	}

	private void InteractionsWareGrabbed (RaycastHit2D hitinfos)
	{
		bool dropWare = true;

		if (hitinfos.collider != null && hitinfos.collider.CompareTag ("Merchant"))
		{
			InteractMerchant (hitinfos.collider.gameObject);
			dropWare = false;
		}

		if (dropWare)
			DropObject();
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
			{
				GameObject tmp = storeSlot.BuyWare(playerId);
				GrabObject (tmp);
			}
		}
	}

	private void InteractMerchant(GameObject gameObject)
	{
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

	void ResetDropIndicator()
	{
		dropIndicator.transform.localScale = Vector3.one;

		dropIndicator.transform.rotation = Quaternion.Euler (Vector3.zero);
		indicatorRotated = false;

		dropIndicator.SetActive (false);
	}

	//Places the ware back on the ground
	private void ResetGrabbedObject (Vector3 pos)
	{ 
		grabbedObject.PlaceOnGround(pos);
		grabbedObject = null;
		ResetDropIndicator();
	}

	//Returns the ware to the pool
	private void ReturnGrabbedObject ()
	{ 
		grabbedObject.ReturnToPoolDeactivated();
		grabbedObject = null;
		ResetDropIndicator();
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
