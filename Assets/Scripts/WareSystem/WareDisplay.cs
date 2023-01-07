using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class WareDisplay : MonoBehaviour
{
	//Public Values
	public Ware ware;

	[Header ("Lara Values")]
	public bool damaged = false;
	public bool isStoresCorrectly = false;
	public uint fragilityDmg;
	[SerializeField] float collisionBuffer = 0.1f;
	[SerializeField] SpriteRenderer spriteRenderer;

	//Private Values
	private double baseTimer = 0.0;
	private double extendetTimer = 0.0;
	private float collisionBufferTimer = 0f;
	private Dictionary <StoringAreaId, bool> storingAreas;
	Ivyyy.AudioHandler audioHandler;

	//Public Functions
	public void ReturnToPool ()
	{
		Debug.Log ("Ware released back to pool");
		transform.SetParent (WarePool.Me.transform);
	}

	public void ReturnToPoolDeactivated ()
	{
		ReturnToPool();
		gameObject.SetActive (false);
	}

	public void Init (Ware w)
	{
		ware = w;
		Init();
	}

	public void CheckDurability ()
	{
		float timeOffset = Time.deltaTime;

		if (isStoresCorrectly)
		{
			extendetTimer += timeOffset;
			baseTimer = ware.durability * (extendetTimer / ware.durabilityExtended);
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

	public void PickUp (Transform parent)
	{
		transform.position = parent.position;
		transform.SetParent (parent);
		transform.localScale = new Vector3 (0.5f, 0.5f);
		gameObject.layer = LayerMask.NameToLayer ("NoCollision");
		audioHandler.PlayOneShotFromList (ware.audiosPickUp);
	}

	public void PlaceOnGround (Vector3 pos)

	{
		transform.position = pos;
		transform.SetParent (WarePool.Me.transform);
		gameObject.layer = LayerMask.NameToLayer ("Interactables");
		collisionBufferTimer = 0f;

		audioHandler.PlayOneShotFromList (ware.audiosPlaceDown);
	}

	public void AddFragilityDmg ()
	{
		fragilityDmg++;
		audioHandler.PlayOneShotFromList (ware.audiosBump);
	}

	//Private Functions
	void Init ()
	{
		ChangeSprite();
		baseTimer = 0.0f;
		extendetTimer = baseTimer;
		damaged = false;
		fragilityDmg = 0;
		collisionBufferTimer = 0f;
		isStoresCorrectly = false;
		transform.rotation = new Quaternion();

		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		collider.size = ware.GetSizeInWorld();

		//Adjusting renderer position to ware size
		if (spriteRenderer != null)
			spriteRenderer.gameObject.transform.localPosition = new Vector2 (0f, -collider.size.y / 2);

		storingAreas = new Dictionary<StoringAreaId, bool>();

		foreach (StoringAreaId i in ware.storingAreaIds)
			storingAreas.Add (i, false);
	}

	// Start is called before the first frame update
	void Start()
	{
		audioHandler = Ivyyy.AudioHandler.Me;
		Init();
	}

	// Update is called once per frame
	void Update()
	{
		if (collisionBufferTimer < collisionBuffer)
			collisionBufferTimer += Time.deltaTime;

		if (ware != null)
		{
			if (!damaged)
				CheckDurability();

			ChangeSprite();
		}
		else
			Debug.Log ("Ware not set!");
	}

	void ChangeSprite()
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

	private void SetStoringArea (GameObject obj, bool status)
	{
		StoringArea area = obj.GetComponent <StoringArea>();

		if (area != null && storingAreas.ContainsKey (area.areaId))
			storingAreas [area.areaId] = status;

		bool tmp = true;

		foreach (var i in storingAreas)
			tmp &= i.Value;

		isStoresCorrectly = tmp;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag ("StoringArea"))
			SetStoringArea (collision.gameObject, true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag ("StoringArea"))
			SetStoringArea (collision.gameObject, false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (ware.fragility == Ware.Fragility.Very && collisionBufferTimer >= collisionBuffer)
			AddFragilityDmg();
	}
}
