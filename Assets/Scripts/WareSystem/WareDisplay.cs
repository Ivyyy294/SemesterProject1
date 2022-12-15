using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class WareDisplay : MonoBehaviour
{
	//Public Values
	public Ware ware;
	public bool damaged = false;
	public bool isCooled = false;

	//Private Values
	private double lifeTime = 0.0;

	//Public Functions
	public Ware GetWare() { return ware;}

	public void Init (Ware w)
	{
		ware = w;
		Init();
	}

	public void CheckDurability ()
	{
		//Lifetime expires when not cooled
		if (!isCooled)
		{
			lifeTime += Time.deltaTime;

			if (ware.durability > 0 && ware.durability <= lifeTime)
				damaged = true;
		}
	}

	//Private Functions
	void Init ()
	{
		ChangeSprite();
		lifeTime = 0.0f;
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag ("Cooling"))
			isCooled = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag ("Cooling"))
			isCooled = false;
	}
}
