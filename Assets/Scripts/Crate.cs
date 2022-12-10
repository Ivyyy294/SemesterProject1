using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class Crate : MonoBehaviour
{
	//Editor Values
	[Header ("General")]
	[Tooltip("Must be unique!")]
	[SerializeField] uint ID = 0;
	[SerializeField] uint value = 1;
	[SerializeField] Sprite SpriteOk;
	[SerializeField] Sprite SpriteDamaged;

	[Header ("Durability Settings")]
	[SerializeField] bool damaged = false;
	//[SerializeField] bool needsCooling = false;
	[Tooltip("Durability in seconds")]
	[SerializeField] uint durability;

	//Private Values
	double lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
				r.sprite = SpriteDamaged;
			else
				r.sprite = SpriteOk;
		}
	}

	void CheckDurability()
	{
		lifeTime += Time.deltaTime;

		if (durability > 0 && durability <= lifeTime)
			damaged = true;
	}
}
