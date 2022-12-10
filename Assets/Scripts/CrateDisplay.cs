using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class CrateDisplay : MonoBehaviour
{
	public Crate crate;

    // Start is called before the first frame update
    void Start()
    {
		ChangeSprite ();
    }

    // Update is called once per frame
    void Update()
    {
		if (!crate.damaged)
			crate.CheckDurability();

		ChangeSprite();
    }

	void ChangeSprite()
	{
		SpriteRenderer r = GetComponent<SpriteRenderer>();

		if (r == null)
			Debug.Log ("SpriteRenderer missing!");
		else
		{
			if (crate.damaged)
				r.sprite = crate.SpriteDamaged;
			else
				r.sprite = crate.SpriteOk;
		}
	}
}
