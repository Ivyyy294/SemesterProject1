using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Crate", menuName = "Crate")]
public class Crate : ScriptableObject
{
	//Generall
	public new string name;
	public uint ID = 0;
	public uint value = 1;
	public Sprite SpriteOk;
	public Sprite SpriteDamaged;

	//Durability
	public uint durability;
	public bool damaged = false;

	private double lifeTime = 0.0;

	public void CheckDurability ()
	{
		lifeTime += Time.deltaTime;

		if (durability > 0 && durability <= lifeTime)
			damaged = true;
	}
}
