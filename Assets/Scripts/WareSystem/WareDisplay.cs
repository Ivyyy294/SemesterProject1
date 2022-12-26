using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class WareDisplay : MonoBehaviour
{
	//Public Values
	public Ware ware;
	public bool damaged = false;
	public bool isStoresCorrectly = false;

	//Private Values
	private double baseTimer = 0.0;
	private double extendetTimer = 0.0;
	private Dictionary <StoringAreaId, bool> storingAreas;

	//Public Functions
	public Ware GetWare() { return ware;}

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

		Debug.Log ("Base: " + baseTimer + " Extended: " + extendetTimer);
		damaged = baseTimer >= ware.durability || (ware.durabilityExtended > 0f && extendetTimer >= ware.durabilityExtended);
	}

	//Private Functions
	void Init ()
	{
		ChangeSprite();
		baseTimer = 0.0f;
		extendetTimer = baseTimer;
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
}
