using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Ware", menuName = "Ware")]
public class Ware : ScriptableObject
{
	public enum WeightCategory
	{
		Light,
		Medium,
		Heavy
	}

	//Generall
	[Header ("General")]
	public new string name;
	public uint ID = 0;
	public uint value = 1;
	public Sprite SpriteOk;
	public Sprite SpriteDamaged;
	public WeightCategory weight;

	//Durability
	[Header ("Durability")]
	//public bool needsCooling;
	//public bool fragile;
	public uint durability;

	public static Ware GetFromGameObject (GameObject obj)
	{
		Ware result = null;

		if (obj != null)
		{
			WareDisplay tmp = obj.GetComponent <WareDisplay>();
			
			if (tmp != null)
				result = tmp.ware;
		}

		return result;
	}
}
