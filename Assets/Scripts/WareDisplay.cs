using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class WareDisplay : MonoBehaviour
{
	public Ware ware;

	public bool damaged = false;
	private double lifeTime = 0.0;

	public void CheckDurability ()
	{
		lifeTime += Time.deltaTime;

		if (ware.durability > 0 && ware.durability <= lifeTime)
			damaged = true;
	}

    // Start is called before the first frame update
    void Start()
    {
		ChangeSprite ();
    }

    // Update is called once per frame
    void Update()
    {
		if (!damaged)
			CheckDurability();

		ChangeSprite();
    }

	void ChangeSprite()
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
