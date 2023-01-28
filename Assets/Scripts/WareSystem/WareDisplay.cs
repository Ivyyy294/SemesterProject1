using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent (typeof (BoxCollider2D))]
public class WareDisplay : MonoBehaviour
{
	//Public Values
	public Ware ware;
	[SerializeField] AudioClip audioStoredCorrectly;

	[Header ("Fragility")]
	public float speedThreshold = 0.9f;
	public float collisionCoolDown = 0.5f;

	[Header ("Animations")]
	[SerializeField] Ivyyy.AnimationData2D pickUpAnimationData;
	[SerializeField] Ivyyy.AnimationData2D dropAnimationData;

	[Header ("Lara Values")]
	public bool damaged = false;
	public bool isStoresCorrectly = false;
	public uint fragilityDmg;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] GameObject placeHolderPrefab;

	//Private Values
	private float collisionTimer = 0.0f;
	private double baseTimer = 0.0;
	private double extendetTimer = 0.0;
	private Dictionary <StoringAreaId, bool> storingAreas;
	Ivyyy.AudioHandler audioHandler;
	bool initFrame = true;
	PlayerStatsManager statsManager;
	private Ivyyy.Animation2D animationHandler = new Ivyyy.Animation2D();
	GameObject placeHolder;
	BoxCollider2D wareCollider;

	//Public Functions
	public static WareDisplay CreateInstance (Ware ware)
	{
		GameObject obj = WarePool.Me.GetPooledObject();
		WareDisplay wareDisplay = null;

		if (obj != null)
		{
			obj.SetActive (true);
			wareDisplay = obj.GetComponent<WareDisplay>();

			if (wareDisplay != null)
			{
				wareDisplay.ware = ware;
				wareDisplay.Init ();
			}
		}

		return wareDisplay;
	}

	public void ReturnToPool ()
	{
		transform.localScale = Vector3.one;
		gameObject.layer = LayerMask.NameToLayer ("Interactables");
		transform.SetParent (WarePool.Me.transform);
	}

	public void ReturnToPoolDeactivated ()
	{
		ReturnToPool();
		gameObject.SetActive (false);
	}

	public void CheckDurability ()
	{
		float timeOffset = Time.deltaTime;

		if (isStoresCorrectly)
		{
			//Ware durabilityExtended is unlimited when 0
			if (ware.durabilityExtended > 0f)
			{
				extendetTimer += timeOffset;
				baseTimer = ware.durability * (extendetTimer / ware.durabilityExtended);
			}
		}
		else
		{
			baseTimer += timeOffset;
			extendetTimer = ware.durabilityExtended * (baseTimer / ware.durability);
		}

		damaged = (ware.durability > 0f && baseTimer >= ware.durability) //Base durability expired
			|| (ware.durabilityExtended > 0f && extendetTimer >= ware.durabilityExtended)  //Extended durability expired
			|| (ware.fragility != Ware.Fragility.None && fragilityDmg >= ware.fragilityHp); //Fragility limit exceeded
	}

	public void PickUp (Transform destination)
	{
		gameObject.layer = LayerMask.NameToLayer ("NoCollision");
		transform.SetParent (destination);

		if (ware != null)
			Ivyyy.AudioHandler.Me.PlayOneShotFromList (ware.audiosPickUp);
		
		animationHandler.Init (transform, destination, pickUpAnimationData);
		StartCoroutine (animationHandler.Play());
	}

	public void PlaceOnGround (Vector3 pos)
	{
		//Prevents players from placing object on the same spot
		SpawnPlaceholder (pos);

		transform.SetParent (WarePool.Me.transform);

		animationHandler.Init (transform, pos, dropAnimationData);
		StartCoroutine (animationHandler.Play());

		//EnableRenderer (true);
		audioHandler.PlayOneShotFromList (ware.audiosPlaceDown);
	}

	//public void EnableRenderer (bool val)
	//{
	//	if (spriteRenderer != null)
	//	{
	//		spriteRenderer.enabled = val;
	//	}
	//}

	public Sprite GetActiveSprite()
	{
		if (spriteRenderer != null)
			return spriteRenderer.sprite;

		return null;
	}

	public void OnCollisionPlayer (GameObject playerObj, bool isCarried)
	{
		PlayerMotor playerMotor = playerObj.GetComponent <PlayerMotor>();
		
		if (playerMotor != null)
		{
			float playerSpeedFactor = playerMotor.GetCurrentSpeedFactor();
			//Plays the collision sound with a volume equal to the playerspeed
			audioHandler.PlayOneShotFromList (ware.audiosBump, playerSpeedFactor);

			if ((isCarried || ware.fragility == Ware.Fragility.Very)
				&& collisionTimer >= collisionCoolDown
				&& playerSpeedFactor >= speedThreshold)
			{
				Debug.Log ("CollisionTimer:" + collisionTimer.ToString());
				fragilityDmg++;
				collisionTimer = 0f;

				PlayerInteraktions playerInteraktions = playerObj.GetComponent <PlayerInteraktions>();
				
				if (playerInteraktions != null)
				{
					if (!damaged && fragilityDmg >= ware.fragilityHp && statsManager != null)
						statsManager.Stats(playerInteraktions.GetPlayerId()).WareDamaged++;
				}
			}			
		}
	}

	public void ChangeSprite()
	{
		if (ware != null)
		{
			//SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

			if (spriteRenderer == null)
				Debug.Log ("SpriteRenderer missing!");
			else
			{
				//Use the vertival sprites if available and the ware is rotated
				if (transform.rotation.z != 0f 
					&& ware.SpriteVerticalDamaged != null
					&& ware.SpriteVerticalOk != null)
				{
					if (damaged)
						spriteRenderer.sprite = ware.SpriteVerticalDamaged;
					else
						spriteRenderer.sprite = ware.SpriteVerticalOk;
				}
				else
				{
					if (damaged)
						spriteRenderer.sprite = ware.SpriteDamaged;
					else
						spriteRenderer.sprite = ware.SpriteOk;
				}
			}
		}
	}



	//Private Functions
	void Init ()
	{
		ChangeSprite();
		baseTimer = 0.0f;
		extendetTimer = baseTimer;
		collisionTimer = collisionCoolDown;
		damaged = false;
		fragilityDmg = 0;
		isStoresCorrectly = false;
		transform.rotation = new Quaternion();

		wareCollider = GetComponent<BoxCollider2D>();
		wareCollider.size = ware.GetSizeInWorld();

		//Adjusting renderer position to ware size
		if (spriteRenderer != null)
			spriteRenderer.gameObject.transform.localPosition = new Vector2 (0f, -wareCollider.size.y / 2);

		storingAreas = new Dictionary<StoringAreaId, bool>();

		foreach (StoringAreaId i in ware.storingAreaIds)
			storingAreas.Add (i, false);

		transform.localScale = Vector3.one;

		initFrame = true;
	}

	// Start is called before the first frame update
	void Start()
	{
		audioHandler = Ivyyy.AudioHandler.Me;
		statsManager = PlayerStatsManager.Me;
		Init ();
	}

	// Update is called once per frame
	void Update()
	{
		if (collisionTimer <= collisionCoolDown)
			collisionTimer += Time.deltaTime;

		if (ware != null)
		{
			if (!damaged)
				CheckDurability();

			ChangeSprite();
		}
		else
			Debug.Log ("Ware not set!");

		//Removes the active placeholder, once the drop animation is done
		//And set Layer back to Interactables
		if (placeHolder != null && animationHandler.Done)
		{
			gameObject.layer = LayerMask.NameToLayer ("Interactables");
			Destroy (placeHolder);
		}
	}

	private void LateUpdate()
	{
		//Prevents storedCorrectly sound from being played, when ware spawns in storage area
		if (initFrame && baseTimer > Time.fixedDeltaTime)
			initFrame = false;
	}

	private void SetStoringArea (GameObject obj, bool status)
	{
		StoringArea area = obj.GetComponent <StoringArea>();

		if (area != null && storingAreas.ContainsKey (area.areaId))
			storingAreas [area.areaId] = status;

		isStoresCorrectly = false;

		foreach (var i in storingAreas)
		{
			if (i.Value == true)
			{
				isStoresCorrectly = true;
				break;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag ("StoringArea"))
		{
			SetStoringArea (collision.gameObject, true);

			//Play audio if now stoed correctly
			if (isStoresCorrectly && !initFrame)
				Ivyyy.AudioHandler.Me.PlayOneShot (audioStoredCorrectly);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag ("StoringArea"))
			SetStoringArea (collision.gameObject, false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag ("Player"))
			OnCollisionPlayer (collision.gameObject, false);
	}

	private void SpawnPlaceholder (Vector3 pos)
	{
		if (placeHolderPrefab !=  null)
		{
			placeHolder = Instantiate (placeHolderPrefab, pos, new Quaternion());

			BoxCollider2D placeHolderCollider = placeHolder.GetComponent<BoxCollider2D>();

			if (wareCollider != null && placeHolder != null)
				placeHolderCollider.size = wareCollider.size;
		}
		else
			Debug.Log ("Missing Prefab placeHolderPrefab");
	}
}

#if UNITY_EDITOR
[CustomEditor (typeof (WareDisplay))]
public class WareDisplayEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		WareDisplay storeDisplay = (WareDisplay) target;
		storeDisplay.ChangeSprite();
	}
}
#endif