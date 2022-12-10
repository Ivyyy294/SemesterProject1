using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class CrateDisplay : MonoBehaviour
{
	public Crate crate;

	public bool damaged = false;
	private double lifeTime = 0.0;

	public void CheckDurability ()
	{
		lifeTime += Time.deltaTime;

		if (crate.durability > 0 && crate.durability <= lifeTime)
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
				r.sprite = crate.SpriteDamaged;
			else
				r.sprite = crate.SpriteOk;
		}
	}
}
