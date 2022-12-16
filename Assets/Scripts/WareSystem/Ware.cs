using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Ware", menuName = "Ware")]
public class Ware : ScriptableObject
{
	//Generall
	[Header ("General")]
	public new string name;
	public uint ID = 0;
	public uint value = 1;
	public Sprite SpriteOk;
	public Sprite SpriteDamaged;

	//Durability
	[Header ("Durability")]
	//public bool needsCooling;
	//public bool fragile;
	public uint durability;
}
