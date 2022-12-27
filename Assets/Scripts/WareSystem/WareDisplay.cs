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

	//Private Values
	private double baseTimer = 0.0;
	private double extendetTimer = 0.0;
	private float collisionBufferTimer = 0f;
	private Dictionary <StoringAreaId, bool> storingAreas;

	//Public Functions
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
			|| (ware.fragility != Ware.Fragility.None && fragilityDmg >= (uint) ware.fragility); //Fragility limit exceeded
	}

	public void PlaceOnGround (Vector3 pos)
	{
		transform.position = pos;
		transform.SetParent (WarePool.Me.transform);
		gameObject.layer = 0;
		collisionBufferTimer = 0f;
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

		storingAreas = new Dictionary<StoringAreaId, bool>();

		foreach (StoringAreaId i in ware.storingAreaIds)
			storingAreas.Add (i, false);
	}

	// Start is called before the first frame update
	void Start()
	{
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
			SpriteRenderer r = GetComponent<SpriteRenderer>();

			if (r == null)
				Debug.Log ("SpriteRenderer missing!");
			else
			{
				if (damaged)
					r.sprite = ware.SpriteDamaged;
				else
					r.sprite = ware.SpriteOk;
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
		if (collisionBufferTimer >= collisionBuffer)
			fragilityDmg++;
	}
}
